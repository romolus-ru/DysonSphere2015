using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Engine.Utils
{
	/// <summary>
	/// Класс для работы с особо большими числами
	/// </summary>
	/// <remarks>Возможна сильное погрешность при работе с вещественными числами - там число 
	/// переводится в дробь, состоящую из числителя и знаменателя, что может сказаться на точности</remarks>
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

		public MegaInt(){}

		public MegaInt(int power, int value) {AddValue(power,value); }

		public static MegaInt Create(int power, int value)
		{
			var a = new MegaInt();
			a.AddValue(power,value);
			return a;
		}

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
			names.Add(99, "*10^99");
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

		/// <summary>
		/// Получить копию значений
		/// </summary>
		/// <returns></returns>
		private Dictionary<int, int> GetValuesCopy()
		{
			var ret = new Dictionary<int, int>();
			foreach (var value in _values){
				if (value.Value==0)continue;
				ret.Add(value.Key, value.Value);
			}
			return ret;
		}

		#region Base operations

		/// <summary>
		/// Сдвигаем число по разрядам вверх, оно не вмещается в отведенную переменную
		/// </summary>
		/// <param name="power"></param>
		/// <param name="shifted"></param>
		private void Normalize1(int power, int shifted=0)
		{
			if (!_values.ContainsKey(power)){_values.Add(power, 0);}
			var v = _values[power] + shifted;
			var tmp = v%1000;// остающееся значение
			int big = (v - tmp)/1000;// передаваемое выше значение
			_values[power] = tmp;
			if (big > 0) Normalize1(power + 3, big);
		}

		/// <summary>
		/// Прибавляем значение
		/// </summary>
		/// <param name="power"></param>
		/// <param name="value"></param>
		/// <remarks>Очень полезный метод, поэтому оставлен открытым</remarks>
		public void AddValue(int power, int value)
		{
			if (power % 3 != 0){// если степень не кратная 3, то надо домножить на нужное значение
				int n = power/3;
				power = power - n;// корректируем степень, ууменьшая её
				var k = 10;
				if (n == 2) k = 100;
				value *= k;// компенсируем уменьшение степени, увеличивая переданное значение
			}
			//if (power%3 != 0) throw new Exception("Степень должна быть кратна трем. полученное значение = " + power);
			if (power < 0) return;
			if (power > 100) return;
			if (!_values.ContainsKey(power)) { _values.Add(power, 0); }
			_values[power] += value;
			Normalize1(power);
		}

		/// <summary>
		/// Отнимаем значение
		/// </summary>
		/// <param name="power"></param>
		/// <param name="value"></param>
		private void MinusValue(int power, int value)
		{
			if (power < 0) return;
			if (power > 100) return;
			if (!_values.ContainsKey(power)) { _values.Add(power, 0); }
			var v = _values[power];
			v -= value;
			int big = 0;
			if (v < 0) { big = 1; v += 1000; }// отняли значение и получили меньше 0 значит передаём дальше 1, а текущее значение увеличиваем на тыщу, забирая его из предыдущего (который и отнимаем
			_values[power] = v;
			Normalize1(power);// нормализуем текущее значение
			// сменил +3 на -3 - надеюсь теперь правильно, иногда возникала непонятная ошибка когда всё становилось заполненным
			if (big > 0) MinusValue(power - 3, big);// отнимаем дальше
		}

		/// <summary>
		/// Умножить на значение
		/// </summary>
		/// <param name="power">Текущая умножаемая степень</param>
		/// <param name="mul">множитель</param>
		private void MulValue(int power, int mul)
		{
			if (mul == 1) return;
			if (power < 0) return;
			if (power > 100) return;
			if (!_values.ContainsKey(power)) return; // нету значения - незачем его создавать

			//var v = _values[power];
			//var vt = v*mul; // новое значение
			_values[power] *= mul;
			Normalize1(power);
		}

		/// <summary>
		/// Разделить значение
		/// </summary>
		/// <param name="power">Текущая разделяемая степень</param>
		/// <param name="prevValue">Предыдущее значение</param>
		/// <param name="div">На сколько делим</param>
		/// <returns>Значение, которое надо прибавить к предыдущему разряду</returns>
		/// <remarks></remarks>
		private int DivValue(int power, int prevValue, int div)
		{
			if (div <= 0) return 0;
			if (power < 0) return 0;
			if (!_values.ContainsKey(power) && prevValue == 0){
				return 0;
			}
			if (!_values.ContainsKey(power)) _values.Add(power, 0);
			var v = _values[power] + prevValue;
			var fl = 1f*v/div;
			if (fl < 1){
				_values[power] = 0;// обнуляем значение
				return v*1000;// передаем дальше значение на которое надо будет умножить разряд для нормального деления
			}
			// число получилось больше 1, можно разделить
			var vt = v / div;// новое значение
			var vNewBig = v - vt * div;
			_values[power] = vt;
			return vNewBig*1000;
		}

		#endregion

		#region MegaInt operations

		/// <summary>
		/// Нормализуем число
		/// </summary>
		public void Normalize()
		{
			var valuesLocal = GetValuesCopy();
			foreach (var value in valuesLocal){// нормализуем каждое отдельное значение
				if (value.Value > 1000)
					Normalize1(value.Key);
			}
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
		/// Умножаем MegaInt на число
		/// </summary>
		/// <param name="mul"></param>
		public void MulValue(int mul)
		{
			var max = GetMaxPowerReal();
			for (int i = max; i >= 0; i -= 3){
				MulValue(i, mul);
			}
		}

		/// <summary>
		/// Делим MegaInt на число
		/// </summary>
		/// <param name="div"></param>
		public void DivValue(int div)
		{
			var max = GetMaxPowerReal();
			var prev = 0;
			for (int i = max; i >= 0; i-=3){
				prev = DivValue(i, prev, div);
			}
		}

		#endregion

		/// <summary>
		/// Умножаем на вещественное число
		/// </summary>
		/// <param name="mul"></param>
		public void MulValue(float mul)
		{
			if (mul== 1) return;
			if (mul < 0) return;
			// узнаем степень
			if (mul > 1000){// если умножаем на большое число то там мелочи не важны - переводим число в целое и умножаем
				int mul1 = Convert.ToInt32(mul);
				MulValue(mul1);
				return;
			}
			int bottomValue = 10000;
			int topValue = (int)(mul * bottomValue);
			MulValue(topValue);
			DivValue(bottomValue);
		}

		/// <summary>
		/// Делим на вещественное число
		/// </summary>
		/// <param name="div"></param>
		public void DivValue(float div)
		{
			MulValue(1/div);
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

		/// <summary>
		/// Больше
		/// </summary>
		/// <param name="forCompare"></param>
		/// <returns></returns>
		public Boolean IsBiggerThen(MegaInt forCompare)
		{
			var a = this.GetMaxPowerReal();
			var b = forCompare.GetMaxPowerReal();
			if (a>b)return true;
			if (a<b)return false;
			var va = this.GetValuePower(a);
			var vb = forCompare.GetValuePower(b);
			if (va > vb) return true;
			return false;
		}

		/// <summary>
		/// Больше нуля
		/// </summary>
		/// <returns></returns>
		public Boolean IsBigger0()
		{
			var a = this.GetMaxPowerReal();
			if (a > 0) return true;
			var va = this.GetValuePower(a);
			if (va > 0) return true;
			return false;
		}

		public int GetValue(int power)
		{
			return _values[power];
		}


		#region ToStrings

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

			for (int i = maxPower; i >= 0; i = i - 3){
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

		#endregion

		public static MegaInt Function1(int baseValue, int level)
		{
			if (baseValue<2)return new MegaInt();// возвращаем пустое значение
			var mi = new MegaInt();
			mi.AddValue(0,baseValue);
			Function1R(mi,baseValue,1,level);// рекурсивная функция
			return mi;
		}

		/// <summary>
		/// Рекурсивная функция
		/// </summary>
		/// <param name="megaInt"></param>
		/// <param name="baseValue"></param>
		/// <param name="curLevel"></param>
		/// <param name="maxLevel"></param>
		private static void Function1R(MegaInt megaInt, int baseValue, int curLevel,int maxLevel)
		{
			if (curLevel > maxLevel) return;
			megaInt.MulValue(baseValue);
			megaInt.MulValue(curLevel);
			Function1R(megaInt, baseValue, curLevel + 1, maxLevel);
		}

		/// <summary>
		/// Получение 100 точек от 0 до maxInt в равной пропорции в зависимости от максимальной степени исходного числа
		/// </summary>
		/// <param name="maxInt"></param>
		/// <returns></returns>
		public List<MegaInt> Function100(MegaInt maxInt,string fileName)
		{
			var a = new FileArchieve(fileName);// создаём
			var ms = new MemoryStream();
			a.AddString(ms, "" + maxInt.GetAsFullLineString() + " : " + Environment.NewLine);
			
			var r = new List<MegaInt>();

			var max = maxInt.CopyThis();
			var maxPower = max.GetMaxPowerReal();// степень числа (кратная 3) (особенность MegaInt)
			float maxValue = max.GetValue(maxPower);// значение максимальной степени
			// количество степеней в переданном числе. надо распределить в каждой степени заданное количество подэлементов
			float stepPower = maxPower/100f;
			float stepValue = maxValue/100f;

			a.AddString(ms, "stepP=" + stepPower + " stepV= " + stepValue + " maxP=" + maxPower + " maxV=" + maxValue + Environment.NewLine);

			for (int i = 1; i <= 100; i++){
				int vp = (int) (Math.Round(i*stepPower));
				int vv = (int) (Math.Truncate(i*stepValue));
				// корректируем в зависимости от степени
				var u = i*stepPower;
				u /= 3;
				var korr1 = 1;
				if (u > .33) korr1 = 10;
				if (u > .66) korr1 = 100;
				vv *= korr1;
				//усилить корректировку, сейчас стыки будут видны

				var mi = new MegaInt(vp, vv);
				r.Add(mi);

				a.AddString(ms,
					i.ToString().PadLeft(3) + "\t u=\t" + (u).ToString().PadRight(7) + "\t" + (i*stepPower).ToString().PadRight(7) +
					"\t p=\t" + vp.ToString().PadRight(3) + "\t v=\t" + vv.ToString().PadRight(4) +"\t"+
					mi.GetAsFullLineString() + Environment.NewLine);
			}

			a.AddStream(fileName, ms);// сохраняем
			a.Dispose();


			return r;
		}
	}
}
