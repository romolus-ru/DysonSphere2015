using Engine;
using Engine.Controllers;
using GalaxyArmy.Model;

namespace GalaxyArmy
{
	/// <summary>
	/// Покупка дополнительных тренировочных баз
	/// </summary>
	class ScreenTraining:ScreenBase
	{
		public ScreenTraining(Controller controller, string caption, GalaxyArmyModel gam)
			: base(controller, caption,gam)
		{}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			var tp1 = new TrainingProgressBuy(Controller, Gam, Gam.Armies[0]);
			tp1.SetParams(150, 100, 600, 100, "Тренировка пехоты");
			AddControl(tp1);
		}

	}
}