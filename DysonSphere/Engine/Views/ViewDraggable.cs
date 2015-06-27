using System;
using System.Drawing;
using System.Windows.Forms;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;

namespace Engine.Views
{
	/// <summary>
	/// Объект визуализации, умеющий обрабатывать события перемещения
	/// </summary>
	public class ViewDraggable : ViewControl
	{
		private StateOne _stateLButton = StateOne.Init();

		/// <summary>
		/// Координата курсора
		/// </summary>
		public Point CursorPoint;

		/// <summary>
		/// Начальная точка перемещения
		/// </summary>
		public Point CursorPointFrom;

		/// <summary>
		/// Private! Режим определения относительного перемещения
		/// </summary>
		protected Boolean DragStarted;

		/// <summary>
		/// Разрешение на включение режима определения относительного перемещения
		/// </summary>
		public Boolean IsCanStartDrag;

		/// <summary>
		/// Коррекция в модальном режиме, когда есть только компонент, и об остальных предках информация отсутствует
		/// </summary>
		protected Point _cursorCorrection;
		
		public ViewDraggable(Controller controller)
			: base(controller)
		{
			IsCanStartDrag = true;// надо активировать режим извне, что бы отлавливать перемещение
		}

		/// <summary>
		/// Определяем то что зависит от перемещения курсора
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="point"></param>
		/// <remarks>На выходе получаем события, которые нужно переопределить - перемещение, дельта, координаты и т.п.</remarks>
		protected override void Cursor(object sender, PointEventArgs point)
		{
			if (CursorPoint.Equals(point.Pt)) return;// выходим, если курсор не сдвинулся
			CursorPoint = point.Pt;
			MouseMove(CursorPoint.X, CursorPoint.Y);
		}

		public override void KeyboardEH(object o, InputEventArgs args)
		{
			if (DragStarted&&!args.Handled){
				args.AddCorrectionCursor(_cursorCorrection.X, _cursorCorrection.Y);
				Keyboard(o, args);
				args.AddCorrectionCursor(-_cursorCorrection.X, -_cursorCorrection.Y);
				args.Handled = true;
				return;
			}
			base.KeyboardEH(o, args);
		}


		protected override void Keyboard(object sender, InputEventArgs e)
		{
			//if (InRange(e.CursorX, e.CursorY)) e.Handled = true;// попробуем без этого. пока не понятно откуда появилось, но прекращает всю дальнейшую обработку
			var sLButton = _stateLButton.Check(e.IsKeyPressed(Keys.LButton));
			if (DragStarted){// кнопка не нажата, значит формируем сигнал о завершении перемещения
				if ((sLButton == StatesEnum.Off)/*|(!e.ParentCursorOver)*/){// событие завершается независимо от перемещения по экрану. если следить за координатами родителя то событие перемещения "передаётся" дальше - одновремено будет перемещаться ещё объекты которые рядом с курсором
					var relX = CursorPointFrom.X - e.CursorX;
					var relY = CursorPointFrom.Y - e.CursorY;
					DragIn(relX, relY);
					DragEnd(relX, relY);
					DragCancel();
					ModalStop();
				}
				e.Handled = true;
				return;
			}
			if ((sLButton == StatesEnum.On)/*&e.ParentCursorOver*/){
				CursorPointFrom = new Point(e.CursorX, e.CursorY);
				if (InRangeToDrag(CursorPointFrom.X, CursorPointFrom.Y)){
					if (ModalStart()){
						BringToFront();
						DragStarted = true; // активируем перемещение и сохраняем текущие координаты
						_cursorCorrection = new Point(e.CursorCorrectionX, e.CursorCorrectionY);
						DragStart(CursorPointFrom.X, CursorPointFrom.Y);
					}
					e.Handled = true;
					MouseClick(e.CursorX, e.CursorY, DragStarted);
				}
			}
			if (!DragStarted) _stateLButton.Check(false); // сбрасываем
		}

		public override bool InRange(int x, int y)
		{
			if (DragStarted) return true;
			return base.InRange(x, y);
		}

		/// <summary>
		/// Определяет область, клик на которой запускает перемещение
		/// </summary>
		/// <returns></returns>
		protected virtual Boolean InRangeToDrag(int x, int y)
		{
			if (!CursorOver) return false;
			return InRange(x, y);
		}
	
		/// <summary>
		/// Начало перемещения после включения режима
		/// </summary>
		public virtual void DragStart(int x, int y) { }

		/// <summary>
		/// Нажатие на кнопку мыши
		/// </summary>
		public virtual void MouseClick(int x, int y, Boolean dragStarted){}

		/// <summary>
		/// Перемещение мыши по слою
		/// </summary>
		public virtual void MouseMove(int x, int y){}

		/// <summary>
		/// Объект перемещён на эти относительные координаты
		/// </summary>
		public virtual void DragEnd(int relX, int relY){}

		/// <summary>
		/// Перемещение, обрабатываемое компонентом
		/// </summary>
		public virtual void DragIn(int relX, int relY) { }

		/// <summary>
		/// Отменяем событие перемещения
		/// </summary>
		public void DragCancel()
		{
			DragStarted = false;
			_cursorCorrection = new Point(0,0);
			_stateLButton.Check(false);
			ModalStop();
		}

	}
}
