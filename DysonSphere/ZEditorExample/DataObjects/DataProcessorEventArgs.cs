using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;

namespace ZEditorExample.DataObjects
{
	class DataProcessorEventArgs:EngineEventArgs
	{
		public DataProcessor DataProcessor;

		static public DataProcessorEventArgs Send(DataProcessor dp)
		{
			var a = new DataProcessorEventArgs();
			a.DataProcessor = dp;
			return a;
		}
	}
}
