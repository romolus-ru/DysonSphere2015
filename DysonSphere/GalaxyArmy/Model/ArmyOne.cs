using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;
using Engine.Utils;

namespace GalaxyArmy.Model
{
	/// <summary>
	/// Основа для работы с армиями
	/// </summary>
	class ArmyOne
	{
		/// <summary>
		/// Количество тренируемой армии
		/// </summary>
		public MegaInt TrainingArmy;

		/// <summary>
		/// Количество солдат которых можно купить с имеющимися деньгами
		/// </summary>
		public MegaInt BuyMaxArmy;

		/// <summary>
		/// Цена одного солдата
		/// </summary>
		public int SoldierCost;

		/// <summary>
		/// Время тренировки армии
		/// </summary>
		public int TimeDelay = 100;
		
		/// <summary>
		/// Текущее время тренировки, прошедшее количество тактов со времени последней выдачи
		/// </summary>
		public int TimeDelayCurrent = 0;
		
		/// <summary>
		/// Армия, готовая к отправке
		/// </summary>
		public MegaInt ReadyArmy;

		public ArmyOne()
		{
			TimeDelay = 150;
			TimeDelayCurrent = 0;
			TrainingArmy = new MegaInt();
			TrainingArmy.AddValue(0, 1);
			ReadyArmy = new MegaInt();
			BuyMaxArmy=new MegaInt();
			SoldierCost=100;
		}

		/// <summary>
		/// Проверяем, натренировались ли войска
		/// </summary>
		/// <returns></returns>
		public void TrainArmy()
		{
			TimeDelayCurrent++;
			if (TimeDelayCurrent > TimeDelay){
				TimeDelayCurrent = 0;
				ReadyArmy.AddValue(TrainingArmy);
			}
		}

		/// <summary>
		/// Обновить информацию о том, сколько можно купить солдат
		/// </summary>
		public void RefreshTotalBuy(MegaInt currMoney)
		{
			// надо рассчитать сколько солдат можно купить
			// нужно разделить имеющееcя число на стоимость солдата
			var cm = currMoney.CopyThis();
			cm.DivValue(SoldierCost);
			BuyMaxArmy = cm;
		}
	}
}
