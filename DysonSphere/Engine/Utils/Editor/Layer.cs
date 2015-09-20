﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Views;
using Button = Engine.Views.Templates.Button;

namespace Engine.Utils.Editor
{
	/// <summary>
	/// слой в редакторе
	/// </summary>
	/// <remarks>Содержит словарь объектов с интерфейсом T</remarks>
	public class Layer<T> : ViewDraggable, ILayer<T> where T : IDataHolder
	{
		/// <summary>
		/// Счётчик для объектов
		/// </summary>
		private int _counter;

		/// <summary>
		/// Словарь объектов, поддерживающих интерфейс
		/// </summary>
		/// <remarks>
		/// странно что он не отображён в интерфейсе, но так и получается что неизвестно 
		/// как будут храниться данные, а доступ к ним должен быть
		/// </remarks>
		public Dictionary<int, T> Data = new Dictionary<int, T>();

		/// <summary>
		/// Смещение центра карты
		/// </summary>
		protected int MapDelta = 16;

		[Obsolete]
		public void SynhronizeMapCoords(int mx, int my)
		{
			Editor.MapX = mx;
			Editor.MapY = my;
		}

		public Layer(Controller controller, String layerName) : base(controller)
		{
			_counter = 0;
			LayerName = layerName;
			IsCanStartDrag = false;// надо активировать режим извне, что бы отлавливать перемещение
			CanStore = true;// по умолчанию можно сохранять данные слоя
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			X = 0;
			Y = 0;
			Width = 500;
			Height = 500;
		}

		/// <summary>
		/// Убрать пропуски в индексных номерах объектов
		/// возможно, надо потом заменить на опцию - искать пропуски в ряду данных, по которому будут искаться свободные номера ниже counter
		/// </summary>
		public void CompressIndexes() { }

		public Editor Editor { get; set; }

		/// <summary>
		/// Имя слоя, в архиве будет с таким же именем храниться
		/// </summary>
		public String LayerName { get; set; }

		public bool CanStore { get; set; }

		public Button AddButton(int x, int y, int width, int height, string eventName,
			string caption, string hint, Keys key)
		{
			var btn = Button.CreateButton(Controller, x, y, width, height, eventName, caption, hint, key, caption);
			AddControl(btn);
			return btn;
		}

		/// <summary>
		/// Добавляем новый объект, в ответ получаем его номер в общем словаре. который является и его идентификатором
		/// </summary>
		/// <returns></returns>
		public int AddObject(String objType)
		{
			var newObj = CreateObject(objType);
			var c=AddObject(newObj);
			return c;
		}

		public virtual int AddObject(IDataHolder obj)
		{
			_counter++;
			obj.Num = _counter;
			Data.Add(_counter, (T)obj);
			return _counter;
		}

		public T GetObject(int num)
		{
			return Data[num];
		}

		/// <summary>
		/// По имеющемуся экземпляру объекта получить его номер
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public int GetNum(T obj)
		{
			var ret = -1;
			foreach (var kv1 in Data){
				if (kv1.Value.Equals(obj)) ret = kv1.Key;
			}
			return ret;
		}


		public virtual T CreateObject(String objType)
		{
			return default(T);
		}

		/// <summary>
		/// Сохранить данные слоя в поток
		/// </summary>
		public MemoryStream Save()
		{
			var ms = new MemoryStream();
			var document = new XmlDocument();
			document.CreateXmlDeclaration("1.0", "utf-8", null);
			XmlNode root = document.CreateElement("layer");
			document.AppendChild(root);
			XmlNode lNode1 = document.CreateElement("name");
			lNode1.InnerText = LayerName;
			root.AppendChild(lNode1);// сохраняем имя слоя
			XmlNode lNode2 = document.CreateElement("counter");
			lNode2.InnerText = _counter.ToString();
			root.AppendChild(lNode2);// сохраняем счётчик
			// проходим по элементам, каждый из них сохраняем с некоторыми 
			//отличительными параметрами (например тип объекта) и добавляем dictionary свойств
			foreach (var data in Data)
			{
				XmlNode itemNode = document.CreateElement("item");
				root.AppendChild(itemNode);

				XmlNode numItem = document.CreateElement("num");
				numItem.InnerText = data.Key.ToString();
				itemNode.AppendChild(numItem);

				XmlNode typeItem = document.CreateElement("type");// для типа узнаём только основной тип объекта, без сборки и т.п.
				// но из-за этого могут проявиться ошибки
				var s = data.Value.GetType().ToString();
				typeItem.InnerText = s.Substring(s.LastIndexOf('.') + 1);
				itemNode.AppendChild(typeItem);

				XmlNode dataItem = document.CreateElement("data");
				itemNode.AppendChild(dataItem);

				var dataH = data.Value.Save();
				foreach (var dh in dataH)
				{
					XmlNode row = document.CreateElement(dh.Key);
					row.InnerText = dh.Value;
					dataItem.AppendChild(row);
				}
			}
			document.Save(ms);
			return ms;
		}

