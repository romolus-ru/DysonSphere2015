using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Controllers;
using Engine.Utils.Path;
using Engine.Views;
using Point = Engine.Utils.Path.Point;

namespace ZEditorExample
{
	/// <summary>
	/// Объект умеет создавать точки линии и выводить их
	/// Устаревший объект, всё перенесено в DataLine
	/// </summary>
	class ViewLine1:ViewObject
	{
		public Path p1;
		private Point ptr1=new Point(10,10);
		private Point ptr2 = new Point(700, 100);

		public ViewLine1(Controller controller)
		{}

		/// <summary>
		/// Инициируем точки линии
		/// </summary>
		public void InitLine()
		{
			p1=new Path();
			List<Point> list1=new List<Point>();
			list1.Add(ptr1);
			list1.Add(ptr2);
			p1.ClearAllPoints();
			p1.AddSegment("","Line",list1,10);
			p1.GeneratePoints();
		}

		public override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			vp.SetColor(Color.Aquamarine);
			for (int i = 1; i < p1.CountPoints; i++){
				var pt1 = p1[i - 1];
				var pt2 = p1[i];
				vp.Line(pt1.X, pt1.Y, pt2.X, pt2.Y);
			}
			vp.SetColor(Color.LawnGreen);
			vp.Line(ptr1.X, ptr1.Y+5, ptr2.X, ptr2.Y+5);
		}
	}

}
