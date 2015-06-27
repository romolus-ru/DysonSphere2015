using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;

namespace Engine.Views.Templates
{
	/// <summary>
	/// Простое окно
	/// </summary>
	public class WindowSimple:ViewDraggable
	{
		protected int HeaderX;
		protected int HeaderY;
		protected int HeaderWidth;
		protected int HeaderHeight;
		
		public WindowSimple(Controller controller) : base(controller)
		{}

		/// <summary>Установить координаты и размеры области, за которую перемещаем</summary>
		/// <param name="headerX"></param>
		/// <param name="headerY"></param>
		/// <param name="headerWidth"></param>
		/// <param name="headerHeight"></param>
		public void SetHeader(int headerX, int headerY, int headerWidth, int headerHeight)
		{
			HeaderX = X + headerX;
			HeaderY = Y + headerY;
			HeaderWidth = headerWidth;
			HeaderHeight = headerHeight;
		}

		protected override void DrawComponentBackground(Engine.VisualizationProvider visualizationProvider)
		{
			if (CursorOver) visualizationProvider.SetColor(Color.SlateGray, 100);
			else visualizationProvider.SetColor(Color.Goldenrod, 30);
			visualizationProvider.Box(X, Y, Width, Height);
			visualizationProvider.SetColor(Color.Red);
			visualizationProvider.Box(HeaderX, HeaderY, HeaderWidth, HeaderHeight);
		}

		protected override void DrawObject(VisualizationProvider vp)
		{
			vp.SetColor(Color.FloralWhite, 100);
			var x = X;
			var y = Y;
			//vp.SetColor(Color.LightCyan, 70);
			//vp.Box(x, y, Width, Height);

			if (CursorOver)
			{
				vp.SetColor(Color.FloralWhite);
				vp.Rectangle(x + 1, y + 1, Width - 2, Height - 2);
			}

			if (DragStarted)
			{
				vp.SetColor(Color.Lime);
				var cX = CursorPoint.X - CursorPointFrom.X;
				var cY = CursorPoint.Y - CursorPointFrom.Y;
				vp.Rectangle(x + cX, y + cY, Width, Height);
			}
		}

		protected override bool InRangeToDrag(int x, int y)
		{
			if (!CursorOver) return false;
			var d = (HeaderX < x) && (x < HeaderX + HeaderWidth) && (HeaderY < y) && (y < HeaderY + HeaderHeight);
			return d;
		}


		public override void DragIn(int relX, int relY)
		{
			base.DragIn(relX, relY);
			X -= relX;
			Y -= relY;
			HeaderX -= relX;
			HeaderY -= relY;
		}
	}
}
