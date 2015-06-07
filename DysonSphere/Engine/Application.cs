﻿using System;
using System.Collections;
using System.Collections.Generic;
using Engine.Controllers.Events;
using Engine.Controllers;
using Engine.Controllers.Net;
using Engine.Models;
using Engine.Utils;
using Engine.Utils.Settings;
using Engine.Views;
using Timer = System.Windows.Forms.Timer;

namespace Engine
{
	public enum AppType
	{
		local,
		client,
		server,
		client_server=local
	}

	public class Application
	{
		#region Основные переменные

		/// <summary>
		/// Главный Таймер
		/// </summary>
		private readonly Timer _mainTimer;

		/// <summary>
		/// Модель
		/// </summary>
		private readonly Model _model;

		/// <summary>
		/// Вид
		/// </summary>
		private readonly View _view;

		/// <summary>
		/// Контроллер
		/// </summary>
		private readonly Controller _controller;

		/// <summary>
		/// Визуализатор
		/// </summary>
		private readonly VisualizationProvider _visualizationProvider;

		/// <summary>
		/// Устройства ввода
		/// </summary>
		private readonly Input _input;

		/// <summary>
		/// Звук
		/// </summary>
		private readonly Sound _sound;

		/// <summary>
		/// Собиратель объектов из сборок
		/// </summary>
		private readonly Collector _collector;

		///// <summary>
		///// Параметры для запуска модулей. каждый модуль может хранить свои параметры
		///// </summary>
		//private ModuleParams parameters;

		/// <summary>
		/// Отправка данных клиенту
		/// </summary>
		public static DataSender _toModelDataSender;

		public static DataSender _toViewDataSender;

		/// <summary>
		/// Тип запущенного приложения
		/// </summary>
		/// <remarks>Предполагаю что иногда нужно будет узнать что за приложение запущено, например для расширения функционала клиента, если он является сервером</remarks>
		public static AppType AppType = AppType.local;// по умолчанию тип = "local", без возможности запуска сервера

		#endregion

