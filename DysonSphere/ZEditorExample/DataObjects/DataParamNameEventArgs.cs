using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;

namespace ZEditorExample.DataObjects
{
	class DataParamNameEventArgs:EngineEventArgs
	{
		public DataParamName DataParamName;
		public int EditPos;

		static public DataParamNameEventArgs Send(DataParamName dppn,int editPos)
		{
			var a = new DataParamNameEventArgs();
			a.DataParamName = dppn;
			a.EditPos = editPos;
			return a;
		}
	}
}
