using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views.Templates;

namespace Engine.Views.Debug
{
	/// <summary>
	/// Отладочное окно для вывода порядка контролов
	/// </summary>
	public class WindowControlsView:WindowSimple
	{
		private View _view;
		public WindowControlsView(Controller controller, View view) : base(controller)
		{_view = view;}

		private List<string> _values = new List<string>();
		private int i = 0;

		private void GetValues()
		{
			_values = _view.GetObjectsView();
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			// TODO !!!!! сделать что бы рамка появлялась только при наведении на заголовок
			if (CursorOver) visualizationProvider.SetColor(Color.MediumSeaGreen, 25);
			else visualizationProvider.SetColor(Color.MediumSeaGreen, 20);
			visualizationProvider.Box(X, Y, Width, Height);
			visualizationProvider.SetColor(Color.Red,50);
			visualizationProvider.Box(HeaderX, HeaderY, HeaderWidth, HeaderHeight);
		}

		public override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			i++;
			if (i > 10){
				i = 0;
				GetValues();
			}

			vp.SetColor(Color.White);
			var num = 1;
			foreach (var value in _values){
				vp.Print(X+5, Y+num*15, value);
				num++;
			}
		}
	}
}
