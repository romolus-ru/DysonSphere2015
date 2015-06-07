using System;
using Engine.Controllers;

namespace Engine.Views
{
	public class ViewModalInput : ViewModal
	{
		protected String _text;
		protected String _value;

		public ViewModalInput(Controller controller, string outEvent,  string value)
			: base(controller, outEvent)
		{
			_text = value;
			_value = value;
		}

		public String GetResult() { return _text; }

	}
}
