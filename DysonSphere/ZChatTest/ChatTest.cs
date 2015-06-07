using Engine;
using Engine.Controllers;
using Engine.Models;
using View = Engine.Views.View;

namespace ZChatTest
{
	public class ChatTest : Module
	{
		private View1 view1;
		private Model1 _model1;

		protected override void SetUpModel(Model model, Controller controller)
		{
			base.SetUpModel(model, controller);
			_model1 = new Model1(controller);
			model.AddObject(_model1);
		}

		protected override void SetUpView(View view, Controller controller)
		{
			base.SetUpView(view, controller);
			//_sys = new ViewControlSystem(Controller);
			//_sys.Show();
			//view.AddObject(_sys);

			view1 = new View1(Controller);
			view1.Show();
			view.AddObject(view1);

		}
	}
}
