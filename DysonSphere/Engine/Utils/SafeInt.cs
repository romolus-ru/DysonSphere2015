using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils
{
	/// <summary>
	/// Класс для работы с целыми числами со скрытием значения переменной
	/// </summary>
	/// <remarks>Взято отсюда http://habrahabr.ru/post/266345/ со значительными переделками</remarks>
	public struct SafeInt
	{
		private int _offset;
		private int _value;
		private static Random _rnd;

		public SafeInt(Random rnd, int value = 0)
		{
			_rnd = rnd;
			_offset = _rnd.Next(-1000, +1000);
			this._value = value + _offset;
		}

		public int GetValue()
		{
			return _value - _offset;
		}

		public void Dispose()
		{
			_offset = 0;
			_value = 0;
		}

		public override string ToString()
		{
			return GetValue().ToString();
		}

		public static SafeInt operator +(SafeInt f1, SafeInt f2)
		{
			return new SafeInt(_rnd, f1.GetValue() + f2.GetValue());
		}

		// ...похожим образом перегружаем остальные операторы
	}
}