﻿using System;

namespace Engine.Attributes
{
	/// <summary>
	/// Атрибут имени класса, как его запомнит коллектор
	/// </summary>
	/// <remarks>
	/// Разрешено только 1 имя у объекта
	/// При совпадении имени с уже имеющимся в коллекторе произойдёт или 
	/// замещение объекта в коллекторе новым или ошибка
	/// Для стандартных классов, которые ищет коллектор (View Module и т.п.)
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class ClassNameAttribute : Attribute
	{
		private readonly string _className;

		/// <summary>
		/// конструктор
		/// </summary>
		/// <param name="className"></param>
		public ClassNameAttribute(string className)
		{
			_className = className;
		}

		/// <summary>
		/// имя объекта
		/// </summary>
		public string ClassName
		{
			get { return _className; }
		}
	}
}
