using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;

namespace Engine.Views.Templates
{
	/// <summary>
	/// Вертикальный скроллбар
	/// </summary>
	/// <remarks>На основе TrackBar. в принципе подготовлен работать как вертикальный так и как горизонтальный</remarks>
	public class ScrollBar:ViewControl
	{
		private int _currentValue;
		private int _currentValueOld;
		private int _maxValue;
		private int _minValue;
		private int _step1;
		public Boolean IsVertical;
		private DragObject _slider;
		public delegate void OnValueChanged(int newValue);
		public event TrackBar.OnValueChanged CurrValueChanged;
		private StateOneTime _stateLButton = StateOneTime.Init(10);

		public ScrollBar(Controller controller)
			: base(controller)
		{
			_currentValue = 50;
			_currentValueOld = _currentValue;
			_maxValue = 100;
			_minValue = 0;
			IsVertical = true;
			RecalcStep();
			_slider = new DragObject(Controller);
			_slider.NewPos += SetValueFromSlider;
			_slider.IsVertical = IsVertical;

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

		private int _slx = 0;
		/// <summary>
		/// От слайдера пришло сообщение об изменении положения
		/// </summary>
		private void SetValueFromSlider()
		{
			float x = 100f*_slider.X/(Width-_slider.Width);// переводим в другую шкалу, не учитывая полную длину
			_slx = (int)x;
			// определяем значение _currentValue
			var i2 = _maxValue - _minValue;
			_currentValue = _minValue + (int) (x*i2/100);
			SendNewCurrentValue();
			RecalcSliderPos();
		}

		public void SetValues(int min, int max)
		{
			_currentValue = 0;
			SendNewCurrentValue();
			_minValue = min;
			_maxValue = max;
			RecalcStep();
			//RecalcSliderPos();
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
				_slider.SetCoordinates(0, GetCurrentPos() - _slider.Height/2);
			}else{
				_slider.SetCoordinates(GetCurrentPos() - _slider.Width/2, 0);
			}
			_slider.Correct();
		}

		/// <summary>
		/// вычислить шаг в соответствии с текущими настройками
		/// </summary>
		private void RecalcStep()
		{
			var a = (_maxValue - _minValue)/10;
			_step1 = (a > 1) ? a : 1;
		}

		private int _cpos = 0;

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			int cp = GetCurrentPos();
			int x,y,r;
			if (IsVertical){
				x = Width / 2;y = cp;r = x / 3;
			}else{
				x = cp;y = Height / 2;r = y / 3;
			}
			
			visualizationProvider.SetColor(Color.Wheat);
			visualizationProvider.Print(X + Width/2, Y - 20, " c=" + _currentValue + " x=" + x + " cx=" + _cpos + " sx=" + _slx);
			visualizationProvider.SetColor(Color.White);
			if (CursorOver) visualizationProvider.SetColor(Color.Chartreuse);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			visualizationProvider.Circle(X + x, Y + y, r);
		}

		private int GetCurrentPos()
		{
			var i1 = _currentValue - _minValue;
			var i2 = _maxValue - _minValue;
			var w = _slider.Width / 2;// оно квадратное
			var wh = (IsVertical) ? Height : Width;
			int x = w;
			if (i2!=0) x = w + ((wh - w * 2) * i1 / i2);
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
			//if (args.IsKeyPressed(Keys.Left)) {_currentValue -= _step1;RecalcSliderPos();}
			//if (args.IsKeyPressed(Keys.Right)) {_currentValue += _step1;RecalcSliderPos();}
			var a = args.IsKeyPressed(Keys.LButton);
			if (CursorOver&&a) args.Handled = true;// если кнопка нажата - отмечаем что событие обработано
			var sLButton = _stateLButton.Check(a);
			if (CursorOver&&sLButton==StatesEnum.On){// определяем с какой стороны кликнули и перемещаем объект
				var cp = GetCurrentPos();
				if (IsVertical)_cpos = args.CursorY - Y;
				else _cpos = args.CursorX - X;
				if (_cpos < cp) _currentValue -= _step1;
				if (_cpos > cp) _currentValue += _step1;
				RecalcSliderPos();
			}
			CorrectValue();
		}

	}
}
