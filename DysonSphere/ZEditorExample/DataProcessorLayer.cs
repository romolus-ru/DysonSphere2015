using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils.Editor;
using Engine.Utils.Path;
using Engine.Views.Templates;
using ZEditorExample.DataObjects;
using Point = Engine.Utils.Path.Point;

namespace ZEditorExample
{
	/// <summary>
	/// Основной слой
	/// </summary>
	class DataProcessorLayer:Layer<DataProcessor>
	{
		private DataProcessor _targeted;// выделенный объект на экране
		private bool _dragProcess;// перемещение при включенном режиме перемещения 
		private int _x1OldCreated = -9473;
		private int _y1OldCreated = -14748;
		private Boolean _shiftPressed;
		private Boolean _ctrlPressed;
		private ViewLine1 vl1;
		private Path _vb1;
		private EnumOperation _op=EnumOperation.none;
		private DataLineLayer _dl;

		public DataProcessorLayer(Controller controller, string layerName, Dictionary<int, DataProcessor> data)
			: base(controller, layerName)
		{
			Data = data;
			vl1=new ViewLine1(Controller);
			vl1.InitLine();
		}

		public void SetDataLayer(DataLineLayer dl)
		{
			_dl = dl;
		}

		public override DataProcessor CreateObject(string objType)
		{
			var o = new DataProcessor();
			return o;
		}

		public override int AddObject(IDataHolder obj)
		{
			var counter=base.AddObject(obj);
			var o = obj as DataProcessor;
			CreatePath(o.PosX, o.PosY);
			return counter;
		}

		private void CreatePath(int x, int y)
		{
			var pt = vl1.p1[5];
			var dr = 200;
			if (pt.X < x) dr = -dr;
			_vb1 = new Path();
			List<Point> basePoints = new List<Point>();
			basePoints.Add(new Point(x, y));
			basePoints.Add(new Point(x+dr, y));
			basePoints.Add(new Point(pt.X-dr, pt.Y));
			basePoints.Add(pt);
			_vb1.ClearAllPoints();
			_vb1.AddSegment("", "Bezier", basePoints, 30);
			_vb1.GeneratePoints();
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			SetCoordinates(0, 0, 0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
			visualizationProvider.LoadTexture("ZEEmenu01", @"..\Resources\zEditorExample\menu01.png");
		}

		public override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			var mapX = Editor.MapX;
			var mapY = Editor.MapY;
			if (_op == EnumOperation.MapMove)
			{
				mapX -= (CursorPointFrom.X - CursorPoint.X);
				mapY -= (CursorPointFrom.Y - CursorPoint.Y);
			}
			_dl.DrawObjectInBackground(vp, mapX, mapY);
			vp.SetColor(Color.AntiqueWhite);
			vp.Print(900, 380, " M(" + mapX + "," + mapY + ")");
			vp.Print(900, 395, " C(" + CursorPoint.X + "," + CursorPoint.Y + ")");
			vp.Print(900, 410, "CF(" + CursorPointFrom.X + "," + CursorPointFrom.Y + ")");
			vp.Print(900, 425, "" + (IsCanStartDrag ? "Перемещение" : "Запрет перемещения"));

			DrawObjectInBackground(vp, mapX, mapY);

			if (_targeted != null)
			{
				vp.SetColor(Color.BurlyWood);
				var x1 = _targeted.PosX + mapX;
				var y1 = _targeted.PosY + mapY;
				vp.Circle(x1, y1, 38);
				//vp.SetColor(Color.Coral,40);//vp.Box(x1 - 10, y1 - 10, 20, 20);
				//vp.SetColor(Color.CadetBlue, 40);//vp.Box(x1 - 30, y1 - 10, 20, 20);
				//vp.SetColor(Color.DeepSkyBlue, 40);//vp.Box(x1 + 10, y1 - 10, 20, 20);
				vp.DrawTexture(x1, y1+25, "ZEEmenu01");
			}
			if (_dragProcess&&_op==EnumOperation.dragXY){// для перемещения выводим отдельно цель в новых координатах, полупрозрачно
				vp.SetColor(Color.BurlyWood, 50);
				int x1 = _targeted.PosX + mapX - (CursorPointFrom.X - CursorPoint.X);
				int y1 = _targeted.PosY + mapY - (CursorPointFrom.Y - CursorPoint.Y);
				DrawObject(vp, x1, y1, _targeted.Width, _targeted.Height, _targeted);
				vp.Circle(x1, y1, 43);
				CreatePath(x1, y1);
			}
			if (_dragProcess && _op == EnumOperation.dragWH){// для перемещения выводим отдельно цель в новых координатах, полупрозрачно
				vp.SetColor(Color.Chartreuse, 70);
				int x1 = _targeted.PosX + mapX;
				int y1 = _targeted.PosY + mapY;
				int w1 = _targeted.Width - (CursorPointFrom.X - CursorPoint.X);
				int h1 = _targeted.Height - (CursorPointFrom.Y - CursorPoint.Y);
				DrawObject(vp, x1, y1, w1, h1, _targeted);
				vp.Circle(x1, y1, 43);
			}
			vl1.DrawObject(vp);
			if (_vb1 != null){
				vp.SetColor(Color.Chocolate);
				for (int i = 1; i < _vb1.CountPoints; i++)
				{
					var pt1 = _vb1[i - 1];
					var pt2 = _vb1[i];
					vp.Line(pt1.X, pt1.Y, pt2.X, pt2.Y);
				}
			}
		}

