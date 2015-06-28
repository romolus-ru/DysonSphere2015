using System;
using System.Drawing;
using Engine;
using Engine.Controllers;
using Engine.Utils;
using GalaxyArmy.Model;

namespace GalaxyArmy
{
	/// <summary>
	/// Вывод информации о галактике, сколько там армии, сколько населения, сколько оккупировано, сколько доход и т.п.
	/// </summary>
	class ScreenSendArmy:ScreenBase
	{
		private ViewClicks viewClicks;

		public ScreenSendArmy(Controller controller, string caption, GalaxyArmyModel gam)
			: base(controller, caption, gam)
		{}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			var gai = new GalaxyCaptureProgressInfo(Controller, Gam, Gam.Galaxies[0]);
			gai.SetParams(150, 100, 600, 400, "Галактика 1");
			AddControl(gai);
			viewClicks = new ViewClicks();
		}

		public void Click(int x, int y, MegaInt added)
		{
			viewClicks.ClickAdd(x, y, added);
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			viewClicks.Draw(visualizationProvider);
		}
	}
}
