namespace Engine.Controllers.Events
{
	/// <summary>
	/// Получение объекта Collector
	/// </summary>
	class GetCollectorEventArgs : EngineEventArgs
	{
		/// <summary>
		/// Коллектор
		/// </summary>
		public Collector Collector;
	}
}