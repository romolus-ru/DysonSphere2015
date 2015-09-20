using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils.Editor;
using Engine.Utils.Path;

namespace ZEditorExample.DataObjects
{
	/// <summary>
	/// Объединяет 2 DataProcessor
	/// </summary>
	class DataLine:IDataHolder
	{
		public int P1Num;
		public int P2Num;
	
		public int x1;
		public int y1;
		public int x2;
		public int y2;

		public int Dx1;
		public int Dy1;
		public int Dx2;
		public int Dy2;

		public int Cur=0;
		public DataProcessor dp1;
		public DataProcessor dp2;

		/// <summary>
		/// Путь из точек для вывода на экран
		/// </summary>
		public Path Path=new Path();
		/// <summary>
		/// Опорные точке для построения пути. для отладки и проверки
		/// </summary>
		public List<Point> basePoints=new List<Point>();

		public int Num { get; set; }
		public Dictionary<string, string> Save()
		{
			var d = new Dictionary<String, String>();
			d.Add("P1Num", P1Num.ToString());
			d.Add("P2Num", P2Num.ToString());
			return d;

		}

		public void Load(Dictionary<string, string> data)
		{
			P1Num = Convert.ToInt32(data["P1Num"]);
			P2Num = Convert.ToInt32(data["P2Num"]);
		}

		/// <summary>
		/// Пересчитываем вспомогательные параметры когда dp1 и dp2 уже заданы (и p1num и p2num тоже)
		/// </summary>
		public void InitValuesFromDataProcessors()
		{
			P1Num = dp1.Num;
			P2Num = dp2.Num;
			x1 = dp1.PosX;
			y1 = dp1.PosY;
			x2 = dp2.PosX;
			y2 = dp2.PosY;
			Dx1 = dp1.Height / 2;
			Dy1 = dp1.Width / 2;
			Dx2 = dp2.Height / 2;
			Dy2 = dp2.Width / 2;
			RefreshPath();
		}

		public void RefreshPath()
		{
			var dx = (x2 - x1) / 4;
			var dy = (y2 - y1) / 4;

			var a1 = Math.Atan2(dx, dy)+Math.PI/8;
			int degree = (int)(a1 * 180 / Math.PI);
			if (degree < 0) degree += 360;
			if (degree >360) degree -= 360;
			degree /= 45;// приводим к конкретному углу, одному из 8

			Dictionary<int, Point> dp = new Dictionary<int, Point>()
			{
				{0, new Point( 0, 1)},
				{1, new Point( 1, 1)},
				{2, new Point( 1, 0)},
				{3, new Point( 1,-1)},
				{4, new Point( 0,-1)},
				{5, new Point(-1,-1)},
				{6, new Point(-1, 0)},
				{7, new Point(-1, 1)}
			};

			var p2 = dp[degree];
			var dx1 = p2.X*dp1.Width/2;
			var dy1 = p2.Y*dp1.Height/2;

			degree += 4;
			if (degree > 7) degree -= 8;
			var p3 = dp[degree];
			var dx2 = p3.X * dp2.Width / 2;
			var dy2 = p3.Y * dp2.Height / 2;


			var pt1 = new Point(x1 + dx1, y1 + dy1);
			var pt4 = new Point(x2 + dx2, y2 + dy2);

			var dx45 = dx - dy;// переворачиваем "единичный" вектор на 90 градусов и вычисляем новый суммарный вектор, 45градусов
			var dy45 = dy + dx;

			var pt2 = new Point(pt1.X + dx45, pt1.Y + dy45);// пускаем безье по точкам трапеции
			var pt3 = new Point(pt4.X - dy45, pt4.Y + dx45);

			basePoints.Clear();
			basePoints.Add(pt1);
			basePoints.Add(pt2);
			basePoints.Add(pt3);
			basePoints.Add(pt4);

			Path=new Path();
			Path.ClearAllPoints();
			Path.AddSegment("", "Bezier", basePoints, 30);
			Path.GeneratePoints();
		}

	}
}
