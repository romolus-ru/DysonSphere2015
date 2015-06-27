﻿using System;
using System.Collections.Generic;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Views
{
	/// <summary>
	/// Объект встраивается во View и обеспечивает обработку поступающих команд
	/// </summary>
	/// <remarks>Обеспечивает поведение модальных объектов, блокируя </remarks>
	public class ViewSystem : ViewControl
	{
		/// <summary>
		/// Модальный объект
		/// </summary>
		private ViewControl _modalObject;

		public ViewSystem(Controller controller): base(controller)
		{
			_modalObject = null;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			X = 0;// объект занимает весь экран
			Y = 0;
			Height = visualizationProvider.CanvasHeight;
			Width = visualizationProvider.CanvasWidth;
		}

		protected override void HandlersAdd()
		{
			base.HandlersAdd();
			Controller.AddEventHandler("ViewSystem.ModalStart", ModalStartEH);
			Controller.AddEventHandler("ViewSystem.ModalStop", ModalStopEH);
		}

		protected override void HandlersRemove()
		{
			Controller.RemoveEventHandler("ViewSystem.ModalStart", ModalStartEH);
			Controller.RemoveEventHandler("ViewSystem.ModalStop", ModalStopEH);
			base.HandlersRemove();
		}

		private void ModalStartEH(object sender, EventArgs e)
		{
			var a = e as ViewControlEventArgs;
			if (a == null) return;
			if ((_modalObject != null)/*&&(a.ViewControl!=_modalObject)*/){//var o = _modalObject as ViewDraggable;if (o!=null)o.DragCancel();
				return;}
			a.Result = true;
			_modalObject = a.ViewControl;
		}

		private void ModalStopEH(object sender, EventArgs e)
		{
			var a = e as ViewControlEventArgs;
			if (a == null) return;// передаётся объект для того, что бы можно было проверить, тот ли объект отменяет модальность. пока не проверяется
			_modalObject = null;
		}

		public override void CursorEH(object o, PointEventArgs args)
		{
			// если модальный объект задан то событие идёт только непосредственно ему
			if (_modalObject == null)  
				base.CursorEH(o, args);
			else _modalObject.CursorEH(o, args);
		}

		public override void KeyboardEH(object o, InputEventArgs args)
		{
			if (_modalObject == null)  
				base.KeyboardEH(o, args);
			else _modalObject.KeyboardEH(o, args);
		}

		protected override void DrawComponents(VisualizationProvider visualizationProvider)
		{
			if (_modalObject != null) _modalObject.Hide();
			base.DrawComponents(visualizationProvider);
			// прорисованы все объекты, кроме объекта, объявленного модальным
			if (_modalObject != null){
				_modalObject.Show();// не очень хорошо - 2 проверки идут, + скрываем показываем объект
				// но цель будет достигнута - объект будет скрыт, выведутся все остальные объекты, а потом выведется модальный объект
				_modalObject.Draw(visualizationProvider);
			}
		}

		public ViewControl GetModalObject()
		{
			return _modalObject;
		}

	}
}
