﻿using System;
using System.Collections.Generic;
using Engine.Controllers;
using Engine.Controllers.Events;

namespace Engine.Models
{
	/// <summary>
	/// Модель. Один из трёх главных классов
	/// </summary>
	public class Model
	{
		/// <summary>
		/// Список математических объектов
		/// </summary>
		private List<IModelObject> _modelObjects = new List<IModelObject>();

		/// <summary>
		/// Контроллер
		/// </summary>
		private Controller _controller;

		/// <summary>
		/// Конструктор
		/// </summary>
		public Model(Controller controller)
		{
			_controller = controller;
			// одинаковые. потом удалить одну
			_controller.AddEventHandler("ModelDelObject", (o, args) => EHDelObject(o, args as ModelObjectEventArgs));
			_controller.AddEventHandler("ModelAddObject", (o, args) => EHAddObject(o, args as ModelObjectEventArgs));

		}

		private void EHDelObject(object sender, ModelObjectEventArgs modelObjectEventArgs)
		{
			_modelObjects.Remove(modelObjectEventArgs.ModelObject);
		}

		/// <summary>
		/// Добавить переданный объект к модели
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="modelObjectEventArgs"></param>
		private void EHAddObject(object sender, ModelObjectEventArgs modelObjectEventArgs)
		{
			AddObject(modelObjectEventArgs.ModelObject);
		}

		/// <summary>
		/// Добавить объект
		/// </summary>
		/// <param name="modelObject"></param>
		public void AddObject(IModelObject modelObject)
		{
			_modelObjects.Add(modelObject);
		}

		/// <summary>
		/// Удалить объект
		/// </summary>
		/// <param name="modelObject"></param>
		public void RemoveObject(IModelObject modelObject)
		{
			_modelObjects.Remove(modelObject);
		}

		/// <summary>
		/// Сделать шаг в алгоритмах модели (одновременно объекты отправят события виду с новой информацией)
		/// </summary>
		public void Execute()
		{
			foreach (var modelObject in _modelObjects)
			{
				modelObject.Execute();
			}
		}
	}
}
