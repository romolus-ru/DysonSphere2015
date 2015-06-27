using System;
using System.Drawing;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Views;
using Button = Engine.Views.Templates.Button;

namespace SimpleMapEditor
{
	class ViewModalSelectFile : ViewModal// Модальное
	{
		public ViewModalSelectFile(Controller controller, string outEvent) : base(controller,outEvent)
		{

		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			SetCoordinates(200, 200);
			SetSize(500, 100);
			EscButton = Button.CreateButton(Controller, 280, 10, 100, 20, OutEvent, "Закрыть", "Закрыть", Keys.None, "");
			AddControl(EscButton);
			for (int i = 1; i < 10; i++){
				var btn = Button.CreateButton(Controller, 10 + i * 42, 40, 40, 40, "vmSelectFile", "" + i, "" + i, Keys.None, "" + i);
				AddControl(btn);
			}
		}

		/// <summary>
		/// Кнопка закрытия модального окна
		/// </summary>
		protected Button EscButton;


		protected override void HandlersAdd()
		{
			base.HandlersAdd();
			Controller.AddEventHandler("vmSelectFile", ModalPressed);
		}

		protected override void HandlersRemove()
		{
			Controller.RemoveEventHandler("vmSelectFile", ModalPressed);
			base.HandlersRemove();
		}

		private void ModalPressed(object sender, EventArgs e)
		{
			var s = sender as ViewControl;
			if (s != null){
				_name = s.Name;
				ModalResult = Convert.ToInt32(_name);
				EscButton.Press();
			}
		}

		private String _name = "";

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			if (CursorOver)
				visualizationProvider.SetColor(Color.FromArgb(50, Color.Chartreuse));
			else visualizationProvider.SetColor(Color.FromArgb(50, Color.Aquamarine));
			visualizationProvider.Box(X, Y, Width, Height);
			base.DrawObject(visualizationProvider);
			visualizationProvider.SetColor(Color.Aquamarine);
			visualizationProvider.Print(X + 10, Y + 10, "Для выхода из режима нажмите 8 " + _name);
			var c = Controller.ToString();
		}
	}
}
