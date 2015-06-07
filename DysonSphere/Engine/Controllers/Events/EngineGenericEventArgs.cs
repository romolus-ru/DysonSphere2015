using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Controllers.Events
{
	public class EngineGenericEventArgs<T>:EngineEventArgs
	{
		public T Value { get; set; }

		public static EngineGenericEventArgs<T> Send(T value)
		{
			var ret = new EngineGenericEventArgs<T>();
			ret.Value = value;
			return ret;
		}
	}
}
