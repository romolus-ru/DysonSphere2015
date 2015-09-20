using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Views
{
	/// <summary>
	/// Вспомогательный объект для визуализации
	/// </summary>
	/// <remarks>Выводит информацию только когда скажут. вся логика связана или с выводом графики и меняется там или вызывается отдельный заранее созданный метод</remarks>
	public class ViewObject
	{
		public virtual void DrawObject(VisualizationProvider vp)
		{
			
		}
	}
}
