using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils.Path
{
	static class PathFactory
	{
		/// <summary>
		/// Поддерживаемые генераторы
		/// </summary>
		private static Dictionary<string, PathGenerator> _generators=new Dictionary<string, PathGenerator>();

		/// <summary>
		/// Получить генератор пути по имени
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		static public PathGenerator GetGenerator(string name)
		{
			if (_generators.ContainsKey(name)) return _generators[name];
			return new PathGenerator();
		}

		static PathFactory()
		{
			RegisterGenerator("Line", new PathGeneratorLine());
			RegisterGenerator("Bezier", new PathGeneratorBezier());
		}
		/// <summary>
		/// Зарегистрировать генератор пути
		/// </summary>
		/// <param name="name"></param>
		/// <param name="pathGenerator"></param>
		static public void RegisterGenerator(string name, PathGenerator pathGenerator)
		{
			if (_generators.ContainsKey(name)) _generators.Remove(name);
			_generators.Add(name, pathGenerator);
		}

		/// <summary>
		/// Удалить зарегистрированный генератор
		/// </summary>
		/// <param name="name"></param>
		static public void UnregisterGenerator(string name)
		{
			if (_generators.ContainsKey(name)) _generators.Remove(name);
		}

		/// <summary>
		/// Генерация точек
		/// </summary>
		/// <param name="name"></param>
		/// <param name="basePoints"></param>
		/// <param name="count"></param>
		static public List<Point> GetPoints(string name, List<Point> basePoints, int count)
		{
			if (!_generators.ContainsKey(name)) return null;
			var g = _generators[name];
			var pts=g.Generate(basePoints, count);
			return pts;
		}

	}
}
