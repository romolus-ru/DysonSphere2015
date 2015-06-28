using System.Collections.Generic;
using System.Drawing;
using Engine.Controllers;
using Engine.Models;
using Engine.Utils;

namespace GalaxyArmy.Model
{
	/// <summary>
	/// Модель игры
	/// </summary>
	class GalaxyArmyModel:ModelObject
	{
		private MegaInt _currentMoney = new MegaInt();
		
		public List<GalaxyOne> Galaxies = new List<GalaxyOne>();
		public List<ArmyOne> Armies = new List<ArmyOne>();

		public GalaxyArmyModel(Controller controller) : base(controller)
		{
			_currentMoney.AddValue(0, 5);
			_currentMoney.AddValue(69, 0);
			//controller.AddEventHandler("GAM1Sec",GAM1SecEH);
			Galaxies.Add(new GalaxyOne());
			Armies.Add(new ArmyOne());
		}

		public string CurrentMoney()
		{
			return _currentMoney.GetAsFullLineString();
		}

		public override void Execute()
		{
			//_currentMoney.AddValue(0, 1);
			var added = false;
			foreach (var galaxy in Galaxies){
				var a = galaxy.CanGetMoney();
				if (a){
					_currentMoney.AddValue(galaxy.IncomeMoney);
					added = true;
				}
			}
			foreach (var army in Armies){army.TrainArmy();}
			if (added) foreach (var army in Armies) { army.RefreshTotalBuy(_currentMoney); }
		}

		public void ClickOnGalaxy(GalaxyOne galaxy)
		{
			_currentMoney.AddValue(galaxy.ClickCost);
		}

		/// <summary>
		/// Купить армию
		/// </summary>
		/// <param name="army">Ссылка на армию</param>
		/// <param name="value">Сколько. 1=1, 25=25% 50=50% 100=max </param>
		public void BuyArmy(ArmyOne army, int value)
		{
			var count = army.BuyMaxArmy.CopyThis();// 100%
			if (value == 2) count.DivValue(2);// 50%
			if (value==1)count.DivValue(4);// 25%
			if (value == 0){
				count = new MegaInt();
				count.AddValue(0, 1);
			}
			// TODO ТУТ сначала надо добавить солдат, потом отнять денег
			//сделать 2 дополнительных метода - _RectangleR и _BoxR? которые будут рисовать скруглённые фигуры
			count.MulValue(army.SoldierCost);
			_currentMoney.MinusValue(count);
		}

	}
}
