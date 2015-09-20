using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils;

namespace GalaxyArmy
{
	/// <summary>
	/// Общие параметры. хранятся все текущие параметры, показатели, количество денег и т.п.
	/// </summary>
	/// <remarks>
	/// в основном с помощью DataBindingSimple отсюда забираются многие параметры
	/// объект хранится в GalaxyArmyModel и обеспечивает централизованное хранение параметров
	/// </remarks>
	class GeneralFactors
	{
		private MegaInt _currentMoney=new MegaInt();

		public MegaInt CurrentMoneyGet(){return _currentMoney.CopyThis();}

		public void CurrentMoneyAdd(int power, int value)
		{
			_currentMoney.AddValue(power, value);
		}

		public void CurrentMoneyAdd(MegaInt value)
		{
			_currentMoney.AddValue(value);
		}
		public void CurrentMoneyMinus(MegaInt value)
		{
			_currentMoney.MinusValue(value);
		}

		public int CurrentCrystals { get; private set; }

		public void CurrentCrystalsAdd(int value){CurrentCrystals += value;}
		public void CurrentCrystalsMinus(int value) { CurrentCrystals -= value; }

		/// <summary>
		/// Открыты ли кристаллы (открываются не улучшением, а при захвате первой галактики и после нажатия кнопки "перезахват")
		/// </summary>
		public int CrystalsOpen=0;

		#region UArmy1

		public int UArmy1 { get { return 1+UArmy1Attack + UArmy1Defance + UArmy1Vitality; } }

		public int UArmy1Attack = 0;

		public int UArmy1Defance = 0;

		public int UArmy1Vitality = 0;

		public int UArmy1SoldierCost = 0;

		public int UArmy1TimeDelayMultiplier = 0;

		public int UArmy1TrainingAdditionalPlace = 0;

		#endregion

		#region Instructor1

		public int Instructor1Upgrade = 0;

		public int Instructor1SoldiersTraining = 1;

		#endregion

		public int Galaxy1Open = 1;
		public int Galaxy2Open = 0;
		public int Galaxy3Open = 0;
		public int Galaxy4Open = 0;
		public MegaInt Galaxy1StartPopulation = new MegaInt(15, 1);
		public MegaInt Galaxy2StartPopulation = new MegaInt(24, 1);
		public MegaInt Galaxy3StartPopulation = new MegaInt(33, 1);
		public MegaInt Galaxy4StartPopulation = new MegaInt(42, 1);

		public int Galaxy1ConquerorCount = 0;
		public int Galaxy2ConquerorCount = 0;
		public int Galaxy3ConquerorCount = 0;
		public int Galaxy4ConquerorCount = 0;

		public int Galaxy1ClickCostMultiplier = 1;

		public int Galaxy1TimeDelayMultiplier = 0;

	}
}
