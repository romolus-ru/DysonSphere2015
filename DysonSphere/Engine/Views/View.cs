using System;
using System.Collections.Generic;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	/// <summary>
	/// Один из трёх главных классов
	/// </summary>
	public class View
	{
		/// <summary>
		/// Пауза для запрета обработки кнопки
		/// </summary>
		public static int Pause = 30;

		private List<DrawToTextureEventArgs> _drawToTexture = new List<DrawToTextureEventArgs>();

		/// <summary>
		/// Класс для объекта визуализации
		/// </summary>
		private VisualizationProvider _visualizationProvider;

		/// <summary>
		/// Основной корневой объект визуализации, фоновый
		/// </summary>
		private ViewSystem _viewMainObjects = null;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="visualizationProvider"></param>
		public View(Controller controller, VisualizationProvider visualizationProvider)
		{
			// сохраняем всё самое важное из переданного
			_visualizationProvider = visualizationProvider;
			_viewMainObjects = new ViewSystem(controller);
			_viewMainObjects.Init(_visualizationProvider);
			_viewMainObjects.Show();
			controller.AddEventHandler("ViewBringToFront", BringToFrontEH);
			controller.AddEventHandler("ViewSendToBack", SendToBackEH);
			controller.AddEventHandler("ViewAddObject", (o, args) => AddObjectEH(o, args as ViewControlEventArgs));
			controller.AddEventHandler("ViewDelObject", (o, args) => DelObjectEH(o, args as ViewControlEventArgs));
			controller.AddEventHandler("ViewDrawToTexture", (o, args) => DrawToTextureEH(o, args as DrawToTextureEventArgs));
			controller.AddEventHandler("Cursor", CursorEH);
			controller.AddEventHandler("Keyboard", KeyboardEH);
		}

		private void KeyboardEH(object sender, EventArgs e)
		{
			_viewMainObjects.KeyboardEH(sender, e as InputEventArgs);
		}

		private void CursorEH(object sender, EventArgs e)
		{
			_viewMainObjects.CursorEH(sender, e as PointEventArgs);
		}

		private void DrawToTextureEH(object sender, DrawToTextureEventArgs drawToTextureEventArgs)
		{
			// сохраняем переданные параметры
			_drawToTexture.Add(drawToTextureEventArgs);
		}

		private void AddObjectEH(object sender, ViewControlEventArgs viewObjectEventArgs)
		{
			AddObject(viewObjectEventArgs.ViewControl);
		}

		private void DelObjectEH(object sender, ViewControlEventArgs viewObjectEventArgs)
		{
			DeleteObject(viewObjectEventArgs.ViewControl);
		}

		private void BringToFrontEH(object sender, EventArgs eaArgs)
		{
			var c = sender as ViewControl;
			if (c == null) return;
			_viewMainObjects.BringToFront(c);
		}

		private void SendToBackEH(object sender, EventArgs eaArgs)
		{
			var c = sender as ViewControl;
			if (c == null) return;
			_viewMainObjects.SendToBack(c);
		}

		/// <summary>
		/// Добавление объекта
		/// </summary>
		/// <param name="obj"></param>
		public void AddObject(ViewControl obj)
		{
			_viewMainObjects.AddControl(obj);
		}

		/// <summary>
		/// Удаление объекта
		/// </summary>
		/// <param name="obj"></param>
		public void DeleteObject(ViewControl obj)
		{
			_viewMainObjects.RemoveControl(obj);
		}

		/// <summary>
		/// Рисование объектов, с проверкой, кого надо рисовать
		/// </summary>
		public void Draw()
		{
			_visualizationProvider.BeginDraw();
			_viewMainObjects.Draw(_visualizationProvider);
			_visualizationProvider.FlushDrawing();
		}

		/// <summary>
		/// Рисование в текстуру
		/// </summary>
		public void DrawToTexture()
		{
			foreach (var argse in _drawToTexture){
				_visualizationProvider.BeginDraw();
				argse.ViewObject.DrawToTexture(_visualizationProvider);
				_visualizationProvider.CopyToTexture(argse.TextureName);
			}
		}
	}
}
