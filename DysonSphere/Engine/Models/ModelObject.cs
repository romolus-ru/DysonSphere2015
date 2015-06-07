using System;
using Engine.Controllers;

namespace Engine.Models
{
	public class ModelObject : IModelObject
	{
		/// <summary>
		/// Выполнить шаг в алгоритме мат. объекта
		/// </summary>
		public virtual void Execute()
		{

		}

		/// <summary>
		/// Ссылка на контроллер
		/// </summary>
		public Controller Controller { get; private set; }

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller">Контроллер</param>
		public ModelObject(Controller controller)
		{
			// работа с событиями, создание и подключение
			Controller = controller;
		}

		public virtual void Init()
		{
			HandlersAdd();
		}

		/// <summary>
		/// Добавить обработчики
		/// </summary>
		/// <remarks>Предназначения для переопределения пользователем</remarks>
		protected virtual void HandlersAdd()
		{
		}

		/// <summary>
		/// Удалить обработчики
		/// </summary>
		/// <remarks>Предназначена для переопределения пользователем</remarks>
		protected virtual void HandlersRemove()
		{
		}

		/// <summary>
		/// Имя объекта
		/// </summary>
		public String Name { get; set; }

		/// <summary>
		/// Для блокировки дополнительных вызовов dispose
		/// </summary>
		private Boolean _disposed = false;

		/// <summary>
		/// Удаление, можно дополнить у потомков
		/// </summary>
		public virtual void Dispose()
		{
			if (!_disposed)
			{
				HandlersRemove();
				Controller = null;
				_disposed = true;
			}
		}

		/// <summary>
		/// Деструктор
		/// </summary>
		~ModelObject()
		{
			Dispose();
		}
	}
}
















