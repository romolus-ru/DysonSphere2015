using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils.Editor;

namespace ZEditorExample.DataObjects
{
	/// <summary>
	/// Имя параметра
	/// </summary>
	class DataParamName:IDataHolder
	{
		public string ParamName;
		public int Num { get; set; }
		public Dictionary<string, string> Save()
		{
			var d = new Dictionary<String, String>();
			d.Add("ParamName", ParamName);
			return d;

		}

		public void Load(Dictionary<string, string> data)
		{
			ParamName = data["ParamName"];
		}
	}
}
