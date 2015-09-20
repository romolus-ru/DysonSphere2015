using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;
using Engine.Views;
using GalaxyArmy.Model;
using Button = Engine.Views.Templates.Button;

namespace GalaxyArmy
{
	class ScreenUpgrades:ScreenBase
	{
		private List<Upgrade> _upgrades = new List<Upgrade>();
		private List<UpgradeButton> _uButtons=new List<UpgradeButton>();// храним кнопки, которые создаются, что бы потом их удалить перед обновлением
		private int _cursorX;
		private int _cursorY;

		public ScreenUpgrades(Controller controller, string caption, GalaxyArmyModel gam)
			: base(controller, caption, gam)
		{}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			// при добавлении улучшений надо некоторые улучшения прятать, пока технология недоступна
			base.InitObject(visualizationProvider);
			RefreshUpgradesList();
		}

		private void PressOnBuyUpgrade(Upgrade upgrade)
		{
			if (upgrade.State == 2) return;
			upgrade.BuyUpgrade();
			if (upgrade.State == 2){
				Gam.RefreshParams(); 
				RefreshUpgradesList();
			}
		}

		/// <summary>
		/// Обновить список кнопок-улучшений
		/// </summary>
		private void RefreshUpgradesList()
		{
			foreach (var button in _uButtons){RemoveControl(button);}
			_uButtons.Clear();
			int n1 = -1;
			int n2 = 0;
			int countU = 0;
			_upgrades = SortUpgrades();
			foreach (var upgrade in _upgrades){
				n1++;
				if (n1 > 0){
					n1 = 0;
					n2++;
				}
				var b = new UpgradeButton(Controller, Gam.GeneralFactors, upgrade);
				b = (UpgradeButton) Button.InitButton(b, Controller,
					150 + n1*420, 50 + n2*55,
					700, 50, "GABuyUpgradeClick", upgrade.Description, upgrade.Hint, Keys.None, upgrade.BtnText);
				b.OnUpgradeBuyPress += PressOnBuyUpgrade;
				b.RecalcUpgrade();
				_uButtons.Add(b);
				AddControl(b);
				countU++;
				if (countU > 8) break;
			}
			this.CursorEH(this, PointEventArgs.Set(_cursorX - 10000, _cursorY - 10000));
			this.CursorEH(this, PointEventArgs.Set(_cursorX, _cursorY));
		}

		/// <summary>
		/// Сортируем улучшения, нужно определенное количество, самые дешевые
		/// </summary>
		/// <returns></returns>
		/// <remarks>Улучшения берутся из объекта Gam и его поля Upgrades</remarks>
		private List<Upgrade> SortUpgrades()
		{
			var _u = Gam.Upgrades;
			var r = _u.Where(upgrade => upgrade.State != 2).ToList();
			if (Gam.GeneralFactors.CrystalsOpen == 0){// требуется дополнительно убрать те улучшения, в которых есть кристаллы
				r = r.Where(upgrade => upgrade.CostCrystals == 0).ToList();
			}

			r.Sort(delegate(Upgrade x, Upgrade y)
			{
				if (x.Cost.IsBiggerThen(y.Cost)) return 1;
				return -1;
			}
				);
			return r;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			
			// линиями выводится цена отсортированных улучшений
			//visualizationProvider.SetColor(Color.IndianRed);
			//visualizationProvider.Box(X + 50, Y + 100, X + 50 + 300, Y + 100 + 400);
			
			//visualizationProvider.SetColor(Color.White);
			//int n = -1;
			//foreach (var upgrade in _upgrades){
			//	n++;// выводим улучшение на 1 линии. длига линии означает количество значащих чисел
			//	var c = upgrade.Cost;
			//	var m1 = c.GetMaxPowerReal();
			//	var m2 = c.GetValue(m1);
			//	var m = m1;
			//	if (m2 > 10) m++;
			//	if (m2 > 100) m++;
			//	m *= 3;
			//	visualizationProvider.Line(X + 50, Y + 100 + n, X + 50 + m, Y + 100 + n);
			//}
		}

		protected override void Cursor(object o, PointEventArgs args)
		{
			base.Cursor(o, args);
			_cursorX = args.Pt.X;
			_cursorY = args.Pt.Y;
		}
	}
}
