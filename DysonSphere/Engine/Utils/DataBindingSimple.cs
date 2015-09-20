using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils
{
	/// <summary>
	/// Простой дата биндинг
	/// </summary>
	/// <remarks>Пример взят оттуда http://rsdn.ru/article/dotnet/Data_Binding_Basics.xml 
	/// там пример чуть покруче - позволяет синхронизировать оба свойства и менять их независимо с конвертацией к другому типу если например int привязан к string</remarks>
	public class DataBindingSimple
	{
		private object _obj;
		private string _fieldName;

		public DataBindingSimple(object obj, string fieldName)
		{
			_obj = obj;
			_fieldName = fieldName;
		}

		/// <summary>
		/// Получаем нужный объект. во вспомогательных методах приводим к нужному типу
		/// </summary>
		/// <returns></returns>
		private object GetObjectProperty()
		{
			/*var fld1 = TypeDescriptor.GetProperties(_obj);
			var fld = fld1.Find(_fieldName, false);
			var ret = fld.GetValue(_obj);*/
			Type t = _obj.GetType(); // взято отсюда https://msdn.microsoft.com/ru-ru/library/66btctbe(v=vs.110).aspx
			// Create an instance of a type.
			//Object[] args = new Object[] { 8 };
			var ret=t.InvokeMember(_fieldName,
			BindingFlags.DeclaredOnly |
			BindingFlags.Public | BindingFlags.NonPublic |
			BindingFlags.Instance | BindingFlags.GetField, null, _obj, null);
			//var fld1 = TypeDescriptor.GetProperties(_obj);
			//var fld = fld1.Find(_fieldName, false);
			//var ret = fld.GetValue(_obj);
			return ret;
		}

		/// <summary>
		/// Устанавливаем свойство
		/// </summary>
		/// <param name="newValue"></param>
		private void SetObjectProperty(object newValue)
		{
			//var fld = TypeDescriptor.GetProperties(_obj).Find(_fieldName, false);
			//fld.SetValue(_obj, newValue);
			Type t = _obj.GetType();
			t.InvokeMember(_fieldName,
			BindingFlags.DeclaredOnly |
			BindingFlags.Public | BindingFlags.NonPublic |
			BindingFlags.Instance | BindingFlags.SetField, null, _obj, new Object[] { newValue });
		}

		public MegaInt GetMegaInt()
		{
			var a = GetObjectProperty();
			return a as MegaInt;
		}

		public int GetInt()
		{
			var a = GetObjectProperty();
			int r = -1;
			if (a != null) r = a is int ? (int) a : -1;
			return r;
		}

		/// <summary>
		/// Установить новое значение для поля типа int
		/// </summary>
		public void SetInt(int newValue)
		{
			SetObjectProperty(newValue);
		}
	}
}
