using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers.Events;

namespace ZEditorExample.DataObjects
{
	class DataLinkParamEventArgs:EngineEventArgs
	{
		/// <summary>
		/// Номер выделенного объекта
		/// </summary>
		public int NumProcessor;
		/// <summary>
		/// полный список связей параметров
		/// </summary>
		public DataLinkParamLayer DataLinkParamsLayer;

		/// <summary>
		/// полный список параметров
		/// </summary>
		public DataParamNameLayer DataParamNamesLayer;

		/// <summary>
		/// Отправляем параметры
		/// </summary>
		/// <param name="numProcessor">Номер процессора</param>
		/// <param name="dataLinkParamsLayer">связи параметров</param>
		/// <param name="dataParamNamesLayer">имена параметров</param>
		/// <returns></returns>
		public static DataLinkParamEventArgs Send(int numProcessor, DataLinkParamLayer dataLinkParamsLayer, DataParamNameLayer dataParamNamesLayer)
		{
			var a = new DataLinkParamEventArgs();
			a.NumProcessor = numProcessor;
			a.DataLinkParamsLayer = dataLinkParamsLayer;
			a.DataParamNamesLayer = dataParamNamesLayer;
			return a;
		}
	}
}
