using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils;

namespace GalaxyArmy.Model
{
	/// <summary>
	/// Основа для вывода информации о завоеванных галактиках
	/// </summary>
	class GalaxyOne
	{
		/// <summary>
		/// Номер галактики
		/// </summary>
		public EnumUpgradesGroup Group{get; protected set; }

		/// <summary>
		/// Количество собираемых денег
		/// </summary>
		public MegaInt IncomeMoney;

		/// <summary>
		/// Цена одного клика
		/// </summary>
		public MegaInt ClickCost;

		/// <summary>
		/// Задержка перед выдачей собранных денег
		/// </summary>
		public int TimeDelay = 100;
		
		/// <summary>
		/// Текущее время задержки, прошедшее количество тактов со времени последней выдачи
		/// </summary>
		public int TimeDelayCurrent = 0;
		
		/// <summary>
		/// Население
		/// </summary>
		private MegaInt _population;

		/// <summary>
		/// Армия
		/// </summary>
		public MegaInt Army;

		/// <summary>
		/// Герои армии, приносят дополнительный доход и остаются после рестарта
		/// </summary>
		public MegaInt ArmyHeroes;

		/// <summary>
		/// Захвачено
		/// </summary>
		private Boolean _captured;

		public GalaxyOne(EnumUpgradesGroup group)
		{
			Group = group;
			TimeDelay = 150;
			TimeDelayCurrent = 0;
			Army = new MegaInt();
			IncomeMoney=new MegaInt();
			IncomeMoney.AddValue(0, 1);
			ClickCost=new MegaInt();
			ClickCost.AddValue(0, 100);
			_captured = false;
		}

		/// <summary>
		/// Проверяем, пришло ли время для получения прибыли
		/// </summary>
		/// <returns></returns>
		public Boolean CanGetMoney()
		{
			var ret = false;
			TimeDelayCurrent++;
			if (TimeDelayCurrent > TimeDelay){
				TimeDelayCurrent = 0;
				ret = true;
			}
			return ret;
		}

		/// <summary>
		/// Перевычислить показатели, в частности IncomeMoney
		/// </summary>
		public void RecalcValues(GeneralFactors factors)
		{
			// ** основные параметры для рассчетов
			MegaInt p = factors.Galaxy1StartPopulation;
			int c = factors.Galaxy1ConquerorCount;
			int m = factors.UArmy1;
			int t = 120;// начальная пауза для передачи денег
			if (Group == EnumUpgradesGroup.Galaxy2) {p = factors.Galaxy2StartPopulation;c = factors.Galaxy2ConquerorCount;}
			if (Group == EnumUpgradesGroup.Galaxy3) {p = factors.Galaxy3StartPopulation;c = factors.Galaxy3ConquerorCount;}
			if (Group == EnumUpgradesGroup.Galaxy4) {p = factors.Galaxy4StartPopulation;c = factors.Galaxy4ConquerorCount;}
			p = p.CopyThis();// чистое население
			var p1 = p.CopyThis();
			p1.MulValue((1+c));// добавляем добавку в зависимости от количества захватов
			_population = p1;
			_captured = Army.IsBiggerThen(_population);

			// ** IncomeMoney
			var m1 = 1;// удваивает доход если галактика захвачена
			if (_captured){m1++;}
			var im = new MegaInt(0, 0);
			if (Army.IsBigger0()){// узнаем показатели для армии
				var a = Army.CopyThis();
				a.MulValue(m);
				if (_captured)a.MulValue(m);// если захвачено доход увеличиваем ещё в 2 раза
				im.AddValue(a);
			}
			im.MulValue(m1);
			IncomeMoney = im;

			// ** цена клика
			var c1 = new MegaInt(0, 1);// по умолчанию доход 1
			var c2 = im.CopyThis();
			c2.DivValue(100);
			//c2.MulValue();// модификатор сколько получаем от IncomeMoney
			c1.AddValue(c2);
			c1.MulValue(factors.Galaxy1ClickCostMultiplier);
			ClickCost = c1;
			
			// ** скорость 
			TimeDelay = t;
			var td = factors.Galaxy1TimeDelayMultiplier;
			if (td > 0) TimeDelay /= td;

			// ** Герои
			ArmyHeroes = new MegaInt(0, 0);
			if (_captured && factors.Galaxy1ConquerorCount > 0){// если галактика захвачена и ранее была захвачена уже
				var a1 = Army.CopyThis();
				a1.DivValue(1000000);// 1 из миллиона может стать героем
				if (a1.IsBigger0()){
					ArmyHeroes = a1.CopyThis();// вычисляем количество героев
					var a = ArmyHeroes.CopyThis();
					a.MulValue(1000);
					IncomeMoney.AddValue(a);
				}
			}

		}

	}
}
