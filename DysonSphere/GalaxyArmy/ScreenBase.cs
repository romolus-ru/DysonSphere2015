using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Controllers;
using Engine.Views;
using GalaxyArmy.Model;

namespace GalaxyArmy
{
	/// <summary>
	/// Заготовка для экрана
	/// </summary>
	/// <remarks>Вспомогательные возможности - хранение точек для круговых прогрессбаров</remarks>
	class ScreenBase:ViewControl
	{
		private string _caption;
		protected GalaxyArmyModel Gam;

		public ScreenBase(Controller controller,string caption, GalaxyArmyModel gam) : base(controller)
		{
			_caption = caption;
			Gam = gam;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			Width = visualizationProvider.CanvasWidth;
			Height = visualizationProvider.CanvasHeight - Y;
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Rectangle(X, Y, Width, Height);
			visualizationProvider.SetColor(Color.WhiteSmoke);
			visualizationProvider.Rectangle(X + 10, Y + 10, Width - 20, Height - 20);
			visualizationProvider.SetColor(Color.White);
			visualizationProvider.Print(X + 15, Y + 15, _caption);
		}

		public override bool InRange(int x, int y)
		{
			return base.InRange(x, y);
		}
	}
}
