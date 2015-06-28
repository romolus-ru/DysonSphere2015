using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Controllers;
using Engine.Views.Templates;

namespace GalaxyArmy
{
	class MenuButton:Button
	{
		public MenuButton(Controller controller) : base(controller)
		{}

		public Boolean Active = false;
		private int _ln = 0;

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			_ln = visualizationProvider.TextLength(Hint);
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.RotateReset();
			String txt;
			Color color;
			if (Active) color = Color.White;
			else color = Color.LightGray;
			if (CursorOver)color = Color.OrangeRed;
			var f = visualizationProvider.FontHeightGet() / 2;

			var i = Y + Height / 2 - f - 3;
			visualizationProvider.SetColor(Color.Black);
			visualizationProvider.Print(X + 5, i+1, Caption);
			visualizationProvider.SetColor(color);
			visualizationProvider.Print(X + 4, i, Caption);
			if (Hint != "" && CursorOver){
				visualizationProvider.SetColor(Color.Black, 60);
				visualizationProvider.Box(X + 7, Y + Height + 3, _ln+6, 15+3);
				visualizationProvider.SetColor(color);
				visualizationProvider.Print(X + 10, Y + Height + 5 - f, Hint);
				visualizationProvider.Box(X + 4, Y + Height, 40, 3);
			}
			//visualizationProvider.SetColor(Color.GreenYellow);visualizationProvider.Rectangle(X, Y, Width, Height);
		}
	}
}
