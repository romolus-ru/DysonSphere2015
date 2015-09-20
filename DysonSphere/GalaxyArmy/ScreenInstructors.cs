using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Engine;
using Engine.Controllers;
using Engine.Views;
using GalaxyArmy.Model;

namespace GalaxyArmy
{
	class ScreenInstructors:ScreenBase
	{

		private InstructorInfo ii;

		public ScreenInstructors(Controller controller, string caption, GalaxyArmyModel gam)
			: base(controller,caption,gam)
		{
			ii = new InstructorInfo(Controller);
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);


			for (int i = 1; i < 9; i++){
				visualizationProvider.SetColor(Color.SeaGreen);
				visualizationProvider.Print(X + 150, Y + i * 10 + 80, ii.getCost(i).GetAsString());
				visualizationProvider.SetColor(ii.GetColor(i));
				visualizationProvider.Box(X + 90, Y + i*10 + 80 + 8, 40, 8, 4);
			}

		}

	}
}