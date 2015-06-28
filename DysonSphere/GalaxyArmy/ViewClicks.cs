using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine;
using Engine.Controllers;
using Engine.Utils;
using Engine.Views;

namespace GalaxyArmy
{
	/// <summary>
	/// Визуализирует клики, показывая где кликнул человек и на сколько
	/// </summary>
	class ViewClicks
	{
		private List<ViewClicks1> _clicks = new List<ViewClicks1>();
		private Random _rnd = new Random();
		/// <summary>
		/// Кликаем
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="moneyAdded"></param>
		public void ClickAdd(int x, int y, MegaInt moneyAdded)
		{
			var a = new ViewClicks1(x, y, moneyAdded.GetAsString(), _rnd);
			_clicks.Add(a);
		}

		public void Draw(VisualizationProvider vp)
		{
			var toDel = new List<ViewClicks1>();
			foreach (var click in _clicks){
				vp.SetColor(Color.PaleGreen, (byte) click.Alpha);
				vp.Print(click.X, click.Y, click.Added);
				click.Process();
				if (click.Alpha < 1) toDel.Add(click);
			}
			foreach (var clicks1 in toDel){
				_clicks.Remove(clicks1);
			}
		}
	}
}
