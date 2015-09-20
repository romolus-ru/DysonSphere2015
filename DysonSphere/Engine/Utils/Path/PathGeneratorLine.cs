﻿using System.Collections.Generic;
using System.Drawing;

namespace Engine.Utils.Path
{
	class PathGeneratorLine:PathGenerator
	{
		public override List<Point> Generate(List<Point> basePoints, int count)
		{
			if (basePoints.Count != 2) return base.Generate(basePoints, count);
			return GenerateLinePath(basePoints[0], basePoints[1], count);
		}

		/// <summary>
		/// Генератору линейного пути требуется 2 опорные точки
		/// </summary>
		/// <returns></returns>
		public override int CountBasePoints()
		{
			return 2;
		}

		/// <summary>
		/// Генерация линейного пути по двум опорным точкам
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="count"></param>
		public List<Point> GenerateLinePath(Point p1, Point p2, int count)
		{
			List<Point> _points = new List<Point>();
			float dx = (p2.X - p1.X+0f)/count;
			float dy = (p2.Y - p1.Y+0f)/count;
			float x = p1.X-dx;
			float y = p1.Y-dy;
			for (int i = 0; i <= count; i++){
				x += dx;
				y += dy;
				_points.Add(new Point((int) x, (int) y));
			}
			return _points;
		}

		/// <summary>
		/// Генерация пути безье по четырём опорным точкам
		/// </summary>
		/// <param name="p1"></param>
		/// <param name="p2"></param>
		/// <param name="p3"></param>
		/// <param name="p4"></param>
		/// <param name="count"></param>
		public List<PointF> GenerateBezierPathF(Point p1, Point p2, Point p3, Point p4, int count)
		{
			List<PointF> _points = new List<PointF>();
			float t = 0;
			float dt = (float)(1.0 / count);
			for (int i = 0; i <= count; i++)
			{
				var x = bezier_spline(t, p1.X, p2.X, p3.X, p4.X);
				var y = bezier_spline(t, p1.Y, p2.Y, p3.Y, p4.Y);
				_points.Add(new PointF(x, y));
				t += dt;
			}
			return _points;
		}

		private float bezier_spline(float t, int p1, int p2, int p3, int p4)
		{
			var mu = t;
			var mum1 = 1 - mu;
			var mum13 = mum1 * mum1 * mum1;
			var mu3 = mu * mu * mu;

			var q = mum13 * p1 + 3 * mu * mum1 * mum1 * p2 + 3 * mu * mu * mum1 * p3 + mu3 * p4;
			return q;
		}
	}
}
