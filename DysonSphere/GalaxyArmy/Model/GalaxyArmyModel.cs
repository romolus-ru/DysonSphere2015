using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
		public GeneralFactors GeneralFactors = null;
		
		public List<GalaxyOne> Galaxies = new List<GalaxyOne>();
		public List<ArmyOne> Armies = new List<ArmyOne>();
		public List<Upgrade> Upgrades=new List<Upgrade>();

		// сделать считывание улучшений из архива. достижения тоже.
		// TODO ТУТ!!! покупку армии отменить. вместо нее сделать кнопку "искать наемников" по которой за 100 денег будет искаться группа наемников. цену не менять, но количчество наемников можно будет со временем увеличивать
		// сделать сохранение героической части армии. сохранение megaint и прогресса игры при выходе. кнопку "сброс"
		// клик на тренировочном круге ускоряет тренировку.
		// клики по галактике можно усилить - они должны конкурировать с кликами для ускорения тренировок
		// объединить экраны management и training, удалить кнопку изменения процентности покупки, теперь там будет другая процентность.
		// сделать приемлимой возможность прохождения первой галактики. После этого можно дополнить показателями для остальных галактик
		// может быть стоит сделать так, что бы галактики возвращали ресурсы, которые нужно продавать (кнопка с двигающейся линией, которая определяет наценку)
		// страницы management и trainingcenter может быть стоит объединить - различаются одной кнопкой покупки тренировочного центра. но надо подумать, по идее вся остальная информация совпадает
		// прогресс бар захвата галактик придётся отменить - пока неизвестно как вычислять процент от уже захваченной территории
		// сделать анимацию борьбы в галактике
		// к галактике добавить звёзды
		// на карте появляются объекты - при клике по ним или добавляется денег или добавляется армия
		
		// массив из 100 значений для каждой галактики - они будут показывать сколько процентов галактики захвачено. должно зависеть от максимального количества жителей в галактике
		// каждое следующее значение должно быть значительно больше предыдущего

		public GalaxyArmyModel(Controller controller) : base(controller)
		{
			GeneralFactors = new GeneralFactors();
			GeneralFactors.CurrentMoneyAdd(03, 0);
			GeneralFactors.CurrentMoneyAdd(69, 0);
			Galaxies.Add(new GalaxyOne(EnumUpgradesGroup.Galaxy1));
			Armies.Add(new ArmyOne(EnumUpgradesGroup.Army1));
			InitUpgrades();
			InitUpgradesCosts();// инициировать только при первом запуске - потом инициировать только после сброса завоевания или полного сброса прогресса игры
			RefreshParams();
		}

		/// <summary>
		/// Обновить все параметры. Вызывается при покупке и т.п. в том числе при покупке улучшений
		/// </summary>
		public void RefreshParams()
		{
			foreach (var galaxy in Galaxies) { galaxy.RecalcValues(GeneralFactors); }
			foreach (var army in Armies) { army.RecalcValues(GeneralFactors); }
		}

		/// <summary>
		/// Начальная инициализация доступных улучшений
		/// </summary>
		private void InitUpgrades()
		{
			// улучшения будут сортироваться по обычной cost, поэтому у цен с кристалами нужно поставить обычную адекватную цену
			// желательно рассчитать так, что бы можно было достичь полного завоевания первой галактики, открыв не все улучшения для первого типа войск
			// а уже потом, с помощью второй галактики открыть дополнительные

			AddUpgrade(1,1, "UArmy1Attack", 1, 0, "Улучшение атаки 1 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyAttack, EnumUpgradesGroup.Army1,1);
			AddUpgrade(1,2, "UArmy1Attack", 1, 0, "Улучшение атаки 2 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyAttack, EnumUpgradesGroup.Army1,1);
			AddUpgrade(1,3, "UArmy1Attack", 2, 0, "Улучшение атаки 3 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyAttack, EnumUpgradesGroup.Army1,1);
			AddUpgrade(1,4, "UArmy1Attack", 4, 0, "Улучшение атаки 4 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyAttack, EnumUpgradesGroup.Army1,2);

			AddUpgrade(2,1, "UArmy1Defance", 1, 0, "Улучшение защиты 1 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyDefance, EnumUpgradesGroup.Army1,1);
			AddUpgrade(2,2, "UArmy1Defance", 1, 0, "Улучшение защиты 2 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyDefance, EnumUpgradesGroup.Army1,1);
			AddUpgrade(2,3, "UArmy1Defance", 2, 0, "Улучшение защиты 3 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyDefance, EnumUpgradesGroup.Army1,1);
			AddUpgrade(2,4, "UArmy1Defance", 4, 0, "Улучшение защиты 4 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyDefance, EnumUpgradesGroup.Army1,2);

			AddUpgrade(3,1, "UArmy1Vitality", 1, 0, "Улучшение здоровья 1 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyVitality, EnumUpgradesGroup.Army1,1);
			AddUpgrade(3,2, "UArmy1Vitality", 1, 0, "Улучшение здоровья 2 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyVitality, EnumUpgradesGroup.Army1,1);
			AddUpgrade(3,3, "UArmy1Vitality", 2, 0, "Улучшение здоровья 3 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyVitality, EnumUpgradesGroup.Army1,2);
			AddUpgrade(3,4, "UArmy1Vitality", 4, 0, "Улучшение здоровья 4 для армии 1", "Купить", "Улучшение для армии", EnumUpgradesTypeIcon.ArmyVitality, EnumUpgradesGroup.Army1,2);

			AddUpgrade(4,1, "Galaxy1ClickCostMultiplier", 1, 0, "Клик x2 для Галактики 1", "Купить", "Улучшение для доходности галактики", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);
			AddUpgrade(4,2, "Galaxy1ClickCostMultiplier", 1, 0, "Клик x3 для Галактики 1", "Купить", "Улучшение для доходности галактики", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);
			AddUpgrade(4,3, "Galaxy1ClickCostMultiplier", 1, 0, "Клик x4 для Галактики 1", "Купить", "Улучшение для доходности галактики", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);
			AddUpgrade(4,4, "Galaxy1ClickCostMultiplier", 1, 0, "Клик x5 для Галактики 1", "Купить", "Улучшение для доходности галактики", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);
			AddUpgrade(4,5, "Galaxy1ClickCostMultiplier", 3, 0, "Клик x8 для Галактики 1", "Купить", "Улучшение для доходности галактики", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);
			AddUpgrade(4,6, "Galaxy1ClickCostMultiplier", 8, 0, "Клик x16 для Галактики 1", "Купить", "Улучшение для доходности галактики", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);

			AddUpgrade(5,1, "UArmy1SoldierCost", -5, 0, "Уменьшение цены найма солдат 1 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);
			AddUpgrade(5,2, "UArmy1SoldierCost", -5, 0, "Уменьшение цены найма солдат 2 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);
			AddUpgrade(5,3, "UArmy1SoldierCost", -5, 0, "Уменьшение цены найма солдат 3 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);
			AddUpgrade(5,4, "UArmy1SoldierCost", -5, 0, "Уменьшение цены найма солдат 4 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);

			AddUpgrade(6,1, "UArmy1TimeDelayMultiplier", 02, 0, "Ускорение тренировки 1 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Army1,1);
			AddUpgrade(6,2, "UArmy1TimeDelayMultiplier", 04, 0, "Ускорение тренировки 2 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Army1,1);
			AddUpgrade(6,3, "UArmy1TimeDelayMultiplier", 08, 0, "Ускорение тренировки 3 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Army1,2);
			AddUpgrade(6,4, "UArmy1TimeDelayMultiplier", 16, 0, "Ускорение тренировки 4 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Army1,2);
			
			AddUpgrade(7,1, "UArmy1TrainingAdditionalPlace", 1, 0, "Увеличение количества тренируемых 1 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Army1,1);
			AddUpgrade(7,2, "UArmy1TrainingAdditionalPlace", 1, 0, "Увеличение количества тренируемых 2 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Army1,1);
			AddUpgrade(7,3, "UArmy1TrainingAdditionalPlace", 1, 0, "Увеличение количества тренируемых 4 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Army1,1);
			AddUpgrade(7,4, "UArmy1TrainingAdditionalPlace", 1, 0, "Увеличение количества тренируемых 3 для армии 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Army1,1);

			AddUpgrade(8,1, "Instructor1Upgrade", 1, 0, "Улучшение 1 для инструктора 1", "Купить", "Влияет на количество и качество обучаемых", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,1);
			AddUpgrade(8,2, "Instructor1Upgrade", 1, 0, "Улучшение 2 для инструктора 1", "Купить", "Влияет на количество и качество обучаемых", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,1);
			AddUpgrade(8,3, "Instructor1Upgrade", 1, 0, "Улучшение 3 для инструктора 1", "Купить", "Влияет на количество и качество обучаемых", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,2);
			AddUpgrade(8,4, "Instructor1Upgrade", 1, 0, "Улучшение 4 для инструктора 1", "Купить", "Влияет на количество и качество обучаемых", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,2);
			AddUpgrade(8,5, "Instructor1Upgrade", 1, 0, "Улучшение 5 для инструктора 1", "Купить", "Влияет на количество и качество обучаемых", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,2);
			AddUpgrade(8,6, "Instructor1Upgrade", 1, 0, "Улучшение 6 для инструктора 1", "Купить", "Влияет на количество и качество обучаемых", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,3);
			AddUpgrade(8,7, "Instructor1Upgrade", 1, 0, "Улучшение 7 для инструктора 1", "Купить", "Влияет на количество и качество обучаемых", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,3);
			AddUpgrade(8,8, "Instructor1Upgrade", 1, 0, "Улучшение 8 для инструктора 1", "Купить", "Влияет на количество и качество обучаемых", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,3);

			AddUpgrade(9,1, "Instructor1SoldiersTraining", 1, 0, "Увеличение обучаемых 1 для инструктора 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,1);
			AddUpgrade(9,2, "Instructor1SoldiersTraining", 1, 0, "Увеличение обучаемых 2 для инструктора 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,1);
			AddUpgrade(9,3, "Instructor1SoldiersTraining", 1, 0, "Увеличение обучаемых 3 для инструктора 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,1);
			AddUpgrade(9,4, "Instructor1SoldiersTraining", 1, 0, "Увеличение обучаемых 4 для инструктора 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Instructor1,1);

			AddUpgrade(10,1, "Galaxy1TimeDelayMultiplier", 02, 0, "Увеличение скорости сбора 1", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);
			AddUpgrade(10,2, "Galaxy1TimeDelayMultiplier", 02, 0, "Увеличение скорости сбора 2", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);
			AddUpgrade(10,3, "Galaxy1TimeDelayMultiplier", 04, 0, "Увеличение скорости сбора 3", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,1);
			AddUpgrade(10,4, "Galaxy1TimeDelayMultiplier", 04, 0, "Увеличение скорости сбора 4", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,2);
			AddUpgrade(10,5, "Galaxy1TimeDelayMultiplier", 16, 0, "Увеличение скорости сбора 5", "Купить", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy1,3);

			AddUpgrade(11,1, "Galaxy2Open", 16, 01, "Открытие галактики 2", "Открыть", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy2,2);
			AddUpgrade(11,2, "Galaxy3Open", 16, 05, "Открытие галактики 3", "Открыть", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy3,3);
			AddUpgrade(11,3, "Galaxy4Open", 16, 15, "Открытие галактики 4", "Открыть", "", EnumUpgradesTypeIcon.None, EnumUpgradesGroup.Galaxy4,4);

		}

		private void InitUpgradesCosts()
		{
			var maxWave = 1;// ищем максимальную волну
			foreach (var upgrade in Upgrades){if (upgrade.NumWave > maxWave) maxWave = upgrade.NumWave;}
			var bCost = new MegaInt(0, 5);//начальное значение улучшения
			for (int wave = 1; wave <= maxWave; wave++){
				//bCost.MulValue(wave);
				// получаем список апгрейдов для данной волны
				var upgrades = Upgrades.Where(upgrade => upgrade.NumWave == wave).ToList();
				// перемешиваем
				Random rnd = new Random();
				upgrades = upgrades.OrderBy(x => rnd.Next()).ToList();
				// последовательно меняем стоимость
				foreach (var upgrade in upgrades){
					upgrade.Cost.AddValue(bCost);
					bCost.MulValue(4*wave);
				}
				var m = bCost.GetMaxPowerReal();// немного перекрываются, но это нормально
				bCost = new MegaInt(m, 1);
			}
			// дополнительные шаги для сортировки - апгрейды расположены не по порядку (не по num2, по которому их надо отсортировать)
			var maxNum1 = 1;
			foreach (var upgrade in Upgrades) { if (upgrade.Num1 > maxNum1) maxNum1 = upgrade.Num1; }
			for (int num1 = 1; num1 <= maxNum1; num1++){// для всех номеров улучшений
				var upgradesNum1 = Upgrades.Where(upgrade => upgrade.Num1 == num1).ToList();
				var count = upgradesNum1.Count;
				if (count < 2) continue;
				upgradesNum1.Sort((u1, u2) => u1.Num2.CompareTo(u2.Num2));// сортируем, подготавливая список к обмену стоимостей
				for (int i = 0; i < count-1; i++){
					var v1 = upgradesNum1[i];
					Upgrade v2 = null;
					// находим элемент с меньшей ценой
					for (int j = i+1; j < count; j++){
						var v2A = upgradesNum1[j];// ищем минимальное значение
						if (v1.Cost.IsBiggerThen(v2A.Cost)) v2 = v2A;
					}
					if (v2==null)continue;
					// меняем местами цены
					var b = v1.Cost;
					v1.CostSet(v2.Cost);
					v2.CostSet(b);
				}
			}
		}

		private void AddUpgrade(int num1, int num2, string fieldName, int value, int costCrystals, string description, string btnText, string hint, EnumUpgradesTypeIcon eType, EnumUpgradesGroup eGroup, int wave)
		{
			// можно в самом улучшении прописать на сколько будет увеличиваться показатель и избавиться от многих вспомогательных вспомогательных переменных
			var u = new Upgrade(num1, num2, GeneralFactors, fieldName, value, new MegaInt(0, 1), costCrystals, description, btnText, hint, eType, eGroup,wave);
			Upgrades.Add(u);
		}

		public string CurrentMoney()
		{
			return GeneralFactors.CurrentMoneyGet().GetAsFullLineString();
		}

		public override void Execute()
		{
			//_currentMoney.AddValue(0, 1);
			var added = false;
			foreach (var galaxy in Galaxies){
				var a = galaxy.CanGetMoney();
				if (a){
					GeneralFactors.CurrentMoneyAdd(galaxy.IncomeMoney);
					added = true;
				}
			}
			foreach (var army in Armies){army.TrainArmy();}
			if (added) foreach (var army in Armies) { army.RefreshTotalBuy(GeneralFactors.CurrentMoneyGet()); }
		}

		public void ClickOnGalaxy(GalaxyOne galaxy)
		{
			GeneralFactors.CurrentMoneyAdd(galaxy.ClickCost);
		}

		/// <summary>
		/// Можно ли нанять нескольких солдат (хватает ли денег)
		/// </summary>
		/// <param name="army">Ссылка на армию</param>
		public MegaInt RecruitArmyCost(ArmyOne army)
		{
			var count = MegaInt.Create(0, 5);
			count.MulValue(army.SoldierCost);
			return count;
		}

		/// <summary>
		/// Нанять нескольких солдат
		/// </summary>
		/// <param name="army">Ссылка на армию</param>
		public void RecruitArmy(ArmyOne army)
		{
			var countSoldiers = MegaInt.Create(0, 5);
			var cost=RecruitArmyCost(army);
			if (cost.IsBiggerThen(GeneralFactors.CurrentMoneyGet())) return;
			GeneralFactors.CurrentMoneyMinus(cost);// покупаем солдат
			army.ReadyArmy.AddValue(countSoldiers);// получаем солдат в готовую армию
			RefreshParams();
		}

		public void BuyTrainingCenter(ArmyOne army)
		{
			var trainingBaseCost = army.TrainingBaseCost;
			if (trainingBaseCost.IsBiggerThen(GeneralFactors.CurrentMoneyGet())) return;
			GeneralFactors.CurrentMoneyMinus(trainingBaseCost);// покупаем
			army.TrainingBaseCount++;
			army.TimeDelayCurrent = 0;// сбрасываем цикл, начинаем его с начала
			RefreshParams();
		}

		/// <summary>
		/// Сколько пожно послать армии
		/// </summary>
		/// <param name="galaxyNum">Номер галактики, по номеру можно установить сколько войск можно послать</param>
		/// <param name="armyNum">Номер передаваемой армии. Накладкее, но зато одна функция отвечает за получение количества отправляемой армии</param>
		/// <returns></returns>
		public MegaInt SendArmyAvailableCount(EnumUpgradesGroup galaxyNum,EnumUpgradesGroup armyNum=EnumUpgradesGroup.None)
		{
			if (!SendArmyAvailable(galaxyNum)) return new MegaInt();
			// проверяем общее количество готовых войск
			var ar1 = Armies[0].ReadyArmy.CopyThis();
			return ar1;
		}
	
		/// <summary>
		/// Проверка, можно ли послать армию
		/// </summary>
		/// <param name="galaxyNum">Номер галактики, по номеру можно установить сколько войск можно послать</param>
		/// <returns></returns>
		public Boolean SendArmyAvailable(EnumUpgradesGroup galaxyNum)
		{
			// пока делаем для первой галактики
			var ag0 = new MegaInt(0, 1);
			var ar1 = Armies[0].ReadyArmy;
			var b = !(ag0.IsBiggerThen(ar1));
			return b;
		}

		/// <summary>
		/// Отправляем армию в галактику
		/// </summary>
		/// <param name="galaxyNum"></param>
		public void SendArmy(EnumUpgradesGroup galaxyNum)
		{
			if (SendArmyAvailable(galaxyNum)){
				var a = SendArmyAvailableCount(galaxyNum);
				Armies[0].ReadyArmy.MinusValue(a);// обнуляем
				Galaxies[0].Army.AddValue(a);// прибавляем
				Galaxies[0].RecalcValues(GeneralFactors);
			}
		}

		/// <summary>
		/// Сохранить слои в архив
		/// </summary>
		/// <param name="fileName"></param>
		public void Save(string fileName)
		{
			var a = new FileArchieve(fileName);
			var ms = new MemoryStream();
			var enc = Encoding.UTF8;
			foreach (var u1 in Upgrades){
				var b = enc.GetBytes(u1.Description + Environment.NewLine);
				ms.Write(b, 0, b.Length);
			}
			a.AddStream("u1", ms);
			a.Dispose();
		}

		// TODO !! тут
		// сохранение улучшений (и загрузка)
		// сохранение кристалов, количества сбросов и количества захватов галактик

	}
}