		/// <summary>
		/// Конструктор
		/// </summary>
		public Application()
		{
			AppType = GetArgsAppType();
			testArchiveOperations();

			// создание контроллера
			_controller = new Controller();

			// регистрируем метод для получения главного таймера (писалось для XNA, что бы можно было передать потом основной цикл ему)
			_controller.AddEventHandler("GetMainTimerRun", (o, args) => GetMainTimerRun(o, (GetHandlerEventArgs)args));
			_controller.AddEventHandler("StartServer", SetNetCreateServer);
			_controller.AddEventHandler("StartClient", SetNetCreateClient);

			// создание коллектора
			_collector = new Collector(_controller);

			#region заполнение массива typesForSearch классами и интерфейсами, которые надо искать в сборках

			var typesForSearch = new List<Type>();
			typesForSearch.Add(typeof(VisualizationProvider)); // визуализация
			typesForSearch.Add(typeof(IModelObject)); // объект модели
			typesForSearch.Add(typeof(ControllerEvent)); // Событие контроллера
			typesForSearch.Add(typeof(Module)); // Модуль
			typesForSearch.Add(typeof(Input)); // Устройства ввода
			typesForSearch.Add(typeof(ISound)); // Вывод звука

			#endregion

			// создаём таймер и настраиваем его на более менее оптимальную работу
			// но так как нужна работа инициализации визуализации,
			// которая убирает таймер, его лучше создать сразу
			_mainTimer = new Timer();
			_mainTimer.Interval = TimerInterval;
			//if (AppType != AppType.server){
			_mainTimer.Tick += MainTimerRun;//}else{_mainTimer.Tick += MainTimerRunServer;}

			// чтение из настроек сборок, которые надо сканировать
			var assemblies = new List<string>();
			foreach (var sr in Settings.EngineSettings.GetValues("assembly"))
			{
				assemblies.Add(sr.Hint);
			}
			// сканирование сборок);
			foreach (var assembly in assemblies)
			{
				_collector.FindObjectsInAssembly(assembly, typesForSearch);
			}
			// если ошибок не возникло то надо создавать визуализацию
			// хотя можно создать визуализацию через контрол
			//_controller.StartEvent("VisualizationStart", null, EventArgs.Empty);
			String visualizationName = Settings.EngineSettings.GetValue("Default", "Visualization");
			// для сервера запускаем отдельную специальную визуализацию
			if (AppType == AppType.server){
				visualizationName = Settings.EngineSettings.GetValue("Default", "ServerVisualization");
			}
			if (visualizationName == "") { throw new Exception(" не указана система визуализации в настройках"); }
			_visualizationProvider = (VisualizationProvider)_collector.Create(
				typeof(VisualizationProvider), visualizationName);
			if (_visualizationProvider == null) { throw new Exception("визуализатор не создан"); }
			// визуализация запускается в методе run
			_visualizationProvider.InitVisualization(_controller);

			// создание модели и вида (контроллер создаётся в начале, он особо не требователен)
			_model = new Model(_controller);
			_view = new View(_controller, _visualizationProvider);

			String inputName = Settings.EngineSettings.GetValue("Default", "Input");
			if (inputName == "") { inputName = "Engine.Input"; }// базовый класс, заготовка
			_input = (Input)_collector.Create(typeof(Input), inputName);
			if (_input == null)
			{
				_controller.SendError("Устройство ввода отсутствует " + inputName);
				_input = new Input();
			}
			_input.Init(_controller);

			// пока предполагаем что звук отдельно. но в общем случае звук может быть встроен в визуализацию
			String soundName = Settings.EngineSettings.GetValue("Default", "Sound");
			if (soundName == "") { soundName = "Engine.ISound"; }// базовый класс, заготовка
			var iSound = (ISound)_collector.Create(typeof(ISound), soundName);
			if (iSound == null)
			{
				_controller.SendError("Звуковое устройство отсутствует " + soundName);
				// и оставляем его null раз его всё равно нету
			}
			_sound = new Sound(iSound);
			_sound.Init(_controller);

			// локальные обработчики создаются в любом случае. потом уже в процессе запуска они заменяются
			// а вот как заменяются - пока не понятно SetNewObjects
			// но если предполагается запуск сервера, то создаётся серверный обработчик сразу. и при инициализации модуля он инициализирует только сервер
			_toModelDataSender = new DataSender(_controller, "SendToModel");// создаём отправителя сообщений Модели (фактически серверу)
			_toViewDataSender = new DataSender(_controller, "SendToView");// создаём отправителя сообщений Виду (фактически клиенту)
			// пока создаётся всё локально. проще собрать то что общее в функции и в зависимости от требуемого вызывать наборы функций
			//SetNetCreateServer(null, EventArgs.Empty);// 

			// запуск одного модуля из настроек
			String moduleName = Settings.EngineSettings.GetValue("Default", "Module");
			if (moduleName == "") { throw new Exception(" не указан запускаемый модуль в настройках"); }// модуль обязательно нужен
			var module = (Module)_collector.Create(typeof(Module), moduleName);
			if (module == null) { _controller.SendError("Запускаемый модуль не обнаружен в подключенных сборках " + moduleName); }
			else module.Init(_model, _view, _controller);// тоже зависит от типа приложения

			_controller.SendError("Создание объекта Application завершено");
			_controller.StartEvent("SystemStarted");// передаём все сообщения объектам
		}

		/// <summary>
		/// Получаем параметры командной строки и забираем из них тип сервера
		/// </summary>
		/// <returns></returns>
		private AppType GetArgsAppType()
		{
			var r = AppType.local;// по умолчанию всё запускается локально, на одном компьютере, используются внутрипрограммная передача данных
			var a = Environment.GetCommandLineArgs();
			foreach (var s in a)
			{
				var s1 = s.ToUpper();
				if (s1 == "CLIENT") r = AppType.client;
				if (s1 == "SERVER") r = AppType.server;
				if (s1 == "CLIENT-SERVER") r = AppType.client_server;// запускается клиент с сервером
			}
			return r;
		}

