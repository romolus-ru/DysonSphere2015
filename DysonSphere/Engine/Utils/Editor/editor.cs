using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;

namespace Engine.Utils.Editor
{
	/// <summary>
	/// Класс - основа для всех редакторов
	/// </summary>
	/// <remarks>
	/// Для нормальной работоспособности нужно переопределить записываемые данные, сохраняемые данные в слое и т.п.
	/// слои являются обхектами класса Layer с интерфейсом ILayer, хранятся в Controls
	/// </remarks>
	public class Editor : ViewControl
	{
		//private List<ILayer<IDataHolder>> _layers = new List<ILayer<IDataHolder>>();

		/// <summary>
		/// Кордината центра карты
		/// </summary>
		public int MapX;

		/// <summary>
		/// Кордината центра карты
		/// </summary>
		public int MapY;

		/// <summary>
		/// Степень уменьшения. от 1:1 до 1:10 и более. вторая цифра и есть степень уменьшения
		/// </summary>
		public int Zoom;

		/// <summary>
		/// Спрятанное поле. все функции работают с текущим слоем, чаще всего
		/// </summary>
		private ILayer<IDataHolder> _currentLayer = null;

		public Editor(Controller controller) : base(controller) { }

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			X = 0;
			Y = 0;
			Width = 800;
			Height = 600;
		}

		private Boolean LayerExists(String layerName) { return (GetLayer(layerName) != null); }

		private ILayer<IDataHolder> GetLayer(String layerName) {
			foreach (var control in Controls){
				var l = control as ILayer<IDataHolder>;
				if (l == null) continue;
				if (l.LayerName == layerName) return l;
			}
			return null;
		}

		public void AddNewLayer(ILayer<IDataHolder> layer)
		{
			//одинаковые имена в любом случае противопоказаны
			if (LayerExists(layer.LayerName)) return;// если такой слой уже создан то выходим
			var l = layer as ViewDraggable;
			(l as ILayer<IDataHolder>).Editor = this;
			AddControl(l);
		}

		public void SetCurrentLayer(String layerName)
		{
			_currentLayer = GetLayer(layerName);
		}

		/// <summary>
		/// Создать объект в текущем слое
		/// </summary>
		/// <returns></returns>
		public int AddNewObject(String objectType)
		{
			return _currentLayer.AddObject(objectType);
		}

		/// <summary>
		/// Создать объект в текущем слое
		/// </summary>
		/// <returns></returns>
		public int AddNewObject(IDataHolder obj)
		{
			return _currentLayer.AddObject(obj);
		}

		/// <summary>
		/// Получить объект по индексу
		/// </summary>
		/// <returns></returns>
		public IDataHolder GetObject(int num)
		{
			return _currentLayer.GetObject(num);
		}

		/// <summary>
		/// Сохранить слои в архив
		/// </summary>
		/// <param name="fileName"></param>
		public void Save(string fileName)
		{
			var a = new FileArchieve(fileName);
			foreach (var control in Controls){
				var layer=control as ILayer<IDataHolder>;
				if (layer == null) continue;
				if (!layer.CanStore) { continue; }
				var ms = layer.Save();
				a.AddStream(layer.LayerName, ms);
			}
			a.Dispose();
		}

		/// <summary>
		/// Загрузить слои из архива
		/// </summary>
		/// <param name="fileName"></param>
		public void Load(string fileName)
		{
			if (!File.Exists(fileName)) return;
			var a = new FileArchieve(fileName, false);
			foreach (var fl in a.Files){
				string layerName = fl.FullName;
				if (!LayerExists(layerName)) { continue; }// все нужные слои должны быть созданы заранее
				var layer = GetLayer(layerName);// получаем на него ссылку, что бы загрузить данные
				var ms = a.GetStream(layerName);
				layer.Load(ms);
			}
			a.Dispose();
		}

		/// <summary>
		/// Список объектов у текущего слоя (возвращается интерфейс IDataObject)
		/// </summary>
		/// <returns></returns>
		public IEnumerable<IDataHolder> Objects()
		{
			return null;
		}

		/// <summary>
		/// Переопределяем - фон не должен реагировать на мышу и даже присутствовать не должен
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.PowderBlue);
			visualizationProvider.Rectangle(X, Y, Width, Height);
		}

		/// <summary>
		/// Установить активный слой, остальные скрыть
		/// </summary>
		/// <param name="layerName"></param>
		/// <remarks>Было на ActiveHandlers но решил убрать это, всё равно основа это видимость слоя. можно будет другой флаг ввести - обработчики тут тоже свои пока</remarks>
		public void SetActiveLayer(string layerName)
		{
			foreach (var control in Controls){
				var layer = control as ILayer<IDataHolder>;
				if (layer == null) continue;
				control.Hide();
				if (layer.LayerName == layerName){
					control.Show();
				}
			}
			SetCurrentLayer(layerName);
		}

		/// <summary>
		/// Вычисляем расстояние между координатами
		/// </summary>
		/// <returns></returns>
		public float Distance(int x, int y, int X, int Y)
		{
			var dx = x - X;
			var dy = y - Y;
			return (float)Math.Sqrt(dx * dx + dy * dy);
		}

	}
}
