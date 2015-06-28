using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyArmy
{
	class ViewClicks1
	{
		public string Added;
		public int X;
		public int Y;
		public int Alpha = 100;
		public int Dx = 0;
		public int Dy = 0;

		public ViewClicks1(int x, int y, string added, Random rnd)
		{
			X = x;
			Y = y;
			Added = added;
			Dx = rnd.Next(-5, 5);
			Dy = rnd.Next(-5, 5);
		}

		/// <summary>
		/// Перемещаем и подобное
		/// </summary>
		public void Process()
		{
			X += Dx;
			Y += Dy;
			Alpha--;
		}
	}
}
