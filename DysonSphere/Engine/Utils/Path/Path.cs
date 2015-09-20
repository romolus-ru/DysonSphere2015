using System;
using System.Collections;
using System.Collections.Generic;

namespace Engine.Utils.Path
{
	/// <summary>
	/// Содержит опорные и остальные точки пути
	/// </summary>
	public class Path
	{
		/// <summary>
		/// Точки пути
		/// </summary>
		private List<Point> _points = new List<Point>();

		private List<PathSegment> _segments=new List<PathSegment>();

		public int Num = 0;
		static public Random Rnd = new Random();

		public IEnumerable Points()
		{
			foreach (Point p in _points)
			{
				yield return p;
			}
		}

		public int CountPoints { get { return _points.Count; } }

		public Point this[int i]
		{
			get{
				var n = i;
				while (n > _points.Count) { n -= _points.Count; }
				return _points[n];
			}
			set{
				var n = i;
				while (n > _points.Count) { n -= _points.Count; }
				_points[n] = value;
			}
		}

		/// <summary>
		/// Нормализуем точки. в данном случае удаляем соседние точки с одинаковыми координатами
		/// </summary>
		public void Normalize()
		{
			// последнюю точку не удаляем. даже если она совпадает с первой - это позволит сделать замкнутый путь
			var c = _points.Count;
			if (c == 0) return;
			for (int i = c - 1; i > 0; i--){// удаляем дублирующую точку в списке
				if (_points[i] == _points[i - 1]) _points.Remove(_points[i]);
			}
			c = _points.Count;
			var pt = _points[0];// удаляем точки ближе определенного расстояния друг к другу
			for (int i = c - 1; i > 1; i--){// удаляем дублирующую точку в списке
				var pt2 = _points[i];
				var d = Distance(pt.X, pt.Y, pt2.X, pt2.Y);
				if (d < 10){// удаляем точку, но только если она не самая последняя
					if (i!=0)_points.Remove(_points[i]);
				}else{
					pt = pt2;
				}
				//if (_points[i] == _points[i - 1]) _points.Remove(_points[i]);
			}

		}

		/// <summary>
		/// Вычисляем расстояние между координатами
		/// </summary>
		/// <returns></returns>
		/// <remarks>Очень часто появляющаяся функция..</remarks>
		public float Distance(int x, int y, int X, int Y)
		{
			var dx = x - X;
			var dy = y - Y;
			return (float)Math.Sqrt(dx * dx + dy * dy);
		}

		public void ClearAllPoints()
		{
			_points.Clear();
			_segments.Clear();
		}

		public void AddSegment(string sName,string pgName, List<Point> basePoints, int count)
		{
			var sg = new PathSegment();
			sg.SegmentName = sName;
			sg.PathGeneratorName = pgName;
			sg.BasePoints = basePoints;
			sg.Count = count;
			_segments.Add(sg);
		}

		/// <summary>
		/// Переустанавливаем опорные точки и количество точек в сегменте
		/// </summary>
		/// <param name="sName"></param>
		/// <param name="basePoints"></param>
		/// <param name="count"></param>
		public void ResetPoints(string sName, List<Point> basePoints, int count)
		{
			foreach (var segment in _segments){
				if (segment.SegmentName == sName){
					segment.BasePoints = basePoints;
					segment.Count = count;
					break;
				}
			}
		}

		/// <summary>
		/// Генерация точек с нормализацией
		/// </summary>
		public void GeneratePoints()
		{
			foreach (var sg in _segments){
				var pts = PathFactory.GetPoints(sg.PathGeneratorName, sg.BasePoints, sg.Count);
				_points.AddRange(pts);
			}
			Normalize();
		}

	}
}
