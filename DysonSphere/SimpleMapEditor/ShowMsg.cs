using System;
using System.Drawing;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;

namespace SimpleMapEditor
{
	class ShowMsg : ViewControl
	{
		private String _msg = "Сообщение";
		private byte _alpha;
		public ShowMsg(Controller controller) : base(controller)
		{
			_alpha = 20;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			visualizationProvider.LoadTexture("ShowMsg", @"..\Resources\msg01.jpg");
			SetCoordinates(0, 0, 0);// занимаем всё пространство
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
		}

		protected override void HandlersAdd()
		{
			base.HandlersAdd();
			Controller.AddEventHandler("ShowMsg", ShowMsgEH);
		}

		protected override void HandlersRemove()
		{
			Controller.RemoveEventHandler("ShowMsg", ShowMsgEH);
			base.HandlersRemove();
		}

		private void ShowMsgEH(object sender, EventArgs e)
		{
			MessageEventArgs m = e as MessageEventArgs;
			if (m == null) return;
			_alpha = 100;
			_msg = m.Message;
		}


		public override bool InRange(int x, int y)
		{
			return false;// не реагируем совсем
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			if (_alpha == 0) { return; }
			_alpha--;

			var mx = visualizationProvider.CanvasWidth / 2;
			var my = visualizationProvider.CanvasHeight / 2;
			visualizationProvider.SetColor(Color.White, _alpha);
			visualizationProvider.DrawTexture(mx, my, "ShowMsg");
			var l = visualizationProvider.TextLength(_msg) / 2;
			visualizationProvider.Print(mx - l, my - 14, _msg);
		}
	}
}
