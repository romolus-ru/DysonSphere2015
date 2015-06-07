using System;
using System.Collections.Generic;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	/// <summary>
	/// Элемент управления. может содержать другие компоненты и передавать им клавиатуру и мышь
	/// </summary>
	public class ViewControl:IViewObject
	{
		#region Основные переменные

		public Controller Controller { get; protected set; }
		/// <summary>
		/// Возможно, устарело. имя объекта может пригодится при отладке
		/// </summary>
		public String Name { get; protected set; }
		protected VisualizationProvider VisualizationProvider;

		/// <summary>
		/// Координата X объекта
		/// </summary>
		public int X { get; protected set; }

		/// <summary>
		/// Координата Y объекта
		/// </summary>
		public int Y { get; protected set; }

		/// <summary>
		/// Координата Z объекта
		/// </summary>
		public int Z { get; protected set; }
		
		public bool CanDraw { get; private set; }
		protected Boolean IsModal;
		private Boolean _isModalStoreCanDraw;
		/// <summary>
		/// Флаг, находится ли курсор над компонентом
		/// </summary>
		public Boolean CursorOver { get; set; }

		/// <summary>
		/// вложенные компоненты
		/// </summary>
		protected List<ViewControl> Controls = new List<ViewControl>();

		/// <summary>
		/// Покинул ли курсор пределы объекта (что бы лишний раз не сбрасывать состояние)
		/// </summary>
		protected Boolean CursorOverOffed;

		#endregion

		#region Размеры объекта

		/// <summary>
		/// Высота объекта
		/// </summary>
		public int Height { get; protected set; }

		/// <summary>
		/// Ширина объекта
		/// </summary>
		public int Width { get; protected set; }

		/// <summary>
		/// Установить размеры объекта
		/// </summary>
		/// <param name="width">Ширина</param>
		/// <param name="height">Высота</param>
		public void SetSize(int width, int height)
		{
			Height = height;
			Width = width;
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Для блокировки дополнительных вызовов dispose
		/// </summary>
		private Boolean _disposed = !true;

		public virtual void Dispose()
		{
			//base.Dispose();
			if (!_disposed)
			{// обработчики основного класса удаляются в viewObject
				foreach (var control in Controls) { control.Dispose(); }
				Controls.Clear();
				Controls = null;
				_disposed = true;
				X = -99000;
				Y = -99000;
				Width = 0;
				Height = 0;
			}
		}

		/// <summary>
		/// Деструктор
		/// </summary>
		~ViewControl()
		{
			Dispose();
		}

		#endregion

		public ViewControl(Controller controller)
		{
			Controller = controller;
		}

		/// <summary>
		/// Инициализация объекта для текущей визуализации (размер экрана и т.п.)
		/// </summary>
		/// <param name="visualizationProvider"></param>
		public void Init(VisualizationProvider visualizationProvider)
		{
			VisualizationProvider = visualizationProvider;// сохраняем для будущего использования
			HandlersAdd();
			InitObject(VisualizationProvider);
		}

		/// <summary>
		/// переопределяемая инициализация объекта для текущей визуализации
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void InitObject(VisualizationProvider visualizationProvider){}

		/// <summary>
		/// Добавляем дополнительные обработчики, нужные для контрола
		/// </summary>
		protected virtual void HandlersAdd() { }

		/// <summary>
		/// Удаляем дополнительные обработчики
		/// </summary>
		protected virtual void HandlersRemove() { }

		/// <summary>
		/// В данном случае надо показать и компоненты
		/// </summary>
		public void Show()
		{
			CanDraw = true;
			foreach (var control in Controls) { control.Show(); }
		}



		/// <summary>
		/// В данном случае надо скрыть и компоненты
		/// </summary>
		public void Hide()
		{
			foreach (var control in Controls) { control.Hide(); }
			CanDraw = false;
		}

		/// <summary>
		/// Добавить объект к списку компонентов
		/// </summary>
		/// <param name="control"></param>
		public void AddControl(ViewControl control)
		{
			Controls.Add(control);
			control.Show();
			control.Init(VisualizationProvider);
		}

		/// <summary>
		/// Удалить объект
		/// </summary>
		/// <param name="control"></param>
		public void RemoveControl(ViewControl control)
		{
			control.HandlersRemove();
			control.Hide();
			Controls.Remove(control);
		}

		/// <summary>
		/// Переместить объект на передний план
		/// </summary>
		/// <param name="topObject"></param>
		public void BringToFront(ViewControl topObject)
		{
			if (Controls.Contains(topObject)){
				Controls.Remove(topObject);
				Controls.Insert(0, topObject);
			}else{
				foreach (var control in Controls){
					control.BringToFront(topObject); // каждый компонент который может содержать другие объекты 
					// это можно улучшить, храня у объекта ссылку на предка
					// но ссылка на предка может быть использована в корыстных целях
					// можно ввести вспомогательную переменную, которая после выполнения операции просто завершит вызов всех рекурсивных функций
					// можно вместо list сделать dictionary что повысит скорость операции
					// но будем надеяться что жта функция будет вызываться редко поэтому ничего делать не нужно
					// но как только появится parent - надо переписать, с парентом это проще делается - у родителя вызываем бринг 
					// и всё. а если у него нету этого объекта значит у объекта неправильный парент прописан, делов то
				}
			}
		}

		/// <summary>
		/// Переместить объект на задний план
		/// </summary>
		/// <param name="topObject"></param>
		public void SendToBack(ViewControl topObject)
		{
			if (Controls.Contains(topObject)){
				Controls.Remove(topObject);
				Controls.Add(topObject);
			}else{
				foreach (var control in Controls){
					control.SendToBack(topObject); // каждый компонент который может содержать другие объекты 
				}
			}
		}

		/// <summary>
		/// Переместить объект на передний план
		/// </summary>
		public void BringToFront()
		{
			Controller.StartEvent("ViewBringToFront", this, EventArgs.Empty);
		}

		public void SendToBack()
		{
			Controller.StartEvent("ViewSendToBack", this, EventArgs.Empty);
		}

		#region Cursor

		/// <summary>
		/// Обработка события курсора
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		/// <remarks>Для перемещаемого контрола это придётся переопределить. + он сам должен уметь определять момент начала перемещения</remarks>
		public virtual void CursorEH(object o, PointEventArgs args)
		{
			if (!CanDraw) return;
			CursorDeliver(o, args);
		}

		/// <summary>
		/// Стандартное распределение обработки события курсора
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		/// <remarks>Для перемещаемого контрола это придётся переопределить. + он сам должен уметь определять момент начала перемещения</remarks>
		protected void CursorDeliver(object o, PointEventArgs args)
		{
			//if (args == null) return;// в общем классе, надо сделать эту проверку (View) (а возможно вообще не нужна
			if (!InRange(args.Pt.X, args.Pt.Y)){
				CursorOverOff(); // сбрасываем выделение, в том числе и у вложенных контролов
				return;
			}
			CursorOver = true;
			Cursor(o, args);
			if (Controls != null){
				CursorOverOffed = false;
				foreach (var control in Controls){
					control.CursorOver = false;
					if (!control.InRange(args.Pt.X - X, args.Pt.Y - Y)) continue; // компонент не в точке нажатия
					//control.CursorOver = true;// control.CursorEH сам установит флаг CursorOver
					var b = PointEventArgs.Set(args.Pt.X - X, args.Pt.Y - Y);
						// смещаем курсор и передаём контролу смещенные координаты
					control.CursorEH(o, b);
				}
			}
		}

		/// <summary>
		/// Переопределяемая обработка события курсора
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		protected virtual void Cursor(object o, PointEventArgs args) { }

		#endregion

		#region Keyboard

		/// <summary>
		/// Распределение обработки события клавиатуры
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		/// <remarks>Для перемещаемого компонента придётся переопределить</remarks>
		public virtual void KeyboardEH(object o, InputEventArgs args)
		{
			if (!CanDraw) return;
			KeyboardDeliver(o, args);
		}

		/// <summary>
		/// Стандартное распределение обработки события клавиатуры
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		/// <remarks>Для перемещаемого компонента придётся переопределить</remarks>
		protected void KeyboardDeliver(object o, InputEventArgs args)
		{
			//if (args == null)return;
			Keyboard(o, args);
			if (Controls != null){
				var controlsLocal = Controls.ToArray(); // что бы небыло ошибки что список компонентов изменен
				foreach (var control in controlsLocal){
					if (args.Handled) break; // если событие было обработано - выходим
					// все объекты получат событие клавиатуры, даже если курсор не над ними
					// компонент сам должен решить реагировать или нет
					//if (!component.CursorOver)continue;// курсор не над объектом
					if (!control.CanDraw) continue; // компонент скрыт
					control.KeyboardEH(o, args);
				}
			}
		}

		/// <summary>
		/// Переопределяемая обработка события клавиатуры
		/// </summary>
		/// <param name="o"></param>
		/// <param name="args"></param>
		protected virtual void Keyboard(object o, InputEventArgs args) { }

		#endregion

		#region Draw

		/// <summary>
		/// Прорисовка объекта
		/// </summary>
		/// <param name="visualizationProvider">Объект-визуализатор</param>
		public void Draw(VisualizationProvider visualizationProvider)
		{
			if (CanDraw){
				DrawComponentBackground(visualizationProvider);
				DrawObject(visualizationProvider);
				DrawComponents(visualizationProvider);
			}
		}

		/// <summary>
		/// Прорисовка объекта переопределяемая
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawObject(VisualizationProvider visualizationProvider) { }

		/// <summary>
		/// Прорисовать фон компонентов, если нужно
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			//if (CursorOver)	visualizationProvider.SetColor(Color.DodgerBlue, 20);
			//else			visualizationProvider.SetColor(Color.DimGray, 50);
			//visualizationProvider.Box(X, Y, Width, Height);
		}

		/// <summary>
		/// Перерисовать подчиненные компоненты
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawComponents(VisualizationProvider visualizationProvider)
		{
			// можно проверять на предмет правильного восстановления смещения
			visualizationProvider.OffsetAdd(X, Y);// смещаем и рисуем компоненты независимо от их настроек
			// прорисовываем в обратном порядке, от нижких к верхним - наверху находятся те объекты, которые рисуются последними
			for (int index = Controls.Count - 1; index >= 0; index--){
				var control = Controls[index];
				control.Draw(visualizationProvider);
			}
			visualizationProvider.OffsetRemove();// восстанавливаем смещение			
		}

		/// <summary>
		/// Прорисовка объекта для текстуры
		/// </summary>
		/// <param name="visualizationProvider">Объект-визуализатор</param>
		public void DrawToTexture(VisualizationProvider visualizationProvider)
		{
			if (CanDraw)
			{
				DrawObjectToTexture(visualizationProvider);
			}
		}

		/// <summary>
		/// Прорисовка объекта для текстуры. Без проверки на необходимость вывода на экран
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected virtual void DrawObjectToTexture(VisualizationProvider visualizationProvider)
		{

		}

		#endregion

		/// <summary>
		/// Сбрасываем CursorOver, в том числе и у всех вложенных компонентов
		/// </summary>
		protected void CursorOverOff()
		{
			if (CursorOverOffed) return;
			CursorOverOffed = true;
			CursorOver = false;
			foreach (var controls in Controls)
			{
				controls.CursorOverOff();
			}
		}

		/// <summary>
		/// Проверяем, находятся ли переданные координаты внутри объекта
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>Находится координата в пределах области контрола или нет</returns>
		public virtual Boolean InRange(int x, int y)
		{
			if (!CanDraw) return false; // компонент не рисуется - значит не проверяем дальше
			if ((X < x) && (x < X + Width)){
				if ((Y < y) && (y < Y + Height)){
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Установить координаты объекта
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="z"></param>
		public void SetCoordinates(int x, int y, int z = 0)
		{
			X = x;
			Y = y;
			Z = z;
		}

		/// <summary>
		/// Установить относительные координаты объекта
		/// </summary>
		/// <param name="rx"></param>
		/// <param name="ry"></param>
		/// <param name="rz"></param>
		public void SetCoordinatesRelative(int rx, int ry, int rz)
		{
			X += rx;
			Y += ry;
			Z += rz;
		}

		public void SetName(string name)
		{
			Name = name;
		}

		public void ModalStart()
		{
			IsModal = true;
			_isModalStoreCanDraw = CanDraw;// сохраняем состояние, при выводе модального объекта извне оно не обрабатывается, но сильно меняется
			Controller.StartEvent("ViewSystem.ModalStart", ViewControlEventArgs.Send(this));
		}

		public void ModalStop()
		{
			IsModal = false;
			CanDraw = _isModalStoreCanDraw;// восстанавливаем старое состояние, которое было до модального вызова
			Controller.StartEvent("ViewSystem.ModalStop", ViewControlEventArgs.Send(this));
		}

	}
}
