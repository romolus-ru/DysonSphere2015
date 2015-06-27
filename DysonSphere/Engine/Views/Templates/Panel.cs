using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;

namespace Engine.Views.Templates
{
	/// <summary>
	/// Рассчитывает общие размеры своих элементов и корректирует вывод элементов ползунками
	/// </summary>
	/// <remarks>Всё будет вылазить за экран. вариантов 2 - или максимальные размеры как у карты или дискретно переключать и прятать те элементы которые вылазят за пределы</remarks>
	public class Panel:ViewControl
	{
		private TrackBar _scrollHor;
		private TrackBar _scrollVer;
		private const int ScrollBarSize = 25;
		/// <summary>
		/// Режим отображения. 
		/// </summary>
		/// <remarks>
		/// Если простой - то прячем элементы, которые вылезают за границы и останавливаемся у ближайшего к курсору, ближайшие аналог - обычный список строк
		/// Если не простой - то прячем только те элементы, которые совсем не влезают в окно, остальные выводим несмотря на вылезание
		/// После освоения технологии Вывода в текстуру можно будет усовершенствовать - выводить в любом случае и обрезать выводимую текстуру
		/// </remarks>
		private Boolean _isSimple = false;
		public Panel(Controller controller, Boolean isSimple=true) : base(controller)
		{
			_isSimple = isSimple;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			_scrollHor = new TrackBar(Controller);
			_scrollVer = new TrackBar(Controller);
			_scrollVer.IsVertical = true;
			_scrollHor.SetCoordinates(0, Height - ScrollBarSize);
			_scrollHor.SetSize(Width, ScrollBarSize);
			_scrollVer.SetCoordinates(Width - ScrollBarSize,0);
			_scrollVer.SetSize(ScrollBarSize, Height);
			_scrollHor.CurrValueChanged += HorValueChanged;
			_scrollVer.CurrValueChanged += VerValueChanged;
		}

		private void VerValueChanged(int newvalue)
		{
			// опрерделяем поведение при перемещении по вертикали
		}

		private void HorValueChanged(int newvalue)
		{
			// опрерделяем поведение при перемещении по горизонтали
		}

		public void RecalcScrollbars()
		{
			if (Controls.Count < 1) return;
			int xmin = Controls[0].X;
			int ymin = Controls[0].Y;
			int xmax = Controls[0].X;
			int ymax = Controls[0].Y;
			foreach (var control in Controls){
				if (xmin < control.X) xmin = control.X;
				if (ymin < control.Y) ymin = control.Y;
				if (xmax > control.X) xmax = control.X;
				if (ymax > control.Y) ymax = control.Y;
			}
			if (ymax-ymin<Width)_scrollHor.Hide();
			else{
				_scrollHor.SetValues(ymin, ymax);
				_scrollHor.Show();
			}
			if (xmax - xmin < Height) _scrollVer.Hide();
			else{
				_scrollVer.SetValues(xmin, xmax);
				_scrollVer.Show();
			}
		}
	}
}
