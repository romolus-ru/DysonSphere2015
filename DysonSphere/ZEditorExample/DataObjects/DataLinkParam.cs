using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils.Editor;

namespace ZEditorExample.DataObjects
{
	/// <summary>
	/// Связь между именем параметра и процессором данных
	/// </summary>
	class DataLinkParam:IDataHolder
	{
		public int Num { get; set; }
		public int NumParam;
		public int NumProcessor;

		public Dictionary<string, string> Save()
		{
			var d = new Dictionary<String, String>();
			d.Add("NumParam", NumParam.ToString());
			d.Add("NumProcessor", NumProcessor.ToString());
			return d;

		}

		public void Load(Dictionary<string, string> data)
		{
			NumParam = Convert.ToInt32(data["NumParam"]);
			NumProcessor = Convert.ToInt32(data["NumProcessor"]);
		}
	}
}
