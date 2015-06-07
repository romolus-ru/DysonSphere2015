using System;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine;
using Engine.Views;

namespace Engine.Utils.ExtensionMethods
{
	public static class Systems
	{
		/// <summary>
		/// Добавить объект к визуализации
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="sender"></param>
		/// <param name="viewControl"></param>
		public static void ViewAddObjectCommand(this Controller controller, Object sender, ViewControl viewControl)
		{
			var eventArgs = ViewControlEventArgs.Send(viewControl);
			controller.StartEvent("ViewAddObject", sender, eventArgs);
		}

		/// <summary>
		/// Удалить объект из визуализации
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="sender"></param>
		/// <param name="viewControl"></param>
		public static void ViewDelObjectCommand(this Controller controller, Object sender, ViewControl viewControl)
		{
			var eventArgs = ViewControlEventArgs.Send(viewControl);
			controller.StartEvent("ViewDelObject", sender, eventArgs);
		}

		/// <summary>
		/// Завершить работу
		/// </summary>
		/// <param name="controller"></param>
		public static void SystemExitCommand(this Controller controller)
		{
			controller.StartEvent("systemExit");
		}

		/// <summary>
		/// Отправить данные
		/// </summary>
		/// <param name="controller"></param>
		public static void SendTo<T>(this Controller controller, string sendTo, string eventName, T eventArgs) where T : EngineEventArgs
		{
			var dr = DataRecieveEventArgs.Send(eventName, eventArgs.Serialize<T>());
			controller.AddToOperativeStore(null, StoredEventEventArgs.Stored(sendTo, null, dr));
		}

		/// <summary>
		/// Отправить данные модели (на сервер) (сработает в начале следующего цикла)
		/// </summary>
		/// <param name="controller"></param>
		public static void SendToModelCommand<T>(this Controller controller, string eventName, T eventArgs) where T : EngineEventArgs
		{
			SendTo(controller, "SendToModel", eventName, eventArgs);
		}

		/// <summary>
		/// Отправить данные виду (клиенту)
		/// </summary>
		/// <param name="controller"></param>
		public static void SendToViewCommand<T>(this Controller controller, string eventName, T eventArgs) where T : EngineEventArgs
		{
			SendTo(controller, "SendToView", eventName, eventArgs);
		}

		/// <summary>
		/// Инициализировать рендер в текстуру
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="viewObject"></param>
		/// <param name="textureName"></param>
		public static void DrawToTexture(this Controller controller, IViewObject viewObject, String textureName)
		{
			controller.StartEvent("ViewDrawToTexture", null, DrawToTextureEventArgs.Set(viewObject, textureName));
		}

	}
}
