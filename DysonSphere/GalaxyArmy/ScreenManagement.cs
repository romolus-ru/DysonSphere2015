using Engine;
using Engine.Controllers;
using GalaxyArmy.Model;

namespace GalaxyArmy
{
	/// <summary>
	/// Вывод информации о тренирующихся войсках, возможность увеличить скорость тренировки за процент с прибыли
	/// </summary>
	class ScreenManagement:ScreenBase
	{
		public ScreenManagement(Controller controller, string caption, GalaxyArmyModel gam)
			: base(controller, caption,gam)
		{}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			var tp1 = new TrainProgress1(Controller, Gam, Gam.Armies[0]);
			tp1.SetParams(150, 100, 600, 100, "Тренировка пехоты");
			AddControl(tp1);
		}

	}
}
