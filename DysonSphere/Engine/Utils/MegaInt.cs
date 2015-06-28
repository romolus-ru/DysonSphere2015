using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils
{
	/// <summary>
	/// Класс для работы с особо большими числами
	/// </summary>
	public class MegaInt
	{
		/// <summary>
		/// Названия чисел по степеням
		/// </summary>
		private static Dictionary<int, string> _names = InitNames();

		/// <summary>
		/// Значение числа
		/// </summary>
		private Dictionary<int, int> _values = new Dictionary<int, int>();

		public MegaInt(){InitNames();}
		
		/// <summary>
		/// Заполнение имен чисел
		/// </summary>
		private static Dictionary<int, string> InitNames()
		{
			Dictionary<int, string> names=new Dictionary<int, string>();
			names.Add(0, "");
			names.Add(3, "тысяч");
			names.Add(6, "миллион");
			names.Add(9, "биллион");
			names.Add(12, "триллион");
			names.Add(15, "квадриллион");
			names.Add(18, "квинтиллион");
			names.Add(21, "секстиллион");
			names.Add(24, "септиллион");
			names.Add(27, "октиллион");
			names.Add(30, "нониллион");
			names.Add(33, "дециллион");
			names.Add(36, "андециллион");
			names.Add(39, "дуодециллион");
			names.Add(42, "тредециллион");
			names.Add(45, "кваттордециллион");
			names.Add(48, "квиндециллион");
			names.Add(51, "сексдециллион");
			names.Add(54, "септемдециллион");
			names.Add(57, "октодециллион");
			names.Add(60, "новемдециллион");
			names.Add(63, "вигинтиллион");
			names.Add(66, "анвигинтиллион");
			names.Add(69, "дуовигинтиллион");
			names.Add(72, "тревигинтиллион");
			names.Add(75, "кватторвигинтиллион");
			names.Add(78, "квинвигинтиллион");
			names.Add(81, "сексвигинтиллион");
			names.Add(84, "септемвигинтиллион");
			names.Add(87, "октовигинтиллион");
			names.Add(90, "новемвигинтиллион");
			names.Add(93, "тригинтиллион");
			names.Add(96, "антригинтиллион");
			return names;
		}

		/// <summary>
		/// Получить значение числа по степени
		/// </summary>
		/// <param name="power"></param>
		/// <returns></returns>
		private int GetValuePower(int power)
		{
			var a = 0;
			if (_values.ContainsKey(power)) a = _values[power];
			return a;
		}

		private void Normalize(out int big, ref int value)
		{
			big = 0;
			if (value > 1000){
				var v = value;
				value = v % 1000;// остаток от деления на тыщу
				big = v - value;// отнимаем от первоначального значения остаток - и делим на 1000 - то что передаётся дальше
				big /= 1000;
			}
			if (value < 0){
				big = value;
				value = 0;
			}
		}

		public static MegaInt Create(int power, int value)
		{
			var a = new MegaInt();
			a.AddValue(power,value);
			return a;
		}

		/// <summary>
		/// Прибавляем значение (может быть и отрицательным)
		/// </summary>
		/// <param name="power"></param>
		/// <param name="value"></param>
		public void AddValue(int power, int value)
		{
			if (power < 0) return;
			if (!_values.ContainsKey(power)){_values.Add(power, 0);}
			var v=_values[power];
			v += value;
			int big;
			Normalize(out big, ref v);
			_values[power] = v;
			if (big > 0){AddValue(power + 3, big);}
			if (big < 0){AddValue(power - 3, big);}
		}

		/// <summary>
		/// Добавляем другое МегаЦелое
		/// </summary>
		/// <param name="value"></param>
		public void AddValue(MegaInt value)
		{
			foreach (var v in value._values){
				AddValue(v.Key, v.Value);
			}
		}

		/// <summary>
		/// Отнимаем значение
		/// </summary>
		/// <param name="power"></param>
		/// <param name="value"></param>
		public void MinusValue(int power, int value)
		{
			if (power < 0) return;
			if (!_values.ContainsKey(power)) { _values.Add(power, 0); }
			var v = _values[power];
			v -= value;
			int big = 0;
			if (v < 0){big = -1;v += 1000;}// отняли значение и получили меньше 0 значит передаём дальше -1, а текущее значение увеличиваем на тыщу, забирая его из предыдущего (который и отнимаем
			_values[power] = v;
			if (big < 0) MinusValue(power + 3, big);// отнимаем дальше
		}

		/// <summary>
		/// Отнимаем другое МегаЦелое
		/// </summary>
		/// <param name="value"></param>
		public void MinusValue(MegaInt value)
		{
			foreach (var v in value._values)
			{
				MinusValue(v.Key, v.Value);
			}
		}



		/// <summary>
		/// Умножаем на число
		/// </summary>
		/// <param name="mul"></param>
		public void MulValue(int mul)
		{
			var mm1 = mul - 1;
			foreach (var v in _values){
				AddValue(v.Key,v.Value*(mm1));
			}
		}

		/// <summary>
		/// Разделить значение
		/// </summary>
		/// <param name="power">Текущая разделяемая степень</param>
		/// <param name="bigv">Остаток от предыдущего порядка</param>
		/// <param name="div">На сколько делим</param>
		public void DivValue(int power, int bigv, int div)
		{
			if (div<=0)return;
			if (power < 0) return;
			if (!_values.ContainsKey(power)) { _values.Add(power, 0); }
			var v = _values[power];
			v += bigv*1000;
			// делим, полученное число умножаем и отнимаем
			var vt = v/div;// новое значение
			var vNewBig = v - vt*div;
			_values[power] = vt;
			DivValue(power - 3, vNewBig, div);
		}

		/// <summary>
		/// Делим на число
		/// </summary>
		/// <param name="div"></param>
		public void DivValue(int div)
		{
			DivValue(GetMaxPower(), 0, div);
		}

		/// <summary>
		/// Создать копию значений
		/// </summary>
		/// <returns></returns>
		public MegaInt CopyThis()
		{
			var n = new MegaInt();
			foreach (var value in _values){
				n._values.Add(value.Key, value.Value);
			}
			return n;
		}

		/// <summary>
		/// Получить максимальное значение степени
		/// </summary>
		/// <returns></returns>
		public int GetMaxPower()
		{
			var maxPower = 0;
			foreach (var value in _values){
				if (maxPower < value.Key) { maxPower = value.Key; }
			}
			return maxPower;
		}

		public Boolean IsBiggerThen(MegaInt forCompare)
		{
			var a = this.GetMaxPowerReal();
			var b = forCompare.GetMaxPowerReal();
			if (a>b)return true;
			if (a<b)return false;
			var va = this.GetValuePower(a);
			var vb = forCompare.GetValuePower(b);
			if (va >= vb) return true;
			return false;
		}

		/// <summary>
		/// Получить максимальное значение степени с учётом пустых значений. хотя лучше разобраться где они появляются
		/// </summary>
		/// <returns></returns>
		public int GetMaxPowerReal()
		{
			var maxPower = 0;
			foreach (var value in _values)
			{
				if (value.Value == 0) continue;
				if (maxPower < value.Key) { maxPower = value.Key; }
			}
			return maxPower;
		}

		public string GetAsString()
		{
			var maxPower = GetMaxPowerReal();
			var s = "";
			if (_values.ContainsKey(maxPower)) s += _values[maxPower];else s = "000";
			if (_values.ContainsKey(maxPower - 3)){
				s += "," + _values[maxPower - 3];
			}
			s += " " + _names[maxPower];
			return s;
		}

		public string GetAsFullString()
		{
			var s = "";
			foreach (var value in _values){
				s += value.Value + "(" + value.Key + ")";
			}
			return s;
		}

		public string GetAsFullLineString()
		{
			var maxPower = GetMaxPower();
			var s = "> ";
			//for (int i = maxPower; i >= 0; i=i-3){s += i + " ";}

			for (int i = maxPower; i >= 0; i=i-3)
			{
				var sa = "000";
				if (_values.ContainsKey(i)){
					var v = _values[i];
					sa = v.ToString();
					if (v < 10) sa = "0" + sa;
					if (v < 100) sa = "0" + sa;
				}
				s += " " + sa;
			}


			//узнаем максимальную степерь потом через пробел выводим через for все значения
			return s;
		}
	}
}
