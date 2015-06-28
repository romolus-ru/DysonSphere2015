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
		/// Делегат для обмена строками
		/// </summary>
		public delegate void OnPressDelegate(); 
		public OnPressDelegate OnPress;

		public GAButton(Controller controller) : base(controller)
		{}

		public override void Press()
		{
			if (OnPress != null) OnPress();
		}

		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			if (CursorOver)	visualizationProvider.SetColor(Color.DodgerBlue);
			else			visualizationProvider.SetColor(Color.DeepSkyBlue);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			
		}
	}
}
