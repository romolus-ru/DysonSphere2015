using System;
using System.Drawing;
using Engine;
using Engine.Controllers;
using Engine.Views.Templates;

namespace GalaxyArmy
{
	class MenuButton:Button
	{
		private int _correctHintX;
		public MenuButton(Controller controller) : base(controller)
		{}

		public Boolean Active = false;
		private int _ln;

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			_ln = visualizationProvider.TextLength(Hint);
			if (X + _ln > visualizationProvider.CanvasWidth) _correctHintX = -(X + _ln - visualizationProvider.CanvasWidth+20);
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.RotateReset();
			Color color;
			if (Active) color = Color.White;
			else color = Color.LightGray;
			if (CursorOver&!Active)color = Color.OrangeRed;
			var f = visualizationProvider.FontHeightGet() / 2;

			var i = Y + Height / 2 - f - 3;
			visualizationProvider.SetColor(Color.Black);
			visualizationProvider.Print(X + 5, i+1, Caption);
			visualizationProvider.SetColor(color);
			visualizationProvider.Print(X + 4, i, Caption);
			if (Hint != "" && CursorOver){
				visualizationProvider.OffsetAdd(_correctHintX,0);
				visualizationProvider.SetColor(Color.Black, 60);
				visualizationProvider.Box(X + 7, Y + Height + 3, _ln + 6, 15 + 3, 5);
				visualizationProvider.SetColor(Color.White);
				visualizationProvider.Rectangle(X + 7, Y + Height + 3, _ln + 6, 15 + 3, 5);
				visualizationProvider.SetColor(color);
				visualizationProvider.Print(X + 10, Y + Height + 6 - f, Hint);
				visualizationProvider.OffsetRemove();
				visualizationProvider.Box(X + 4, Y + Height, 40, 3);
			}
			//visualizationProvider.SetColor(Color.GreenYellow);visualizationProvider.Rectangle(X, Y, Width, Height);
		}
	}
}
