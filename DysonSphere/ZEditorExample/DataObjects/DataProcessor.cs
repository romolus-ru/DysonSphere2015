using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Utils.Editor;
using ZEditorExample.DataObjects;

namespace ZEditorExample
{
	/// <summary>
	/// Основной класс для вывода на экран
	/// </summary>
	class DataProcessor:DataPoint,IDataHolder
	{
		public int Num { get; set; }

		public int Height { get; set; }
		public int Width { get; set; }

		public string Text;

		public Dictionary<string, string> Save()
		{
			var d = new Dictionary<String, String>();
			d.Add("PosX", PosX.ToString());
			d.Add("PosY", PosY.ToString());
			d.Add("Height", Height.ToString());
			d.Add("Width", Width.ToString());
			d.Add("Text", Text);
			return d;

		}

		public void Load(Dictionary<string, string> data)
		{
			PosX = Convert.ToInt32(data["PosX"]);
			PosY = Convert.ToInt32(data["PosY"]);
			Height = Convert.ToInt32(data["Height"]);
			Width = Convert.ToInt32(data["Width"]);
			Text = data["Text"];
		}

	}
}
