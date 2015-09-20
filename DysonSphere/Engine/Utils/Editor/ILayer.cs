using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Engine.Views.Templates;

namespace Engine.Utils.Editor
{
	/// <summary>
	/// Интерфейс для слоя, определяет какие операции нужно сделать
	/// </summary>
	/// <typeparam name="T">Тип объекта, с которым работает слой</typeparam>
	/// <remarks>Нужно для обхода работы с объектами, без интерфейса ошибка возникает, несмотря на прямое наследование</remarks>
	public interface ILayer<out T> where T : IDataHolder
	{
		/// <summary>
		/// Владелец слоя, редактор
		/// </summary>
		Editor Editor { get; set; }
		/// <summary>
		/// Имя слоя
		/// </summary>
		String LayerName { get; set; }

		/// <summary>
		/// Можно ли сохранять слой 
		/// </summary>
		/// <remarks>иногда нужно отключать такую возможность
		/// особенно если не только этот слой работает с этими данными 
		/// (слой может быть вспомогательным и работать с объектами, используемыми в другом слое,
		/// в таком случае их сохранять не нужно)</remarks>
		Boolean CanStore { get; set; }

		/// <summary>
		/// Добавить кнопку к списку
		/// </summary>
		Button AddButton(int x, int y, int width, int height, string eventName, String caption, String hint, System.Windows.Forms.Keys key);

		/// <summary>
		/// Добавляем новый объект, в ответ получаем его номер в общем словаре. который является и его идентификатором
		/// </summary>
		/// <returns></returns>
		int AddObject(String objType);

		/// <summary>
		/// Добавляем новый объект, в ответ получаем его номер в общем словаре. который является и его идентификатором
		/// </summary>
		/// <returns></returns>
		int AddObject(IDataHolder obj);
		T GetObject(int num);
		T CreateObject(String objType);

		/// <summary>
		/// Сохранить данные слоя в поток
		/// </summary>
		/// <returns></returns>
		MemoryStream Save();

		/// <summary>
		/// Загрузить данные слоя из потока
		/// </summary>
		/// <param name="data"></param>
		void Load(MemoryStream data);

		void Draw(VisualizationProvider visualizationProvider);

		/// <summary>
		/// Вывод объектов слоя по требованию. может вызываться из других слоёв, нужен для целостного отображения информации
		/// </summary>
		/// <param name="visualizationProvider"></param>
		/// <param name="mapX">Координаты центра карты по X</param>
		/// <param name="mapY">Координаты центра карты по Y</param>
		void DrawObjectInBackground(VisualizationProvider visualizationProvider, int mapX, int mapY);
	}
}
