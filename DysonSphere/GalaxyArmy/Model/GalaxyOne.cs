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
		private MegaInt _army;

		/// <summary>
		/// Сколько захвачено
		/// </summary>
		/// <remarks>100% означает что доход нужно считать немного по другому</remarks>
		private int _captured;

		public GalaxyOne()
		{
			TimeDelay = 150;
			TimeDelayCurrent = 0;
			IncomeMoney=new MegaInt();
			IncomeMoney.AddValue(0, 1);
			ClickCost=new MegaInt();
			ClickCost.AddValue(0, 1);

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

	}
}
