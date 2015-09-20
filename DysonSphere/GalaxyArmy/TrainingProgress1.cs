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
	/// Просмотр успешности тренировки войск с возможностью ускорить тренировку и покупкой солдат
	/// </summary>
	class TrainingProgress1:ViewControl
	{
		private GalaxyArmyModel _gam;
		private ArmyOne _army;
		private GAButton _btnRecruit;
		private GAButton _btnBuyTrainingCenter;

		public TrainingProgress1(Controller controller, GalaxyArmyModel gam, ArmyOne army) : base(controller)
		{
			_gam = gam;
			_army = army;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			//дополнительные кнопки
			_btnBuyTrainingCenter = new GAButton(Controller);
			Button.InitButton(_btnBuyTrainingCenter, Controller, Width - 300, 10, 290, 55, "", "Купить тренировочный центр", "Купить дополнительный тренировочный центр", Keys.None, "btnBuyTrainingCenter");
			_btnBuyTrainingCenter.OnPress += BuyPressed;
			AddControl(_btnBuyTrainingCenter);

			_btnRecruit = new GAButton(Controller);
			var b = Button.InitButton(_btnRecruit, Controller, Width - 150, 65, 140, 30, "", "Нанять солдат", "Нанять солдат", Keys.None, "btnRecruitSoldiers");
			_btnRecruit.OnPress += RecruitPressed;
			AddControl(b);

			//_btnBuyModifier = new GAButton(Controller);
			//b = Button.InitButton(_btnBuyModifier, Controller, Width - 110, 50, 100, 30, "", "x1", "Модификатор покупки", Keys.None, "btnBuySoldiersModifier");
			//_btnBuyModifier.OnPress += BuyModifierPressed;
			//AddControl(b);

			//_btnTrainModifier = new GAButton(Controller);
			//b = Button.InitButton(_btnTrainModifier, Controller, Width - 220, 10, 100, 70, "", "x1", "Модификатор скорости тренировки", Keys.None, "btnTrainingSpeedModifier");
			//_btnTrainModifier.OnPress += ChangeTrainingSpeedModifierPressed;
			//AddControl(b);
		
		}

		/// <summary>
		/// Модификатор скорости тренировки
		/// </summary>
		/// <remarks>C формулой будут сложности - нужно будет высчитать сколько успеет натренироваться до выключения</remarks>
		private void ChangeTrainingSpeedModifierPressed()
		{
			
		}

		private void RecruitPressed()
		{
			_gam.RecruitArmy(_army);
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
				tb = cm;
				cm = _gam.RecruitArmyCost(_army);
				var isBiggerThen = cm.IsBiggerThen(tb);
				var cantPress = !isBiggerThen;
				_btnRecruit.CantPress = !cantPress;
				_btnRecruit.Active = !_btnRecruit.CantPress;
				_btnRecruit.SetHint(cm.GetAsString()+" "+tb.GetAsString());
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
			visualizationProvider.Print(X + Width - 300+5, Y + 10-5, "Цена " + _army.TrainingBaseCost.GetAsString());
			visualizationProvider.Print(X + Width - 300+5, Y + 20-5, "Уровень " + (_army.TrainingBaseCount + 1));
		}

	}
}
