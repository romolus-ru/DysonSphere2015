using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Views;
using GalaxyArmy.Model;
using Button=Engine.Views.Templates.Button;

namespace GalaxyArmy
{
	class ViewScreen:ViewControl
	{
		private const int menuPosY = 8;
		private int _scrollOffset = 0;
		private List<ViewControl> _screens = new List<ViewControl>();
		private GalaxyArmyModel _gam;
		public ViewScreen(Controller controller, GalaxyArmyModel gam) : base(controller)
		{
			_gam = gam;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			var b = Button.InitButton(new MenuButton(Controller), Controller, 950, menuPosY, 74, 30, "systemExit", "Выход", "Esc",
				Keys.Escape, "btnExit");
			AddControl(b);

			b = Button.InitButton(new MenuButton(Controller), Controller, 000, menuPosY, 74, 30, "GATotal", "   X", "Общая информация",
				Keys.D0, "btnTotal");
			AddControl(b);

			b = Button.InitButton(new MenuButton(Controller), Controller, 100, menuPosY, 74, 30, "GASendArmy", "Завоевания", "Отправить армии",
				Keys.D1, "btnSendArmy");
			AddControl(b);

			b = Button.InitButton(new MenuButton(Controller), Controller, 200, menuPosY, 74, 30, "GAManagement", "Управление", "Управление инфраструктурой",
				Keys.D2, "btnManagement");
			AddControl(b);

			b = Button.InitButton(new MenuButton(Controller), Controller, 300, menuPosY, 74, 30, "GATraining", "Тренировка", "Тренировка войск",
				Keys.D3, "btnTraining");
			AddControl(b);

			b = Button.InitButton(new MenuButton(Controller), Controller, 400, menuPosY, 74, 30, "GAInstructors", "Инструкторы", "Найм и улучшение инструкторов",
				Keys.D4, "btnInstructors");
			AddControl(b);

			b = Button.InitButton(new MenuButton(Controller), Controller, 500, menuPosY, 74, 30, "GAUpgrades", "Улучшения", "Улучшения",
				Keys.D4, "btnUpgrades");
			AddControl(b);

			AddScreen(0, "GATotal", new ScreenTotal(Controller, "Общая информация", _gam));
			AddScreen(1, "GASendArmy", new ScreenSendArmy(Controller, "Отправка армий", _gam));
			AddScreen(2, "GAManagement", new ScreenManagement(Controller, "Управление инфраструктурой", _gam));
			AddScreen(3, "GATraining", new ScreenTraining(Controller,"Тренировка войск",_gam));
			AddScreen(4, "GAInstructors", new ScreenInstructors(Controller,"Инструкторы",_gam));
			AddScreen(5, "GAUpgrades", new ScreenUpgrades(Controller,"Улучшения",_gam));

			Controller.AddEventHandler("GATotal", GATotalEH);
			Controller.AddEventHandler("GASendArmy", GASendArmyEH);
			Controller.AddEventHandler("GAManagement", GAManagementEH);
			Controller.AddEventHandler("GATraining", GATrainingEH);
			Controller.AddEventHandler("GAInstructors", GAInstructorsEH);
			Controller.AddEventHandler("GAUpgrades", GAUpgradesEH);

			visualizationProvider.LoadTexture("topMenu", @"..\Resources\GalaxyArmy\topMenue.png");
		}

		private void AddScreen(int pos,string name,ViewControl screen)
		{
			screen.SetName(name);
			screen.SetCoordinates(1024*pos,130);
			AddControl(screen);
			_screens.Add(screen);
		}

		private void GATotalEH(object sender, EventArgs e)
		{
			MoveToScreen("GATotal");
			SetActive((Button)sender);
		}

		private void GAUpgradesEH(object sender, EventArgs e)
		{
			MoveToScreen("GAUpgrades");
			SetActive((Button)sender);
		}

		private void GAInstructorsEH(object sender, EventArgs e)
		{
			MoveToScreen("GAInstructors");
			SetActive((Button)sender);
		}

		private void GATrainingEH(object sender, EventArgs e)
		{
			MoveToScreen("GATraining");
			SetActive((Button)sender);
		}

		private void GAManagementEH(object sender, EventArgs e)
		{
			MoveToScreen("GAManagement");
			SetActive((Button)sender);
		}

		private void GASendArmyEH(object sender, EventArgs e)
		{
			MoveToScreen("GASendArmy");
			SetActive((Button) sender);
		}

		private void MoveToScreen(string name)
		{
			ViewControl ret = null;
			foreach (var screen in _screens){
				if (screen.Name == name){
					ret = screen;break;
				}
			}
			if (ret!=null)_scrollOffset = ret.X;
		}

		private void SetActive(Button btn)
		{
			foreach (var control in Controls){
				var a = control as MenuButton;
				if (a == null) continue;
				a.Active = (a == btn);
			}
		}

		/// <summary>
		/// Перемещаем все экраны в указанную позицию
		/// </summary>
		private void Scroll()
		{
			var sc = _scrollOffset / 10;
			if (Math.Abs(_scrollOffset) < 100) sc = _scrollOffset/3;
			if (Math.Abs(_scrollOffset) < 10){
				sc = _scrollOffset;
			}
			_scrollOffset -= sc;
			foreach (var control in _screens){
				var x = control.X - sc;
				var y = control.Y;
				control.SetCoordinates(x, y);
			}

		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			_gam.Execute();
			base.DrawObject(visualizationProvider);
			visualizationProvider.DrawTexture(1024/2, 90/2, "topMenu");
			if (_scrollOffset != 0) Scroll();
			visualizationProvider.SetColor(Color.GreenYellow);
			visualizationProvider.Print(20, 100, "Money :" + _gam.CurrentMoney());
		}

	}
}
