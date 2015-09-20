using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Views;
using Button = Engine.Views.Templates.Button;

namespace ZEditorExample
{
	class View1:ViewControl
	{
		private const int menuPosY = 8;

		public View1(Controller controller) : base(controller)
		{}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			var b = Button.InitButton(new Button(Controller), Controller, 950, menuPosY, 74, 30, "exit", "Выход", "Esc",
				Keys.Escape, "btnExit");
			AddControl(b);
				b = Button.InitButton(new Button(Controller), Controller, 950, menuPosY+35, 74, 30, "Objs", "Объекты", "1",
				Keys.D1, "btnObjs");
			AddControl(b);
			b = Button.InitButton(new Button(Controller), Controller, 950, menuPosY + 70, 74, 30, "Lines", "Линии", "2",
			Keys.D2, "btnLines");
			AddControl(b);
			b = Button.InitButton(new Button(Controller), Controller, 950, menuPosY + 105, 74, 30, "ParamNames", "Имена параметров", "3",
			Keys.D3, "btnParamNames");
			AddControl(b);
			b = Button.InitButton(new Button(Controller), Controller, 950, menuPosY + 140, 74, 30, "LinkParam", "Связь с параметром", "4",
			Keys.D4, "btnLinkParams");
			AddControl(b);
			b = Button.InitButton(new Button(Controller), Controller, 950, menuPosY + 175, 74, 30, "Zoom", "Масштаб", "5",
			Keys.D5, "btnZoom");
			AddControl(b);
			visualizationProvider.LoadFont("fontTest","Century Gothic",8);
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			visualizationProvider.DrawTexture(1024/2, 90/2, "topMenu");
			visualizationProvider.SetColor(Color.GreenYellow);
			visualizationProvider.SetFont("default");
			visualizationProvider.Print(20, 100, "Текст для примера первый");
			visualizationProvider.SetFont("default2");
			visualizationProvider.Print(20, 120, "Текст для примера второй");
			visualizationProvider.SetFont("fontTest");
			visualizationProvider.Print(20, 140, "ТЕКСТ для примера третий");
			visualizationProvider.SetFont("default");
		}

	}
}
