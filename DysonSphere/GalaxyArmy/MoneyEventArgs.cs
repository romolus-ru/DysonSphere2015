using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;
using Engine.Utils;

namespace GalaxyArmy
{
	class MoneyEventArgs:EngineEventArgs
	{
		public MegaInt Money;

		public static MoneyEventArgs Send(MegaInt money)
		{
			var a = new MoneyEventArgs();
			a.Money = money;
			return a;
		}
	}
}
