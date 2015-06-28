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
	class ScreenUpgrades:ScreenBase
	{
		public ScreenUpgrades(Controller controller, string caption,GalaxyArmyModel gam)
			: base(controller, caption,gam)
		{}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
		}
	}
}
