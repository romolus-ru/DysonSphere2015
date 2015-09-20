using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utils
{
	/// <summary>
	/// Примеры методов для конвертирования словаря в строку и обратно
	/// </summary>
	class DictionaryString
	{
		string GetLine(Dictionary<string, int> d)
		{
			// Build up each line one-by-one and then trim the end
			StringBuilder builder = new StringBuilder();
			foreach (KeyValuePair<string, int> pair in d)
			{
				builder.Append(pair.Key).Append(":").Append(pair.Value).Append(',');
			}
			string result = builder.ToString();
			// Remove the final delimiter
			result = result.TrimEnd(',');
			return result;
		}

		Dictionary<string, int> GetDict(string f)
		{
			Dictionary<string, int> d = new Dictionary<string, int>();
			string s = File.ReadAllText(f);
			// Divide all pairs (remove empty strings)
			string[] tokens = s.Split(new char[] { ':', ',' },
				StringSplitOptions.RemoveEmptyEntries);
			// Walk through each item
			for (int i = 0; i < tokens.Length; i += 2)
			{
				string name = tokens[i];
				string freq = tokens[i + 1];

				// Parse the int (this can throw)
				int count = int.Parse(freq);
				// Fill the value in the sorted dictionary
				if (d.ContainsKey(name))
				{
					d[name] += count;
				}
				else
				{
					d.Add(name, count);
				}
			}
			return d;
		}

	}
}
