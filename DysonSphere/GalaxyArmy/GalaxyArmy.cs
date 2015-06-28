using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Views.Templates;
using GalaxyArmy.Model;
using Button = Engine.Views.Templates.Button;

namespace GalaxyArmy
{
	public class GalaxyArmy : Module
	{
		private ViewScreen _scr;
		private GalaxyArmyModel _gam;

		protected override void SetUpView(Engine.Views.View view, Controller controller)
		{

			_gam=new GalaxyArmyModel(Controller);

			//var w1 = new WindowSimple(controller);w1.SetParams(100, 100, 100, 100, "w1");w1.SetHeader(10,10,80,20);view.AddObject(w1);
			//var w2 = new WindowSimple(controller);w2.SetParams(300, 100, 100, 100,"w2");w2.SetHeader(10, 10, 80, 20);view.AddObject(w2);

			// создаём объект визуализации
			_scr = new ViewScreen(controller, _gam);
			_scr.SetParams(0,0,1024,768,"MainScreen");
			_scr.Show();
			view.AddObject(_scr);

			//var background = new Background(controller);
			//view.AddObject(background);

		}

	}
}