		/// <summary>
		/// Загрузить данные слоя из потока
		/// </summary>
		/// <param name="data"></param>
		public void Load(MemoryStream data)
		{
			var document = new XmlDocument();
			document.Load(data);
			XmlNode root = document.DocumentElement;
			foreach (XmlNode item in root.ChildNodes){
				if (item.Name == "name"){
					if (LayerName != item.InnerText){
						throw new Exception("Сюрприз! имя слоя как файла не совпадает с именем слоя в слое и послойно неслоится");
					}
					LayerName = item.InnerText; continue;
				}// имя
				if (item.Name == "counter") { _counter = Convert.ToInt32(item.InnerText); continue; }// счётчик созданных объектов
				if (item.Name != "item") { continue; }// работаем с элементом
				int num = -1;
				String tp = "";
				foreach (XmlNode item2 in item.ChildNodes){// получаем основные данные
					// получаем номер
					if (item2.Name == "num") num = Convert.ToInt32(item2.InnerText);
					// получаем тип
					if (item2.Name == "type") tp = item2.InnerText;
				}
				if (num == -1) continue;// если номер не нашли значит что то не то - грузим следующий объект
				T tmp = CreateObject(tp);// создаём нужный объект
				if (tmp==null)throw new Exception("функция CreateObject скорее всего не переопределена");
				Dictionary<String, String> dd = new Dictionary<string, string>();// словарь для загрузки данных
				foreach (XmlNode item2 in item.ChildNodes){// получаем данные для воссоздания объекта
					if (item2.Name != "data") continue;// нужны только данные словаря
					foreach (XmlNode node in item2.ChildNodes){
						var k = node.Name;
						var v = node.InnerText;
						dd.Add(k, v);
					}
				}
				tmp.Load(dd);
				tmp.Num = num;// присваиваем отдельно, в сохранении этого номера нету
				//не загружается
				Data.Add(num, tmp);
			}
		}

		/// <summary>
		/// Вывод объектов слоя по требованию. может вызываться из других слоёв, нужен для целостного отображения информации
		/// </summary>
		/// <param name="visualizationProvider"></param>
		/// <param name="mapX">Координаты центра карты по X</param>
		/// <param name="mapY">Координаты центра карты по Y</param>
		public virtual void DrawObjectInBackground(VisualizationProvider visualizationProvider, int mapX, int mapY)
		{
			
		}


		protected override void Keyboard(object sender, InputEventArgs e)
		{
			base.Keyboard(sender, e);
			if (e.IsKeyPressed(Keys.Left)) Editor.MapX += MapDelta;
			if (e.IsKeyPressed(Keys.Right)) Editor.MapX -= MapDelta;
			if (e.IsKeyPressed(Keys.Up)) Editor.MapY += MapDelta;
			if (e.IsKeyPressed(Keys.Down)) Editor.MapY -= MapDelta;
		}

		/// <summary>
		/// Убираем фон
		/// </summary>
		/// <param name="visualizationProvider"></param>
		protected override void DrawComponentBackground(VisualizationProvider visualizationProvider)
		{ }

		protected override bool InRangeToDrag(int x, int y)
		{
			//if (DragStarted)return true;
			return base.InRange(x, y);// не header, на слое передвигается объект, а не сам слой
		}

		
	}
}
