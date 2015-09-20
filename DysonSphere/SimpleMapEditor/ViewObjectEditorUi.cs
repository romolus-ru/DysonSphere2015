using System;
using System.Collections;
using System.Drawing;
using Engine;
using Engine.Controllers;
using Engine.Utils.Editor;
using Engine.Views;
using Engine.Controllers.Events;
using System.Windows.Forms;

namespace SimpleMapEditor
{
	class ViewObjectEditorUi : ViewControl
	{
		public Editor Editor;
		public ObjectTypes ObjType;
		public Boolean NewObjClick;// для создания нового объекта при клике на карте
		public Boolean PaintObjClick;// для перекрашивания объекта при клике на карте
		public Boolean GetInfoObjectClick;// для получения информации об объекте

		/// <summary>
		/// Размер блока
		/// </summary>
		private const int BlockH = 16;
		/// <summary>
		/// Размер блока
		/// </summary>
		private const int BlockW = 16;

		private Controller _controller;
		private Point _cPoint = new Point(10, 10);
		private Boolean _moveOperation = false;
		private SimpleEditableObject _moveObjNum = null;
		private SimpleEditableObject _moveObjNumCurrent = null;
		private SimpleEditableObject _sample = null;
		private Point _moveStart;
		private Point _moveCurrent;
		private Boolean _movePressed;
		private int _x1OldCreated = -9473;
		private int _y1OldCreated = -14748;
		public int CenterX = 0;// для перемещения изображения экрана
		public int CenterY = 0;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		public ViewObjectEditorUi(Controller controller)
			: base(controller)
		{
			this._controller = controller;
			NewObjClick = false;
			PaintObjClick = false;
			GetInfoObjectClick = false;
			Editor = null;
			ObjType = ObjectTypes.Empty;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			visualizationProvider.LoadTexture("main", @"..\Resources\defanceLabirinth.jpg");
			_sample = new SimpleEditableObject();
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

		private void CursorMovedEH(object o, EventArgs args)
		{
			CursorMoved(o, args as PointEventArgs);
		}

		private void KeyboardEH(object o, EventArgs args)
		{
			Keyboard(o, args as InputEventArgs);
		}

		/// <summary>
		/// Отслеживаем перемещение курсора
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="point"></param>
		private void CursorMoved(object sender, PointEventArgs point)
		{
			_cPoint = point.Pt;// точка где счас находится курсор
		}

		protected override void Keyboard(object sender, InputEventArgs e)
		{
			if (e.IsKeyPressed(Keys.E)){// применить к текущему выделенному элементу новый тип текстуры
				// лучше заменить номер на ссылку на настоящий объект
				if (_moveObjNumCurrent != null){// элемент выделен
					//var p=(SimpleEditableObject)Editor.GetObject(moveObjNumCurrent);// SetParam(moveObjNumCurrent, "TexNum", textureNum.ToString());
					//if (p!=null){
					_moveObjNumCurrent.ObjType = ObjType;
					//}
				}
			}
			if (e.IsKeyPressed(Keys.LButton)){
				if (NewObjClick){
					AddNewObject(e);
					e.KeyboardClear();
					return;
				}
				_movePressed = true;// нажатие мыши
				_moveCurrent = new Point(e.CursorX, e.CursorY);
				if (!_moveOperation){
					if (_moveObjNumCurrent == null) return;
					_moveObjNum = _moveObjNumCurrent;
					_moveOperation = true;// признак операции перемещения
					_moveStart = new Point(e.CursorX, e.CursorY); ;
				}
			}
			if (e.IsKeyPressed(Keys.Left)) CenterX += BlockW;
			if (e.IsKeyPressed(Keys.Right)) CenterX -= BlockW;
			if (e.IsKeyPressed(Keys.Up)) CenterY += BlockH;
			if (e.IsKeyPressed(Keys.Down)) CenterY -= BlockH;
		}

		/// <summary>
		/// Собираем разные данные и объединяем их в этом методе
		/// </summary>
		private void MoveProcess()
		{
			if (!_moveOperation) return;
			if (_movePressed){// операции перемещения
			}else{// сохранение перемещения и сброс всех переключателей
				var p = _moveObjNum;
				var x1 = p.X + (_moveCurrent.X - _moveStart.X);
				var y1 = p.Y + (_moveCurrent.Y - _moveStart.Y);
				x1 = ((x1 + BlockW / 2) / BlockW) * BlockW;
				y1 = ((y1 + BlockH / 2) / BlockH) * BlockH;
				p.X = x1;// меняем параметры у объекта
				p.Y = y1;
				_moveOperation = false;
				_moveObjNum = null;
				_moveObjNumCurrent = null;
			}
		}



		private void AddNewObject(InputEventArgs iea)
		{
			if (!NewObjClick) return;
			// проверяем на координаты, что бы не добавлялось много объектов 
			var x1 = ((iea.CursorX - CenterX + BlockW / 2) / BlockH) * BlockW;
			var y1 = ((iea.CursorY - CenterY + BlockH / 2) / BlockW) * BlockH;
			if ((x1 == _x1OldCreated) && (y1 == _y1OldCreated)) return;
			_x1OldCreated = x1;
			_y1OldCreated = y1;
			//следующий этап проверки - что бы по указанным координатам небыло ниодного объекта
			var founded = false;
			foreach (SimpleEditableObject p in Editor.Objects()){
				if (p.X == x1) { if (p.Y == y1) { founded = true; break; } }
			}
			if (!founded)
			{// если такой точки не найдено - добавляем
				var seo = new SimpleEditableObject();
				Editor.AddNewObject(seo);
				seo.X = x1;
				seo.Y = y1;
			}
		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			visualizationProvider.SetColor(Color.BurlyWood);
			visualizationProvider.Print(200, 100, " EditorUI");
			visualizationProvider.Line(000, 000, 100, 100);
			visualizationProvider.Line(000, 000, 800, 000);
			visualizationProvider.Line(800, 000, 800, 600);
			visualizationProvider.Line(800, 600, 000, 600);
			visualizationProvider.Line(000, 600, 000, 000);
			visualizationProvider.SetColor(Color.Aquamarine);

			var countPoints = 0;
			foreach (SimpleEditableObject p in Editor.Objects())
			{
				int x1 = p.X + CenterX;
				int y1 = p.Y + CenterY;
				if (x1 > 800) continue;
				if (y1 > 600) continue;
				visualizationProvider.DrawTexturePart(x1, y1, "main", 16, 16, ObjectTypeAtlas.GetTextureNum(p.ObjType));
				countPoints++;
			}

			visualizationProvider.Print(30, visualizationProvider.CanvasHeight - 35, "Количество объектов " + countPoints);
			var n = SearchNearest(Editor.Objects(), _cPoint.X - CenterX, _cPoint.Y - CenterY);
			if ((n != null) & (!_moveOperation)){
				// сбрасываем выбранный элемент, если он выходит за границы окна
				if ((n.X + CenterX > 800) || (n.Y + CenterY > 600)) n = null;
				if (n != null){
					visualizationProvider.SetColor(Color.IndianRed);
					visualizationProvider.Line(_cPoint.X, _cPoint.Y, n.X + CenterX, n.Y + CenterY);
					visualizationProvider.SetColor(Color.Chartreuse);
					visualizationProvider.Circle(n.X + CenterX, n.Y + CenterY, BlockH);

				}
			}
			_moveObjNumCurrent = n;// сохраняем для последующего использования
			DrawObjectInfo(visualizationProvider, _moveObjNumCurrent);

			MoveProcess();
			var s = "";
			if (_moveObjNum != null) s += " N " + _moveObjNum.Num;
			if (_moveObjNumCurrent != null) s += " NC " + _moveObjNumCurrent;
			s += " " + _moveStart;
			s += "=>" + _moveCurrent + "";
			s += " mo " + (_moveOperation ? "перемещение" : "нет перемещения");
			s += " mouse " + (_movePressed ? "нажато" : "не нажато");
			visualizationProvider.SetColor(Color.WhiteSmoke);
			visualizationProvider.Print(30, visualizationProvider.CanvasHeight - 75, s);
			_movePressed = false;// для отлова отпускания кнопки мыши

			// выводим круг при перемещении
			if (_moveOperation){// выводим объект в новом месте
				var p = _moveObjNum;
				var x1 = p.X + (_moveCurrent.X - _moveStart.X) + CenterX;
				var y1 = p.Y + (_moveCurrent.Y - _moveStart.Y) + CenterY;
				visualizationProvider.SetColor(Color.OrangeRed);
				visualizationProvider.Circle(x1, y1, 25);
				visualizationProvider.DrawTexturePart(x1, y1, "main", 16, 16, ObjectTypeAtlas.GetTextureNum(p.ObjType));
			}

			visualizationProvider.SetColor(Color.WhiteSmoke);
			s = " номер " + ObjType;
			s += " Центр " + CenterX + " " + CenterY;
			visualizationProvider.Print(70, visualizationProvider.CanvasHeight - 95, s);
			visualizationProvider.DrawTexturePart(30, 600, "main", 16, 16, ObjectTypeAtlas.GetTextureNum(ObjType));
		}

		private SimpleEditableObject SearchNearest(IEnumerable items, int x, int y)
		{
			const int maxdist = 100;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			SimpleEditableObject obj = null;
			foreach (SimpleEditableObject item in items){
				var dist1 = Distance(x, y, item.X, item.Y);
				if (dist1 < dist) { dist = dist1; obj = item; }
			}
			return obj;
		}

		/// <summary>
		/// Обработка раскраски
		/// </summary>
		/// <param name="vp"></param>
		private void DrawObjectInfo(VisualizationProvider vp, SimpleEditableObject viewS)
		{
			//TODO копирование свойств с выделенного объекта
			//TODO попробовать переделать всё на текстуры 32х32
			SimpleEditableObject view = null;
			String sInfo = "";
			if (GetInfoObjectClick){
				// выводим информацию о текущем блоке
				view = _sample;
				sInfo = "Sample";
			}else{
				view = viewS;
				sInfo = "LIVE";
			}
			if (view != null){
				var y = 400;
				vp.SetColor(Color.Brown);
				vp.Print(840, y - 16, sInfo);
				y = DrawObjectInfoOne(vp, y, "Num", view.Num.ToString());
				y = DrawObjectInfoOne(vp, y, "X", view.X.ToString());
				y = DrawObjectInfoOne(vp, y, "Y", view.Y.ToString());
				vp.DrawTexturePart(860, y + 16 / 2 + 3, "main", 16, 16, ObjectTypeAtlas.GetTextureNum(view.ObjType));
				y = DrawObjectInfoOne(vp, y, "Tex", view.ObjType.ToString());

			}
		}

		private static int DrawObjectInfoOne(VisualizationProvider vp, int y, String name, String value)
		{
			vp.SetColor(Color.Chocolate);
			vp.Print(820, y, name);
			vp.SetColor(Color.Wheat);
			vp.Print(880, y, value);
			return y + 16;
		}

		public void SetHandlers()
		{
			if (CanDraw){
				_controller.AddEventHandler("Cursor", CursorMovedEH);
				_controller.AddEventHandler("Keyboard", KeyboardEH);
			}else{
				_controller.RemoveEventHandler("Cursor", CursorMovedEH);
				_controller.RemoveEventHandler("Keyboard", KeyboardEH);
			}
		}

	}
}
