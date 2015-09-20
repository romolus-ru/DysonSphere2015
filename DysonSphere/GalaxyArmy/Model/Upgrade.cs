using System;
using Engine.Controllers;
using Engine.Utils;

namespace GalaxyArmy.Model
{
	/// <summary>
	/// Абстрактный апгрейд. должен настраиваться на объект, с которым будет взаимодействовать
	/// </summary>
	class Upgrade
	{

		/// <summary>
		/// Состояние улучшения. 0 неактивно из-за очередности или ещё как, 1 активно, 2 куплено
		/// </summary>
		/// <remarks>Сохраняется и загружается отдельно</remarks>
		public int State;
		public int Num1 { get; private set; }
		public int Num2 { get; private set; }
		private DataBindingSimple _binding;
		private GeneralFactors _generalFactors;
		public MegaInt Cost { get; private set; }
		public int CostCrystals { get; private set; }
		public string Description { get; private set; }
		public string BtnText { get; private set; }
		public string Hint { get; private set; }
		public EnumUpgradesTypeIcon EType;
		public EnumUpgradesGroup EGroup;
		public int Value { get; private set; }
		public int NumWave { get; private set; }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="num1">Номер улучшения для сохранения</param>
		/// <param name="num2">Дополнительный номер для сохранения</param>
		/// <param name="generalFactors">
		/// Текущие настройки модели, в том числе количество денег и т.п.
		/// Объект у которого будет меняться свойство (int)</param>
		/// <param name="fieldName">Имя поля, которое будем менять</param>
		/// <param name="value">Значение на которое будет меняться поле</param>
		/// <param name="cost">Цена улучшения в MegaInt</param>
		/// <param name="costCrystals">Цена улучшения в кристаллах</param>
		/// <param name="description">Описание основное</param>
		/// <param name="btnText">текст на кнопке</param>
		/// <param name="hint">появляющаяся подсказка</param>
		/// <param name="eType">Тип улучшения (первая иконка)</param>
		/// <param name="eGroup">Тип группы (вторая иконка)</param>
		/// <param name="numWave">Номер волны, </param>
		public Upgrade(int num1, int num2,GeneralFactors generalFactors, string fieldName, int value, MegaInt cost, int costCrystals, string description, string btnText, string hint
			,EnumUpgradesTypeIcon eType, EnumUpgradesGroup eGroup, int numWave)
		{
			Num1 = num1;
			Num2 = num2;
			NumWave = numWave;
			Value = value;
			_generalFactors = generalFactors;
			Cost = cost;
			CostCrystals = costCrystals;
			Description = description;
			BtnText = btnText;
			Hint = hint;
			EType = eType;
			EGroup = eGroup;
			_binding=new DataBindingSimple(generalFactors,fieldName);
		}

		private void SetValue()
		{
			var a=_binding.GetInt();
			a += Value;
			_binding.SetInt(a);
		}

		/// <summary>
		/// Проверяем, можно ли купить улучшение
		/// </summary>
		/// <returns></returns>
		public Boolean CanBuy()
		{
			var canBuy1 = false;// можем ли  купить за обычные деньги
			if (Cost == null) canBuy1 = true;
			else if (_generalFactors.CurrentMoneyGet().IsBiggerThen(Cost)) canBuy1 = true;
			if (!canBuy1) return false;
			if (CostCrystals > _generalFactors.CurrentCrystals) return false;
			return true;
		}


		public void BuyUpgrade()
		{
			if (!CanBuy()) return;
			// отнимаем деньги и кристалы
			_generalFactors.CurrentMoneyMinus(Cost);
			_generalFactors.CurrentCrystalsMinus(CostCrystals);
			// увеличиваем контролируемый параметр
			SetValue();
			State = 2; // куплено
		}

		/// <summary>
		/// При загрузке улучшений устанавливаем им значение
		/// </summary>
		public void SetState(int state)
		{
			State = state;
			if (State==2)SetValue();
		}

		/// <summary>
		/// Поменять цену. используется при сортировке
		/// </summary>
		/// <param name="newCost"></param>
		public void CostSet(MegaInt newCost)
		{
			Cost = newCost;
		}
	}
}