		public override void DrawObjectInBackground(VisualizationProvider vp, int mapX, int mapY)
		{
			foreach (var d in Data){
				var o = d.Value;
				int x1 = o.PosX + mapX;
				int y1 = o.PosY + mapY;
				//if (x1 < 0) continue;
				//if (y1 < 0) continue;
				//if (x1 > 800) continue;
				//if (y1 > 600) continue;
				vp.SetColor(Color.White);
				DrawObject(vp, x1, y1, o.Width, o.Height, o);
			}
		}

		private void DrawObject(VisualizationProvider vp, int x1, int y1, int w, int h, DataProcessor o)
		{
			var x = x1-w/2;
			var y = y1-h/2;
			vp.Rectangle(x,y,w,h);
			//var l = vp.TextLength(o.Text);
			vp.Print(x, y, o.Text);
		}

		public override void MouseClick(int x, int y, Boolean dragStarted)
		{
			if (_shiftPressed){
				AddObjectOnLayer(x, y);
			}
		}

		public override void MouseMove(int x, int y)
		{
			if (!_dragProcess){// когда начинается процесс перемещения - перестаём определять перемещение объекта
				_targeted = FindNearest(x - Editor.MapX, y - Editor.MapY);
			}
		}

		public override void DragStart(int x, int y)
		{
			if (_ctrlPressed){
				_op = EnumOperation.MapMove;
				return;
			}
			if (_targeted == null)
			{// отменяем перемещение
				DragCancel(); _dragProcess = false; return;
			}
			_dragProcess = true;// цель есть - перемещаем её
			_op = EnumOperation.none;
			if (x < _targeted.PosX-15+Editor.MapX) _op = EnumOperation.dragXY;
			if (x > _targeted.PosX+15+Editor.MapX) _op = EnumOperation.dragWH;
			if (_op == EnumOperation.none){// в dragEnd запускаем окно редактирования текста при отпускании мышки
				_op = EnumOperation.EditTxt;
			}
		}

		public override void DragEnd(int relX, int relY)
		{
			if (_op == EnumOperation.MapMove){
				Editor.MapX = Editor.MapX - relX;
				Editor.MapY = Editor.MapY - relY;
			}
			if (_op == EnumOperation.dragXY){
				_targeted.PosX = RoundX(_targeted.PosX - relX + Editor.MapX);
				_targeted.PosY = RoundY(_targeted.PosY - relY + Editor.MapY);
				_dl.ChangedObject(_targeted);
			}
			if (_op == EnumOperation.dragWH){
				_targeted.Width = RoundX(_targeted.Width - relX + Editor.MapX);
				_targeted.Height = RoundY(_targeted.Height - relY + Editor.MapY);
			}
			if (_op==EnumOperation.EditTxt){// запускаем окно редактирования текста
				Controller.AddToOperativeStore("ZEEStartEdit", this, DataProcessorEventArgs.Send(_targeted));
			}
			_dragProcess = false;
			_op = EnumOperation.none;
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

		protected DataProcessor FindNearest(int x, int y)
		{
			const int maxdist = 100;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			DataProcessor obj = null;
			foreach (var item in Data){
				var dist1 = Distance(x, y, item.Value.PosX, item.Value.PosY);
				if (dist1 < dist) { dist = dist1; obj = item.Value; }
			}
			return obj;
		}

		/// <summary>Округлить координаты по блокам</summary>
		/// <param name="x"></param>
		/// <returns></returns>
		protected int RoundX(int x) { return x - Editor.MapX; }

		/// <summary>Округлить координаты по блокам</summary>
		/// <param name="y"></param>
		/// <returns></returns>
		protected int RoundY(int y) { return y - Editor.MapY; }

		/// <summary>
		/// Добавить объект на слой
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		private void AddObjectOnLayer(int x, int y)
		{
			var x1 = RoundX(x);
			var y1 = RoundY(y);
			if ((x1 == _x1OldCreated) && (y1 == _y1OldCreated)) return;
			_x1OldCreated = x1;
			_y1OldCreated = y1;
			//следующий этап проверки - что бы по указанным координатам небыло ниодного объекта
			var founded = false;
			foreach (DataProcessor p in Data.Values)
			{
				if (p.PosX == x1) { if (p.PosY == y1) { founded = true; break; } }
			}
			if (!founded){// если такой точки не найдено - добавляем
				var seo = new DataProcessor();
				AddObject(seo);
				seo.PosX = x1;
				seo.PosY = y1;
				seo.Height = 20;
				seo.Width = 20;
				seo.Text = "Текст";
			}
		}

		protected override void Keyboard(object sender, InputEventArgs e)
		{
			base.Keyboard(sender, e);
			_shiftPressed = false;
			if (e.IsKeyPressed(Keys.ShiftKey)) _shiftPressed = true;
			_ctrlPressed = false;
			if (e.IsKeyPressed(Keys.LControlKey)) _ctrlPressed = true;
		}
	}
}
