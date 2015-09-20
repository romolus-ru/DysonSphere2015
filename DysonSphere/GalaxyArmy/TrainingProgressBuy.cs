using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Utils;
using Engine.Views;
using GalaxyArmy.Model;
using Button = Engine.Views.Templates.Button;

namespace GalaxyArmy
{
	/// <summary>
	/// Покупка дополнительных тренировочных баз
	/// </summary>
	/// <remarks>
	/// Каждая следующая база стоит значительно дороже
	/// А как написать такую функцию, надо подумать
	/// </remarks>
	class TrainingProgressBuy:ViewControl
	{
		private GalaxyArmyModel _gam;
		private ArmyOne _army;
		private GAButton _btnBuyTrainingCenter;

		public TrainingProgressBuy(Controller controller, GalaxyArmyModel gam, ArmyOne army)
			: base(controller)
		{
			_gam = gam;
			_army = army;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			_btnBuyTrainingCenter = new GAButton(Controller);
			Button.InitButton(_btnBuyTrainingCenter, Controller, Width - 300, 10, 20, 20, "", "Купить тренировочный центр", "Купить дополнительный тренировочный центр", Keys.None, "btnBuyTrainingCenter");
			_btnBuyTrainingCenter.OnPress += BuyPressed;
			AddControl(_btnBuyTrainingCenter);
		}

		/// <summary>
		/// Купить центр тренировки
		/// </summary>
		private void BuyPressed()
		{
			_gam.BuyTrainingCenter(_army);
		}

		private int pause = 0;

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			pause++;
			if (pause > 20){
				pause = 0;
				var cm = _gam.GeneralFactors.CurrentMoneyGet();
				var tb = _army.TrainingBaseCost;
				_btnBuyTrainingCenter.CantPress = !cm.IsBiggerThen(tb);
				_btnBuyTrainingCenter.Active = !_btnBuyTrainingCenter.CantPress;
			}
			const int pad1 = 15;
			var n = Height / 2;
			visualizationProvider.SetColor(Color.AliceBlue);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			visualizationProvider.Print(X + Height + 10, Y + 10, Name);
			visualizationProvider.Print(X + Height + 10, Y + 20, "Армия ".PadLeft(pad1,' ')+_army.ReadyArmy.GetAsString());
			visualizationProvider.Print(X + Height + 10, Y + 30, "Тренируются ".PadLeft(pad1,' ') + _army.TrainingArmy.GetAsString());
			if (_army.TrainingArmy.IsBigger0()){
				visualizationProvider.SetColor(Color.Firebrick);
				visualizationProvider.DrawRound(X + n, Y + n, n - 25, _army.TimeDelayCurrent, _army.TimeDelay);
			}
			visualizationProvider.SetColor(Color.PowderBlue);
			visualizationProvider.Print(X + Width - 300, Y + 10, "Цена " + _army.TrainingBaseCost.GetAsString());
			visualizationProvider.Print(X + Width - 300, Y + 20, "Уровень " + (_army.TrainingBaseCount + 1));

		}

	}
}
