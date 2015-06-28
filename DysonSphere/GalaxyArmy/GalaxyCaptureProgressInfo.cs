using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;
using Engine.Views;
using GalaxyArmy.Model;

namespace GalaxyArmy
{
	/// <summary>
	/// Информация об одной галактике - сколько войск? сколько захватили и т.п. плюс возможность нажимать для вытрясывания денег
	/// </summary>
	class GalaxyCaptureProgressInfo:ViewControl
	{
		private GalaxyOne _galaxy;
		private GalaxyArmyModel _gam;
		private StateOne _click=new StateOne();
		public GalaxyCaptureProgressInfo(Controller controller, GalaxyArmyModel gam, GalaxyOne galaxy) : base(controller)
		{_gam = gam;_galaxy = galaxy;}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			visualizationProvider.SetColor(Color.SeaGreen);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(X + 15, Y + 40, Name);
			visualizationProvider.Print(X + 15, Y + 50, "Цена клика "+_galaxy.ClickCost.GetAsString());
			visualizationProvider.Print(X + 15, Y + 60, _galaxy.TimeDelay + " " + _galaxy.TimeDelayCurrent);
			visualizationProvider.SetColor(Color.Peru);
			visualizationProvider.DrawRound(X + Width / 2, Y + Height / 2, 40, _galaxy.TimeDelayCurrent, _galaxy.TimeDelay);
		}

		protected override void Keyboard(object o, InputEventArgs args)
		{
			base.Keyboard(o, args);
			if (!CursorOver) return;
			var r = _click.Check(args.IsKeyPressed(Keys.LButton));
			if (r == StatesEnum.On){
				_gam.ClickOnGalaxy(_galaxy);
				var a = Parent as ScreenSendArmy;
				if (a != null) a.Click(args.CursorX, args.CursorY+Y, _galaxy.ClickCost);
			}
		}
	}
}
