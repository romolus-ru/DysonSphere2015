using System;
using System.Windows.Forms;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// Для передачи объекта Input (устройства ввода)
	/// </summary>
	public class InputEventArgs : EngineEventArgs
	{
		private Input Input { get; set; }

		public bool IsKeyPressed(Keys key)
		{
			if (Input.KeyboardCleared) return false;
			return Input.IsKeyPressed(key);
		}

		/// <summary>
		/// Очистить клавиатуру от событий. Аналог есть у контроллера
		/// </summary>
		public void KeyboardClear()
		{
			Input.KeyboardClear(this, Empty);
		}

		public string KeyToUnicode()
		{
			return Input.KeysToUnicode();
		}

		#region Координаты курсора. Экранные. Возможно, неправильное использование - Координаты берутся из другого источника

		/// <summary>
		/// Координата курсора X
		/// </summary>
		public int CursorX { get { return Input.CursorX+CursorCorrectionX; } }

		/// <summary>
		/// Координата курсора Y
		/// </summary>
		public int CursorY { get { return Input.CursorY+CursorCorrectionY; } }
		
		public int CursorCorrectionX { get; protected set; }
		public int CursorCorrectionY { get; protected set; }
		/// <summary>
		/// Изменить корректировку курсора
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void AddCorrectionCursor(int x, int y)
		{
			CursorCorrectionX += x;
			CursorCorrectionY += y;
		}

		/// <summary>
		/// Координата курсора Y
		/// </summary>
		public int CursorDelta { get { return Input.CursorDelta; } }

		#endregion

		/// <summary>
		/// Устройство ввода
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		static public InputEventArgs SetInput(Input input)
		{
			var i = new InputEventArgs();
			i.Input = input;
			return i;
		}
	}
}
