using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Controllers;
using Engine.Views.Templates;

namespace GalaxyArmy
{
	/// <summary>
	/// Кнопка с делегатной обратной связью, что бы не плодить кучу событий в контроллере
	/// </summary>
	class GAButton:MenuButton
	{
		/// <summary>
		/// Делегат
		/// </summary>
		public delegate void OnPressDelegate(); 
		public OnPressDelegate OnPress;
		
		/// <summary>
		/// Состояние кнопки когда нажатие на нее бесполезно, не принесет результата
		/// </summary>
		public Boolean CantPress;

		public GAButton(Controller controller) : base(controller)
		{}

		public override void Press()
		{
			if (OnPress != null) OnPress();
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			var color = Color.DeepSkyBlue;
			if (CantPress) color = Color.White;
			if (CursorOver) color = Color.DodgerBlue;
			visualizationProvider.SetColor(color, 40);
			visualizationProvider.Box(X, Y, Width - 3, Height - 3);
			if (CantPress) color = Color.DarkRed;
			visualizationProvider.SetColor(color);
			visualizationProvider.Rectangle(X, Y, Width, Height);
		}
	}
}
