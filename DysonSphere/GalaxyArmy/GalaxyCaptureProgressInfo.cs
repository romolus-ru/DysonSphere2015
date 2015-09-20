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
using Button = Engine.Views.Templates.Button;

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
		private GAButton _sendArmy;
		public GalaxyCaptureProgressInfo(Controller controller, GalaxyArmyModel gam, GalaxyOne galaxy) : base(controller)
		{_gam = gam;_galaxy = galaxy;}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			_sendArmy = new GAButton(Controller);
			Button.InitButton(_sendArmy, Controller, Width - 210, 10, 200, 30, "", "Выслать подкрепление", "Выслать подкрепление для захвата галактики", Keys.None, "btnSendSoldiers");
			_sendArmy.OnPress += SendArmyPressed;
			_sendArmy.CantPress = true;
			AddControl(_sendArmy);
		}

		private void SendArmyPressed()
		{
			if (_sendArmy.CantPress) return;
			_gam.SendArmy(_galaxy.Group);
		}

		private int _updaterCount;
		private string _countArmy1 = "";
		private string _clickCost = "";
		private string _countArmyInGalaxy = "";
		private string _countArmyHeroesInGalaxy = "";
		private string _countArmyAvailable = "";
		private string _incomeMoney = "";
		private Boolean _showRound = false;

		private void UpdateValues()
		{
			var a1 = _gam.Armies[0];
			var ag1 = _galaxy.Army;
			var ar1 = _gam.Armies[0].ReadyArmy;
			var ava = _gam.SendArmyAvailableCount(_galaxy.Group);
			_countArmy1 = a1.ArmyName.PadRight(15)+ar1.GetAsString();
			_clickCost = "Цена клика ".PadRight(15) + _galaxy.ClickCost.GetAsString();
			_countArmyInGalaxy = "Армия ".PadRight(15) + ag1.GetAsString();
			if (_gam.GeneralFactors.Galaxy1ConquerorCount>0)
				_countArmyHeroesInGalaxy = "Герои ".PadRight(15) + _galaxy.ArmyHeroes.GetAsString();
			else _countArmyHeroesInGalaxy = "-";
			_countArmyAvailable = "К отправке ".PadRight(15) + ava.GetAsString();
			_sendArmy.CantPress = !_gam.SendArmyAvailable(_galaxy.Group);
			_incomeMoney = "доход ".PadRight(15) + _galaxy.IncomeMoney.GetAsString();
			_showRound = _galaxy.IncomeMoney.IsBigger0();
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			
			_updaterCount++;if (_updaterCount>10){UpdateValues();_updaterCount = 0;}

			visualizationProvider.SetColor(Color.SeaGreen);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(X + 15, Y + 40, Name);
			visualizationProvider.Print(X + 15, Y + 50, _clickCost);
			visualizationProvider.Print(X + 15, Y + 60, _countArmy1);
			visualizationProvider.Print(X + 15, Y + 70, _countArmyAvailable);
			visualizationProvider.Print(X + 15, Y + 80, _galaxy.TimeDelay + " " + _galaxy.TimeDelayCurrent);
			visualizationProvider.Print(X + 15, Y + 90, _countArmyInGalaxy);
			visualizationProvider.Print(X + 15, Y + 100, _countArmyHeroesInGalaxy);
			visualizationProvider.Print(X + 15, Y + 110, _incomeMoney);
			if (_showRound){
				visualizationProvider.SetColor(Color.Peru);
				visualizationProvider.DrawRound(X + Width/2, Y + Height/2, 40, _galaxy.TimeDelayCurrent, _galaxy.TimeDelay);
			}
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
