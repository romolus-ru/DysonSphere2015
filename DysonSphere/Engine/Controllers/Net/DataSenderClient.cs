using System;
using Engine.Controllers.Events;

namespace Engine.Controllers.Net
{
	/// <summary>
	/// Отправка событий на сервер
	/// </summary>
	class DataSenderClient : DataSender
	{
		private Client _client;
		private Controller _controller;
		private string _address;
		private int _port;

		public DataSenderClient(Controller controller, string acceptedEvent, string address, int port)
			: base(controller, acceptedEvent)
		{
			_controller = controller;
			_address = address;
			_port = port;
			_client = new Client();
			_client.GetRecieved += Recieve;
			_client.PrintNetDebug += PrintNetDebug;
			_client.Start(_address, _port);
			_controller.AddEventHandler("BeginLoop", ProcessSendRepeat);
		}

		private void ProcessSendRepeat(object sender, EventArgs e)
		{
			if (_client.socket.Available != 0)
			{
				_client.ProcessSendTick();
			}
		}

		public override void Send(DataRecieveEventArgs dr)
		{
			// отправляем клиентам
			//PrintNetDebug("Client send " + dr.EventName + "+" + dr.DataString);
			_client.SendAsync(dr.EventName + "+" + dr.DataString);
		}

	}

}