		/// <summary>
		/// тестовая функция для проверки создания архивов
		/// </summary>
		private void testArchiveOperations()
		{
			/*var arch = new FileArchieve("archive.arch");
			var ms1 = new MemoryStream(Encoding.UTF8.GetBytes("______________________________________________________________________"));
			arch.AddStream("ms1", ms1);
			var ms2 = new MemoryStream(Encoding.UTF8.GetBytes("bvbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbfffffffffffffffffdddddddddddddd"));
			arch.AddStream("ms2", ms2);
			arch.Flush();

			int n;
			var e = new Editor();
			e.AddNewLayer("XY","l1");
			e.SetCurrentLayer("l1");
			e.AddParam("store");
			n = e.AddNewObject();
			e.SetParam(n, "store", "значение 1");
			e.Save("a_editor1.arch");

			e = null;// очищаем, подготавливаем для сохранения
			arch = null;

			e = new Editor();
			e.AddNewLayer("XY", "l1");
			e.SetCurrentLayer("l1");
			e.AddParam("store");
			e.Load("a_editor1.arch");
			var u=e.GetParam(0, "store");
			Debug.WriteLine("=("+u+")=");
			*/
		}

		private void SetNetCreateServer(object sender, EventArgs args)
		{
			_toModelDataSender.Dispose();
			String s;
			s = Settings.EngineSettings.GetValue("Default", "port");
			int port = Convert.ToInt32(s);
			_toModelDataSender = new DataSenderServer(_controller, "SendToView", port);
		}

		private void SetNetCreateClient(object sender, EventArgs args)
		{
			_toViewDataSender.Dispose();
			String s;
			s = Settings.EngineSettings.GetValue("Default", "port");
			int port = Convert.ToInt32(s);
			var address = Settings.EngineSettings.GetValue("Default", "address");
			_toViewDataSender = new DataSenderClient(_controller, "SendToModel", address, port);
		}

		/// <summary>
		/// Вспомогательная функция. Получает метод основного цикла событий и отключает таймер
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void GetMainTimerRun(object sender, GetHandlerEventArgs args)
		{
			if (_mainTimer != null)
			{
				_mainTimer.Enabled = false; // отключаем таймер
			}
			// передаём главную функцию
			//if (AppType != AppType.server){
			args.Set(MainTimerRun); //}else{args.Set(MainTimerRunServer);}
		}

		/// <summary>
		/// Запуск Главного Таймера
		/// </summary>
		public void Run()
		{
			_mainTimer.Start();		// сначала запускаем таймер
			_visualizationProvider.Run();	// запускаем модальное диалоговое окно
			Settings.EngineSettings.Save("EngineSettings");		// после закрытия модального окна должен сработать этот оператор
			_sound.ClearLinks();    // прерываем все звуки
		}

		/// <summary>
		/// Значение времени для рассчёта количества кадров в секунду
		/// </summary>
		public static int TimerInterval = 1000 / 60;

		/// <summary>
		/// Основной цикл. по таймеру
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="eventArgs"></param>
		private void MainTimerRun(Object sender, EventArgs eventArgs)
		{
			// Неплохо бы определять сколько времени прошло для рассчета и рисования. 
			// и в зависимости от этого пропускать циклы рисования или пару лишних раз проводить рассчеты

			_controller.StartEvent("BeginLoop", null, EventArgs.Empty);
			_input.GetInput();// обработка устройств ввода

			_model.Execute();

			_view.Draw();

			_controller.StartEvent("EndLoop", null, EventArgs.Empty);
		}


	}
}
