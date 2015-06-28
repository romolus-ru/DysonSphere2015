using System;
using System.Windows.Forms;
using System.Drawing;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;

namespace Engine.Views.Templates
{
	public class Button : ViewControl
	{
		/// <summary>
		/// Имя запускаемого события
		/// </summary>
		private string _eventName;

		/// <summary>
		/// Заголовок кнопки
		/// </summary>
		protected string Caption;

		/// <summary>
		/// Заголовок кнопки
		/// </summary>
		protected string Hint;

		/// <summary>
		/// Кнопка для кнопки
		/// </summary>
		protected Keys Key;

		protected StateOneTime StateOneKeyboard = StateOneTime.Init(15);// для кнопки клавиатуры
		protected StateOne StateOneMouse = new StateOne();// для кнопки мыши

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		public Button(Controller controller) : base(controller) { }

		/// <summary>
		/// Жмём на кнопку
		/// </summary>
		public virtual void Press()
		{
			if (Controller != null){
				Controller.AddToOperativeStore(_eventName, this, EventArgs.Empty);
			}
		}

		protected override void Keyboard(object sender, InputEventArgs e)
		{
			if (!CanDraw) return;
			// (если нажата кнопка мыши и мышка находится над кнопкой) или (если нажата кнопка на клавиатуре)
			bool b = e.IsKeyPressed(Keys.LButton);
			bool b2 = e.IsKeyPressed(Key);
			var clickOnButton = b && CursorOver;
			var sk = StateOneKeyboard.Check(clickOnButton || b2);
			if (sk == StatesEnum.On){
				Press();
			}
			//if (StateOneKeyboard.CurrentState == StatesEnum.On) e.Handled = true;
			if (clickOnButton) e.Handled = true;
			base.Keyboard(sender, e);
		}

		public void Init(string eventName, String caption, String hint, Keys key)
		{
			_eventName = eventName;
			Caption = caption;
			Hint = hint;
			Key = key;
			Name = caption;
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.RotateReset();
			String txt;
			Color color;
			if (CursorOver){
				txt = "[" + Caption + "]"; color = Color.Red;
			}else{
				txt = " " + Caption + " "; color = Color.White;
			}
			var f = visualizationProvider.FontHeightGet() / 2;

			visualizationProvider.SetColor(color);
			visualizationProvider.Rectangle(X, Y, Width, Height);// если текстуры будут то они замаскируют этот прямоугольник

			//DrawComponentBackground(visualizationProvider);// это вызывается другими методами

			visualizationProvider.SetColor(color);
			visualizationProvider.Print(X + 4, Y + Height / 2 - f - 3, txt);
			if (Hint != "" && CursorOver)
			{
				visualizationProvider.Print(X + 10, Y + Height + 5 - f, Hint);
			}
		}

		/// <summary>
		/// Инициализируем кнопку, с учётом названия
		/// </summary>
		public static Button CreateButton(Controller controller, int x, int y, int width, int height, string eventName, String caption, String hint, Keys key, String buttonName)
		{
			var btn = new Button(controller);
			InitButton(btn, controller, x, y, width, height, eventName, caption, hint, key, buttonName);
			return btn;
		}

		public void SetCaption(String newCaption)
		{
			Caption = newCaption;
		}

		/// <summary>
		/// Инициализируем готовую кнопку
		/// </summary>
		/// <param name="btn"></param>
		/// <param name="controller"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="eventName"></param>
		/// <param name="caption"></param>
		/// <param name="hint"></param>
		/// <param name="key"></param>
		/// <param name="buttonName"></param>
		public static Button InitButton(Button btn, Controller controller, int x, int y, int width, int height, string eventName, String caption, String hint, System.Windows.Forms.Keys key, String buttonName)
		{
			btn.Show();
			var dx = 0;//rnd.Next(0, 20);
			var dy = 0;//rnd.Next(0, 20);
			btn.SetCoordinates(x + dx, y + dy, 0);
			btn.SetSize(width, height);
			btn.Init(eventName, caption, hint, key); 
			btn.Name = buttonName;
			return btn;
		}

	}
}
