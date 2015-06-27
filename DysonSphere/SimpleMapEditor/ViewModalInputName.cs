using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;
using Engine.Views;
using Button = Engine.Views.Templates.Button;

namespace SimpleMapEditor
{
	/// <summary>
	/// Для ввода строки
	/// </summary>
	/// <remarks>
	/// В выходном событии sender это объект ViewModalInput, у которого нужно подучить результат
	/// в событии удаления нужно удалить объект из системы и вызвать метод dispose (хотя он должен вызваться сам при удалении всех связей объекта)
	/// </remarks>
	class ViewModalInputName : ViewModalInput
	{
		private StateOneTime _keyTime = StateOneTime.Init(5);
		private int _editCursor = 0;

		public ViewModalInputName(Controller controller, string outEvent, string value)
			: base(controller, outEvent, value)
		{
			_editCursor = _text.Length;
			_keyTime.Check(true);// блокируем клики что бы сразу окно не закрылось

		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			EscButton = Button.CreateButton(Controller, -280, -10, -100, -20, OutEvent, "Закрыть", "Закрыть", Keys.None, "");
			AddControl(EscButton);
			BringToFront();
		}

		/// <summary>
		/// Кнопка закрытия модального окна
		/// </summary>
		protected Button EscButton;

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			if (CursorOver)
				visualizationProvider.SetColor(Color.FromArgb(50, Color.Chartreuse));
			else visualizationProvider.SetColor(Color.FromArgb(50, Color.Aquamarine));
			var cur1 = ":> ";
			visualizationProvider.Box(X, Y, Width, Height);
			base.DrawObject(visualizationProvider);
			visualizationProvider.SetColor(Color.Aquamarine);
			visualizationProvider.Print(X + 10, Y + 20, "Жмите any key. или Enter для завершения ввода");
			visualizationProvider.Print(X + 10, Y + 30, cur1 + _text);
			var s = cur1 + _text.Substring(0, _editCursor);
			var l = visualizationProvider.TextLength(s);
			visualizationProvider.SetColor(Color.Azure);
			visualizationProvider.Print(X + 10 + l, Y + 30, "_");
			visualizationProvider.Print(X + 10, Y + 40, "" + _editCursor);
		}

		protected override void Keyboard(object o, InputEventArgs inputEventArgs)
		{
			base.Keyboard(o, inputEventArgs);
			var st = _keyTime.Check(true);
			if (st != StatesEnum.On) return;
			// обрабатываем управляющие кнопки
			if (inputEventArgs.IsKeyPressed(Keys.LButton)){
				if (!CursorOver){
					EscButton.Press();
					return;
				}
			}
			if (inputEventArgs.IsKeyPressed(Keys.Back)){
				if (_editCursor > 0){
					_editCursor--;
					_text = _text.Remove(_editCursor, 1);
				}
			}
			if (inputEventArgs.IsKeyPressed(Keys.Escape)){
				if (_text == _value){
					EscButton.Press();
					return;
				}
				_text = _value;
				_editCursor = _text.Length;
				return;
			}
			if (inputEventArgs.IsKeyPressed(Keys.Enter)){
				EscButton.Press();
				return;
			}
			if (inputEventArgs.IsKeyPressed(Keys.Left)){
				if (_editCursor > 0) _editCursor--;
			}
			if (inputEventArgs.IsKeyPressed(Keys.Right)){
				if (_editCursor < _text.Length) _editCursor++;
			}
			if (inputEventArgs.IsKeyPressed(Keys.Home)) _editCursor = 0;
			if (inputEventArgs.IsKeyPressed(Keys.End)) _editCursor = _text.Length;

			var a = inputEventArgs.KeyToUnicode();
			if (a.Length > 0){
				_text = _text.Insert(_editCursor, a);
				_editCursor += a.Length;
			}
		}
	}
}
