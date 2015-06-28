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
	class TrainProgress1:ViewControl
	{
		private GalaxyArmyModel _gam;
		private ArmyOne _army;
		private GAButton _btnModifier;
		private int _mod1 = 0;

		public TrainProgress1(Controller controller, GalaxyArmyModel gam, ArmyOne army) : base(controller)
		{
			_gam = gam;
			_army = army;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			//дополнительные кнопки
			var btn = new GAButton(Controller);
			var b = Button.InitButton(btn, Controller, Height + 200, 10, 100, 30, "", "Покупка", "Купить дополнительных солдат", Keys.None, "btnBuySoldiers");
			btn.OnPress += BuyPressed;
			AddControl(b);

			_btnModifier = new GAButton(Controller);
			b = Button.InitButton(_btnModifier, Controller, Height + 300, 10, 50, 30, "", "x1", "Модификатор покупки", Keys.None, "btnBuySoldiersModifier");
			_btnModifier.OnPress += BuyModifierPressed;
			AddControl(b);
		}

		private void BuyModifierPressed()
		{
			_mod1++;
			if (_mod1 > 3) _mod1 = 0;
			if (_mod1 == 0) { _btnModifier.SetCaption("x1"); }
			if (_mod1 == 1) { _btnModifier.SetCaption("25%"); }
			if (_mod1 == 2) { _btnModifier.SetCaption("50%"); }
			if (_mod1 == 3) { _btnModifier.SetCaption("max"); }
		}

		private void BuyPressed()
		{
			_gam.BuyArmy(_army, _mod1);
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			const int pad1 = 15;
			var n = Height / 2;
			visualizationProvider.SetColor(Color.AliceBlue);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			visualizationProvider.Print(X + Height + 10, Y + 10, Name);
			visualizationProvider.Print(X + Height + 10, Y + 20, "Армия ".PadLeft(pad1,' ')+_army.ReadyArmy.GetAsString());
			visualizationProvider.Print(X + Height + 10, Y + 30, "Тренируются ".PadLeft(pad1,' ') + _army.TrainingArmy.GetAsString());
			visualizationProvider.Print(X + Height + 10, Y + 40, "Можно купить ".PadLeft(pad1,' ') + _army.BuyMaxArmy.GetAsString());
			visualizationProvider.SetColor(Color.Firebrick);
			visualizationProvider.DrawRound(X + n, Y + n, n - 25, _army.TimeDelayCurrent, _army.TimeDelay);
		}

	}
}
