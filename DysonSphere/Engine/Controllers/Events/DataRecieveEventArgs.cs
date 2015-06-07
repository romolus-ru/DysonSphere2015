using System;

namespace Engine.Controllers.Events
{
	/// <summary>
	/// Отправить данные
	/// </summary>
	[Serializable]
	public class DataRecieveEventArgs : EngineEventArgs
	{
		/// <summary>
		/// Имя отправляемого события
		/// </summary>
		public String EventName = "";
		/// <summary>
		/// Отправляемые данные
		/// </summary>
		public String DataString = "";

		public static DataRecieveEventArgs Send(String eventName, String dataString)
		{
			var r = new DataRecieveEventArgs();
			r.EventName = eventName;
			r.DataString = dataString;
			return r;
		}

	}
}
