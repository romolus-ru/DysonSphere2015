using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine
{
	/// <summary>
	/// Класс для запуска музыки и звуков
	/// </summary>
	public class Sound
	{
		/// <summary>
		/// Объект для обработки звуков
		/// </summary>
		private ISound _sound;

		/// <summary>
		/// Сохраняем для посылки сообщения о нажатии клавиш или перемещении мышки
		/// </summary>
		protected Controller Controller;

		public Sound(ISound sound) { _sound = sound; }

		/// <summary>
		/// Инициализация, сохранение ссылки на контроллер и установка обработчиков
		/// </summary>
		/// <param name="controller"></param>
		public void Init(Controller controller)
		{
			Controller = controller;
			Controller.AddEventHandler("SoundStart", (o, args) => StartEH(o, args as MessageEventArgs));
			Controller.AddEventHandler("SoundStop", (o, args) => StopEH(o, args as MessageEventArgs));
			Controller.AddEventHandler("SoundUnload", (o, args) => UnloadEH(o, args as MessageEventArgs));
			Controller.AddEventHandler("SoundLoad", (o, args) => LoadEH(o, args as SoundLinkEventArgs));
		}

		#region Обработчики

		private void StartEH(object sender, MessageEventArgs messageEventArgs)
		{
			Start(messageEventArgs.Message);
		}

		private void StopEH(object sender, MessageEventArgs messageEventArgs)
		{
			Stop(messageEventArgs.Message);
		}

		private void UnloadEH(object sender, MessageEventArgs messageEventArgs)
		{
			Unload(messageEventArgs.Message);
		}

		private void LoadEH(object sender, SoundLinkEventArgs soundLinkEventArgs)
		{
			Load(soundLinkEventArgs.SoundName, soundLinkEventArgs.FileName);
		}

		#endregion

		#region Основные функции

		/// <summary>
		/// Загрузка файла и присвоение ему имени
		/// </summary>
		/// <param name="soundName"></param>
		/// <param name="fileName"></param>
		protected virtual void Load(string soundName, string fileName) { _sound.Load(soundName, fileName); }

		/// <summary>
		/// Выгрузка файла из памяти
		/// </summary>
		/// <param name="soundName"></param>
		protected virtual void Unload(string soundName) { _sound.Unload(soundName); }

		/// <summary>
		/// Остановить воспроизведение звука
		/// </summary>
		/// <param name="soundName"></param>
		protected virtual void Stop(string soundName) { _sound.Stop(soundName); }

		/// <summary>
		/// Запустить воспроизведение файла
		/// </summary>
		/// <param name="soundName"></param>
		protected virtual void Start(string soundName) { _sound.Start(soundName); }

		#endregion

		/// <summary>
		/// Очистка данных
		/// </summary>
		public virtual void ClearLinks()
		{
		}
	}
}
