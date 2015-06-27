using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;

namespace Engine.Views.Templates
{
	/// <summary>
	/// Перемещаемый вспомогательный объект для трекбара
	/// </summary>
	class DragObject:ViewDraggable
	{
		public delegate void OnNewPos();
		public event OnNewPos NewPos;

		public Boolean IsVertical = false;
		public DragObject(Controller controller) : base(controller)
		{}

		public override void DragIn(int relX, int relY)
		{
			base.DragIn(relX, relY);
			X -= relX;
			Y -= relY;
			Correct();
		}

		public void CorrectSize(int size)
		{
			Width = size;
			Height = size;
			Correct();
		}

		public void Correct()
		{
			X = CorrectX(X);
			Y = CorrectY(Y);
		}

		private int CorrectX(int x)
		{
			var ret = x;
			if (ret < 0) ret = 0;
			if (ret + Width > Parent.Width) ret = Parent.Width - Width;
			if (IsVertical) ret = (Parent.Width - Width)/2;
			return ret;
		}

		private int CorrectY(int y)
		{
			var ret = y;
			if (ret < 0) ret = 0;
			if (ret + Height > Parent.Height) ret = Parent.Height - Height;
			if (!IsVertical) ret = (Parent.Height - Height)/2;
			return ret;
		}

		protected override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			if (DragStarted){
				vp.SetColor(Color.White);
				var cX = CorrectX(X + CursorPoint.X - CursorPointFrom.X + _cursorCorrection.X);
				var cY = CorrectY(Y + CursorPoint.Y - CursorPointFrom.Y + _cursorCorrection.Y);
				vp.Rectangle(cX + 3 - _cursorCorrection.X, cY + 3 - _cursorCorrection.Y, Width - 6, Height - 6);
			}else{
				vp.SetColor(Color.SkyBlue);
			}
			vp.Rectangle(X - _cursorCorrection.X, Y - _cursorCorrection.Y, Width, Height);
		}

		public override void DragEnd(int relX, int relY)
		{
			base.DragEnd(relX, relY);
			if (NewPos != null) NewPos();
		}
	}
}
