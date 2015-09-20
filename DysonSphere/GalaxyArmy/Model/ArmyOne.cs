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
		/// Текущее количество тренировочных баз
		/// </summary>
		public int TrainingBaseCount;

		/// <summary>
		/// Цена покупки тренировочного центра
		/// </summary>
		public MegaInt TrainingBaseCost;

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

		/// <summary>
		/// Имя армии - пехота, авиация и т.п.
		/// </summary>
		public string ArmyName;

		public ArmyOne(EnumUpgradesGroup armyType)
		{
			ArmyName = "Неизвестный тип армии";
			if (armyType == EnumUpgradesGroup.Army1) { ArmyName = "Пехота"; }
			if (armyType == EnumUpgradesGroup.Army2) { ArmyName = "Танки"; }
			if (armyType == EnumUpgradesGroup.Army3) { ArmyName = "Авиация"; }
			if (armyType == EnumUpgradesGroup.Army4) { ArmyName = "Десант"; }
			TimeDelay = 150;
			TimeDelayCurrent = 0;
			TrainingArmy = new MegaInt();
			ReadyArmy = new MegaInt();
			BuyMaxArmy=new MegaInt();
			TrainingBaseCount = 0;
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

		public void RecalcValues(GeneralFactors factors)
		{
			int t = 240;// начальная пауза для тренировки солдат
			var instructorMultiplier = factors.Instructor1SoldiersTraining;
			var additionalPlace = factors.UArmy1TrainingAdditionalPlace;

			// или придётся отказаться от покупки солдат или значительно снизить эффективность - покупка может сделать игру слишком легкой
			// может быть увеличивать цену покупки если уже есть готовая ждущая армия.
			// ** количество солдат которых можно купить
			RefreshTotalBuy(factors.CurrentMoneyGet());

			// ** Вычисление стоимости центра
			TrainingBaseCost = MegaInt.Function1(4, TrainingBaseCount + 1);

			// ** стомость покупки
			SoldierCost = 100 - factors.UArmy1SoldierCost;

			// ** скорость 
			TimeDelay = t;
			var td = factors.Galaxy1TimeDelayMultiplier;
			if (td > 0) TimeDelay /= td;

			// ** количество обучающихся
			TrainingArmy=new MegaInt();
			if (TrainingBaseCount > 0){
				var a = new MegaInt(0, TrainingBaseCount);
				a.AddValue(0, additionalPlace);
				a.MulValue(instructorMultiplier);
				TrainingArmy.AddValue(a);
			}
		}

		/// <summary>
		/// Обновить информацию о том, сколько можно купить солдат
		/// </summary>
		public void RefreshTotalBuy(MegaInt currMoney)
		{
			var cm = currMoney.CopyThis();
			cm.DivValue(SoldierCost);
			BuyMaxArmy = cm;
		}

	}
}
