using System;
using System.Drawing;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;

namespace Engine.Views.Templates
{
	/// <summary>
	/// перемещаемый ползунок с возможностью выставить значение
	/// </summary>
	public class TrackBar:ViewControl
	{
		private int _currentValue;
		private int _currentValueOld;
		private int _maxValue;
		private int _minValue;
		private int _step1;
		public Boolean IsVertical;
		private DragObject _slider;
		public delegate void OnValueChanged(int newValue);
		public event OnValueChanged CurrValueChanged;
		private StateOneTime _stateLButton = StateOneTime.Init(10);

		public TrackBar(Controller controller) : base(controller)
		{
			_currentValue = 50;
			_currentValueOld = _currentValue;
			_maxValue = 100;
			_minValue = 0;
			IsVertical = false;
			RecalcStep();
			_slider = new DragObject(Controller);
			_slider.NewPos += SetValueFromSlider;

			AddControl(_slider);
		}

		public void SendNewCurrentValue()
		{
			if (_currentValue != _currentValueOld){
				_currentValueOld = _currentValue;
				if (CurrValueChanged!=null)
					CurrValueChanged(_currentValue);
			}
		}

		private int slx = 0;
		/// <summary>
		/// От слайдера пришло сообщение об изменении положения
		/// </summary>
		private void SetValueFromSlider()
		{
			float x = 100f*_slider.X/(Width-_slider.Width);// переводим в другую шкалу, не учитывая полную длину
			slx = (int)x;
			// определяем значение _currentValue
			var i2 = _maxValue - _minValue;
			_currentValue = _minValue + (int) (x*i2/100);
			SendNewCurrentValue();
			RecalcSliderPos();
			//var w = _slider.Width / 2;
			//var x = w + ((Width - w * 2) * i1 / i2);
			//return x;
		}

		public void SetValues(int min, int max)
		{
			_currentValue = 0;
			SendNewCurrentValue();
			_minValue = min;
			_maxValue = max;
		}
		
		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			if (IsVertical) _slider.CorrectSize(Width);
			else _slider.CorrectSize(Height);
			RecalcSliderPos();
		}

		private void RecalcSliderPos()
		{
			if (IsVertical){
				_slider.SetCoordinates(0, GetX() - _slider.Height/2);
			}else{
				_slider.SetCoordinates(GetX() - _slider.Width/2, 0);
			}
			_slider.Correct();
		}

		/// <summary>
		/// вычислить шаг в соответствии с текущими настройками
		/// </summary>
		private void RecalcStep()
		{
			var a = (_maxValue - _minValue)/10;
			_step1 = 1;
			if (a > 1) _step1 = a;
		}

		private int _cx = 0;
		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			int x = GetX();
			int h = Height/2;
			visualizationProvider.SetColor(Color.Wheat);
			visualizationProvider.Print(X + Width/2, Y - 20, " c=" + _currentValue + " x=" + x + " cx=" + _cx + " sx=" + slx);
			visualizationProvider.SetColor(Color.White);
			if (CursorOver) visualizationProvider.SetColor(Color.Chartreuse);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			visualizationProvider.Circle(X + x, Y + h, h/3);
		}

		private int GetX()
		{
			var i1 = _currentValue - _minValue;
			var i2 = _maxValue - _minValue;
			var w = _slider.Width / 2;
			var x = w + ((Width - w * 2) * i1 / i2);
			return x;
		}

		private void CorrectValue()
		{
			if (_currentValue < _minValue) _currentValue = _minValue;
			if (_currentValue > _maxValue) _currentValue = _maxValue;
			SendNewCurrentValue();
		}

		protected override void Keyboard(object o, InputEventArgs args)
		{
			base.Keyboard(o, args);
			if (args.IsKeyPressed(Keys.Left)) {_currentValue -= _step1;RecalcSliderPos();}
			if (args.IsKeyPressed(Keys.Right)) {_currentValue += _step1;RecalcSliderPos();}
			var a = args.IsKeyPressed(Keys.LButton);
			if (CursorOver&&a) args.Handled = true;// если кнопка нажата - отмечаем что событие обработано
			var sLButton = _stateLButton.Check(a);
			if (CursorOver&&sLButton==StatesEnum.On){// определяем с какой стороны кликнули и перемещаем объект
				var x = GetX();
				_cx = args.CursorX - X;
				if (_cx < x) _currentValue -= _step1;
				if (_cx > x) _currentValue += _step1;
				RecalcSliderPos();
			}
			CorrectValue();
		}

	}
}
