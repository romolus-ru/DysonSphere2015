﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;

namespace Engine.Utils.Editor
{
	/// <summary>
	/// Класс - основа для всех редакторов
	/// </summary>
	/// <remarks>
	/// Для нормальной работоспособности нужно переопределить записываемые данные, сохраняемые данные в слое и т.п.
	/// </remarks>
	public class Editor : ViewControl
	{
		/// <summary>
		/// Список слоёв. для централизованного сохранения
		/// </summary>
		private List<ILayer<IDataHolder>> _layers = new List<ILayer<IDataHolder>>();

		/// <summary>
		/// Спрятанное поле. все функции работают с текущим слоем, чаще всего
		/// </summary>
		private ILayer<IDataHolder> _currentLayer = null;

		public Editor(Controller controller) : base(controller) { }

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			X = 0;
			Y = 0;
			Width = 800;
			Height = 600;
		}

		private Boolean LayerExists(String layerName) { return (GetLayer(layerName) != null); }

		private ILayer<IDataHolder> GetLayer(String layerName) { return _layers.Find(l => l.LayerName == layerName); }

		public void AddNewLayer(ILayer<IDataHolder> layer)
		{
			//одинаковые имена в любом случае противопоказаны
			if (LayerExists(layer.LayerName)) return;// если такой слой уже создан то выходим
			_layers.Add(layer);
		}

		public void SetCurrentLayer(String layerName)
		{
			_currentLayer = GetLayer(layerName);
		}

		/// <summary>
		/// Создать объект в текущем слое
		/// </summary>
		/// <returns></returns>
		public int AddNewObject(String objectType)
		{
			return _currentLayer.AddObject(objectType);
		}

		/// <summary>
		/// Создать объект в текущем слое
		/// </summary>
		/// <returns></returns>
		public int AddNewObject(IDataHolder obj)
		{
			return _currentLayer.AddObject(obj);
		}

		/// <summary>
		/// Получить объект по индексу
		/// </summary>
		/// <returns></returns>
		public IDataHolder GetObject(int num)
		{
			return _currentLayer.GetObject(num);
		}

		/// <summary>
		/// Сохранить слои в архив
		/// </summary>
		/// <param name="fileName"></param>
		public void Save(string fileName)
		{
			var a = new FileArchieve(fileName);
			foreach (var layer in _layers){
				if (!layer.CanStore) { continue; }
				var ms = layer.Save();
				a.AddStream(layer.LayerName, ms);
			}
			a.Dispose();
		}

		/// <summary>
		/// Загрузить слои из архива
		/// </summary>
		/// <param name="fileName"></param>
		public void Load(string fileName)
		{
			var a = new FileArchieve(fileName, false);
			foreach (var fl in a.Files){
				string layerName = fl.FullName;
				if (!LayerExists(layerName)) { continue; }// все нужные слои должны быть созданы заранее
				var layer = GetLayer(layerName);// получаем на него ссылку, что бы загрузить данные
				var ms = a.GetStream(layerName);
				layer.Load(ms);
			}
			a.Dispose();
		}

		/// <summary>
		/// Список объектов у текущего слоя (возвращается интерфейс IDataObject)
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IDataHolder> Objects()
		{
			return null;
		}

		/// <summary>
		/// Переопределяем - фон не должен реагировать на мышу и даже присутствовать не должен
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.Brown);
			visualizationProvider.Rectangle(X, Y, Width, Height);
		}

		/// <summary>
		/// Установить активный слой, остальные скрыть
		/// </summary>
		/// <param name="layerName"></param>
		/// <remarks>Было на ActiveHandlers но решил убрать это, всё равно основа это видимость слоя. можно будет другой флаг ввести - обработчики тут тоже свои пока</remarks>
		public void SetActiveLayer(string layerName)
		{
			foreach (var layer in _layers){
				var l = layer as ViewControl;
				l.Hide();
				if (layer.LayerName == layerName){
					l.Show();
				}
			}
			SetCurrentLayer(layerName);
		}

		protected override void Keyboard(object o, InputEventArgs args)
		{
			// обработчики оставил. кнопки и курсор получит только тот слой, который видимый
			base.Keyboard(o, args);
			foreach (var layer in _layers){
				var l = layer as ViewDraggable;
				if (l != null){
					if (!l.CanDraw) { continue; }
					l.KeyboardEH(o, args);
				}
			}
		}

		protected override void Cursor(object o, PointEventArgs args)
		{
			base.Cursor(o, args);
			foreach (var layer in _layers){
				var l = layer as ViewDraggable;
				if (l != null){
					if (!l.CanDraw){continue;}
					l.CursorEH(o, args);
				}
			}
		}
	}
}
