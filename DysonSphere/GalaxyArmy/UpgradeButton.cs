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
using Engine.Views.Templates;
using GalaxyArmy.Model;

namespace GalaxyArmy
{
	class UpgradeButton:GAButton
	{
		/// <summary>
		/// Делегат для обмена строками
		/// </summary>
		public delegate void OnUpgradeBuyPressDelegate(Upgrade upgrade);
		public OnUpgradeBuyPressDelegate OnUpgradeBuyPress;

		private Upgrade _u;
		private GeneralFactors _gf;

		public UpgradeButton(Controller controller,GeneralFactors gf, Upgrade upgrade) : base(controller)
		{
			_u = upgrade;
			_gf = gf;
		}

		private int _ln = 0;

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			_ln = visualizationProvider.TextLength(_u.Hint);
			visualizationProvider.LoadTexture("GAUpgradesGroup", @"..\Resources\GalaxyArmy\UpgradesGroup.png");
			visualizationProvider.LoadTexture("GAUpgradesType", @"..\Resources\GalaxyArmy\UpgradesType.png");
		}

		public override void Press()
		{
			if (OnUpgradeBuyPress != null) OnUpgradeBuyPress(_u);
		}

		/// <summary>
		/// Обновить состояние кнопки
		/// </summary>
		public void RecalcUpgrade()
		{
			CantPress = !_u.CanBuy();
		}

		/// <summary>
		/// Для обновления статуса кнопки
		/// </summary>
		private int _num = 0;

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			//base.DrawObject(visualizationProvider);
			const int offset1 = 85;
			_num++;
			if (_num > 10){
				_num = 0;
				RecalcUpgrade();
			}
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(X + 3+offset1, Y + 3, _u.Description);
			visualizationProvider.Print(X + 3+offset1, Y + 13, "Стоимость "+_u.Cost.GetAsString());
			var s = "";
			var color = Color.White;
			if (CantPress) s = "нету нужного количества денег";
			else{
				if (_u.State == 2) {s = "Куплено";
					color = Color.Silver;
				}
				else {s = "Купить";
					color = Color.GreenYellow;
				}
			}
			visualizationProvider.SetColor(color);
			visualizationProvider.Print(X + 3+offset1, Y + 23, s);

			if (Hint != "" && CursorOver){
				visualizationProvider.SetColor(Color.Black, 60);
				visualizationProvider.Box(X + 7, Y + Height + 3, 16+_ln, 25, 5);
				visualizationProvider.SetColor(Color.White);
				visualizationProvider.Rectangle(X + 7, Y + Height + 3, 16+_ln, 25 , 5);
				visualizationProvider.SetColor(Color.Tomato);
				visualizationProvider.Print(X + 10, Y + Height + 6, Hint);
				visualizationProvider.Box(X + 4, Y + Height, 40, 3);
			}

			int n1 = (int) _u.EGroup;
			visualizationProvider.DrawTexturePart(X + 20, Y + 20, "GAUpgradesGroup", 32, 32, n1);

			int n2 = (int) _u.EType;
			visualizationProvider.DrawTexturePart(X + 55, Y + 20, "GAUpgradesType", 32, 32, n2);

		}

		private bool _pr1 = false;

		protected override void Keyboard(object sender, InputEventArgs e)
		{
			if (!CanDraw) return;
			// (если нажата кнопка мыши и мышка находится над кнопкой) или (если нажата кнопка на клавиатуре)
			bool b = e.IsKeyPressed(Keys.LButton);
			bool b2 = e.IsKeyPressed(Key);
			var clickOnButton = b && CursorOver;
			var sk = StateOneKeyboard.Check(b2);
			if (sk == StatesEnum.On){Press();}// нажимаем когда нажали на клавиатуре нужную кнопку
			if (!clickOnButton&&_pr1){Press();_pr1 = false;}// кнопка не нажата и до этого была нажата (отпустили мышку) то вызываем press
			if (clickOnButton) _pr1 = true;
			//if (StateOneKeyboard.CurrentState == StatesEnum.On) e.Handled = true;
			//if (clickOnButton) e.Handled = true;
			//base.Keyboard(sender, e);
		}

	}
}
