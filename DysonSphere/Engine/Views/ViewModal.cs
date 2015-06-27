using System;
using Engine.Controllers;

namespace Engine.Views
{
	/// <summary>
	/// Прячет клавиатуру и мышь, что бы работать с ними единолично
	/// </summary>
	/// <remarks>Несамостоятельный объект. При создании необходимо передать созданный модальный объект управляющему объекту
	///  А после - запустить метод удаления объекта</remarks>
	public class ViewModal : ViewControl
	{
		/// <summary>
		/// Результат работы функции
		/// </summary>
		public int ModalResult = 0;

		/// <summary>
		/// Событие, которое сработает при закрытии модального окна
		/// </summary>
		protected String OutEvent;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="outEvent">Событие, генерируемое при выходе или закрытии модального объекта. Событие обязательно с отстрочкой или выполняемое вне этого объекта - что бы небыло ошибок</param>
		public ViewModal(Controller controller, String outEvent)
			: base(controller)
		{
			OutEvent = outEvent;
		}

		protected override void HandlersAdd()
		{
			base.HandlersAdd();
			ModalStart();
		}

		protected override void HandlersRemove()
		{
			ModalStop();
			base.HandlersRemove();
		}

	}
}
