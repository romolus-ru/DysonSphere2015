using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;
using Engine.Utils;
using Engine.Views;

namespace GalaxyArmy
{
	/// <summary>
	/// Информация об инструкторе
	/// </summary>
	/// <remarks>Содержит основные данные об инструкторе</remarks>
	class InstructorInfo:ViewControl
	{
		public string InstructorName;
		public int Level;
		/// <summary>
		/// Добавляет атаки обучаемым
		/// </summary>
		public int AddAttack=0;
		/// <summary>
		/// Добавляет защиты обучаемым
		/// </summary>
		public int AddDefance=0;
		/// <summary>
		/// Добавляет живучести обучаемым
		/// </summary>
		public int AddVitality=0;

		public InstructorInfo(Controller controller) : base(controller)
		{
			var g = new RandomNameGenerator();
			InstructorName = g.GenerateRusName();
			Level = 0;// не куплен
		}

		private Dictionary<int, Color> _instructorColors = InitInstructorColors();
		private Dictionary<int, MegaInt> _upgradeCostInts = InitUpgradeCosts();

		public Color GetColor(int level)
		{
			return _instructorColors[level];
		}
		public MegaInt getCost(int level)
		{
			return _upgradeCostInts[level];
		}
		private static Dictionary<int, MegaInt> InitUpgradeCosts()
		{
			var ret = new Dictionary<int, MegaInt>();
			for (int i = 1; i < 9; i++){
				var a = MegaInt.Function1(9000, i);
				a.AddValue(39, 0);
				ret.Add(i, a);
			}
			return ret;
		}

		private static Dictionary<int, Color> InitInstructorColors()
		{
			var ret = new Dictionary<int, Color>();
			ret.Add(1, Color.Beige);
			ret.Add(2, Color.Violet);
			ret.Add(3, Color.Red);
			ret.Add(4, Color.Blue);
			ret.Add(5, Color.Orange);
			ret.Add(6, Color.Green);
			ret.Add(7, Color.Yellow);
			ret.Add(8, Color.Turquoise);
			return ret;
		}
	}
}
