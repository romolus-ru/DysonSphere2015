using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyArmy.Model
{
	/// <summary>
	/// Общее назначение улучшения (Тип армии, номер галактики и т.п.)
	/// </summary>
	enum EnumUpgradesGroup
	{
		None,
		/// <summary>
		/// Пехота
		/// </summary>
		Army1,
		/// <summary>
		/// Танки
		/// </summary>
		Army2,
		/// <summary>
		/// Авиация
		/// </summary>
		Army3,
		/// <summary>
		/// Десант
		/// </summary>
		Army4,
		Galaxy1,
		Galaxy2,
		Galaxy3,
		Galaxy4,
		Instructor1,
		Instructor2,
		Instructor3,
		Instructor4,
	}
}
