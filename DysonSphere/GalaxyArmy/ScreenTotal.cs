using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Utils;
using GalaxyArmy.Model;
using Button = Engine.Views.Templates.Button;

namespace GalaxyArmy
{
	/// <summary>
	/// Общая информация. пока выводятся просто тестовые данные
	/// </summary>
	class ScreenTotal:ScreenBase
	{
		private MegaInt v1;
		private string v1s;
		private GAButton _btnSave;

		public ScreenTotal(Controller controller, string caption, GalaxyArmyModel gam) : base(controller, caption,gam){}
		
		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			v1 = MegaInt.Function1(3, 50);
			v1s = v1.GetAsFullLineString();
			a = MegaInt.Create(9, 500);
			a.AddValue(18, 0);

			_btnSave = new GAButton(Controller);
			var b = Button.InitButton(_btnSave, Controller, 110, 110, 100, 100, "", "Сохранить", "Сохранить", Keys.None, "btnSave1");
			_btnSave.OnPress += SavePressed;
			AddControl(b);

		}

		private void SavePressed()
		{
			Gam.Save("u1u");
		}

		private MegaInt a;

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			visualizationProvider.SetColor(Color.Turquoise);
			visualizationProvider.Print(X + 50, Y + 50, v1s);
			visualizationProvider.Print(X + 50, Y + 70, "Проект пока завершён - много надо доделывать и оттачивать баланс, нужно много времени");
			//visualizationProvider.Rectangle(X + 300-50, Y + 100-50, 500+100, 400+100, 30+50);
			//visualizationProvider.Box(X + 300, Y + 100, 500, 400, 30);
			
			//visualizationProvider.SetColor(Color.SeaGreen);
			//var b = a.CopyThis();

			//_y1 = 50;
			//Draw1("value  ", b, visualizationProvider);b.AddValue(6, 10000);b.AddValue(6, 100000);b.AddValue(9, 1000);
			//Draw1("++++x  ", b, visualizationProvider);b.MulValue(5);
			//Draw1("  *5   ", b, visualizationProvider);b.DivValue(5);
			//Draw1("  /5   ", b, visualizationProvider);b.MulValue(2.5f);
			//Draw1("  *2.5 ", b, visualizationProvider);b.DivValue(2.5f);
			//Draw1("  /2.5 ", b, visualizationProvider);
		}

		//private int _y1;
		//private void Draw1(string txt, MegaInt value, VisualizationProvider vp)
		//{
		//	_y1 += 10;
		//	vp.Print(X + 50, Y + _y1, txt + " " + value.GetAsFullLineString());
		//}
	}
}
