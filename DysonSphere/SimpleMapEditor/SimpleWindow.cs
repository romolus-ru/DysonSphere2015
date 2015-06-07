using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Views;
using Button = Engine.Views.Templates.Button;

namespace SimpleMapEditor
{
	/// <summary>
	/// Тестовое окно с кнопкой
	/// </summary>
	class SimpleWindow : ViewDraggable
	{
		public SimpleWindow(Controller controller) : base(controller)
		{
			AddControl(Button.CreateButton(controller, 10, 10, 20, 20, "UU", "Кнопка", "S", Keys.U, "UU"));
			Name = "SW";
		}


	}
}
