using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Engine;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils.Editor;
using ZEditorExample.DataObjects;
using Button = Engine.Views.Templates.Button;
using Point = Engine.Utils.Path.Point;

namespace ZEditorExample
{
	/// <summary>
	/// Просмотр с изменением масштаба. Но потом лучше масштаб перенести
	/// </summary>
	class DataZoomViewLayer:Layer<DataLinkParam>
	{
		private DataProcessor _targetedProcessor;// выделенный объект на экране
		private bool _dragProcess;// перемещение при включенном режиме перемещения 
		private EnumOperation _op=EnumOperation.none;
		private DataProcessorLayer _dp;
		private DataLineLayer _dl;
		private DataLinkParamLayer _dln;
		private DataParamNameLayer _dn;
		private int _zoom1 = 1;
		private string s1tmp = "";

		public DataZoomViewLayer(Controller controller, string layerName)
			: base(controller, layerName)
		{
			AddB(Controller,1,"ZoomIn","Приблизить");
			Controller.AddEventHandler("ZoomIn", ZoomInEH);
			AddB(Controller, 2, "ZoomOut", "Отдалить");
			Controller.AddEventHandler("ZoomOut", ZoomOutEH);
			AddB(Controller, 3, "ResetZoom", "Переустановить");
			Controller.AddEventHandler("ResetZoom", ResetZoomEH);
		}

		public void SetDataLayer(DataProcessorLayer dp, DataLineLayer dl, DataLinkParamLayer dln,DataParamNameLayer dn)
		{
			_dp = dp;
			_dl = dl;
			_dln = dln;
			_dn = dn;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			SetCoordinates(0, 0, 0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
			//visualizationProvider.LoadTexture("ZEEmenu01", @"..\Resources\zEditorExample\menu01.png");
		}

		public override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			var mapX = Editor.MapX;
			var mapY = Editor.MapY;
			if (_op == EnumOperation.MapMove){
				mapX -= (CursorPointFrom.X - CursorPoint.X);
				mapY -= (CursorPointFrom.Y - CursorPoint.Y);
			}

			vp.SetColor(Color.White);
			vp.Print(900, 365, " map (" + Editor.MapX + "," + Editor.MapY + ")");
			vp.Print(900, 380, " Zoom (" + _zoom1 + ")");
			vp.Print(400, 395, " s1 (" + s1tmp + ")");

			Draw1(vp, mapX, mapY, _zoom1);
			Draw2(vp, mapX, mapY, _zoom1);

		}


		private void Draw1(VisualizationProvider vp, int mapX, int mapY, int zoom1)
		{
			foreach (var d in _dp.Data){
				var o = d.Value;
				int x1 = o.PosX/zoom1 + mapX;
				int y1 = o.PosY/zoom1 + mapY;
				vp.SetColor(Color.White);
				DrawObject(vp, x1, y1, o.Width, o.Height, o,zoom1);
			}
			if (_targetedProcessor != null){
				int x1 = _targetedProcessor.PosX/zoom1 + mapX;
				int y1 = _targetedProcessor.PosY/zoom1 + mapY;
				vp.SetColor(Color.White);
				vp.Circle(x1, y1, 15);

				var row = 0;
				foreach (var links in _dln.Data){
					var l = links.Value;// ищем нужный процессор
					// неоптимально, желательно получить список при выделении цели, но у словарей доступ всё равно быстрый
					if (l.NumProcessor != _targetedProcessor.Num) continue;
					var s = _dn.Data[l.NumParam].ParamName;
					vp.Print(x1 + 20, y1+row*15 - 50, s);
					row++;
				}
			}

		}

		private void DrawObject(VisualizationProvider vp, int x1, int y1, int w, int h, DataProcessor o, int zoom1)
		{
			var x = x1 - w / 2/zoom1;
			var y = y1 - h / 2/zoom1;
			vp.Rectangle(x, y, w/zoom1, h/zoom1);
			vp.Print(x-15+15/zoom1, y-15+15/zoom1, o.Text);
		}

		private void Draw2(VisualizationProvider vp, int mapX, int mapY, int zoom1)
		{
			base.DrawObjectInBackground(vp, mapX, mapY);
			foreach (var d in _dl.Data)
			{
				var o = d.Value;
				vp.SetColor(Color.YellowGreen);
				foreach (Point pt in o.Path.Points())
				{
					vp.Circle(pt.X/zoom1 + mapX, pt.Y/zoom1 + mapY, 1);
				}
				vp.SetColor(Color.OrangeRed);
				foreach (var pt in o.basePoints)
				{
					vp.Circle(pt.X/zoom1 + mapX, pt.Y/zoom1 + mapY, 6/zoom1);
				}
				vp.SetColor(Color.Cyan);
				DrawLineAdd1(vp, o.basePoints, mapX, mapY,zoom1);
				o.Cur++;
				vp.SetColor(Color.YellowGreen);
				if (o.Cur > o.Path.CountPoints - 1) o.Cur = 0;
				var ptC = o.Path[o.Cur];
				vp.Circle(ptC.X/zoom1 + mapX, ptC.Y/zoom1 + mapY, 3);
			}
		}

		private void DrawLineAdd1(VisualizationProvider vp, List<Point> points, int mapX, int mapY, int zoom1)
		{
			vp.Line(points[1].X/zoom1 + mapX, points[1].Y/zoom1 + mapY, points[2].X/zoom1 + mapX, points[2].Y/zoom1 + mapY);
		}

		public override void MouseMove(int x, int y)
		{
			if (!_dragProcess){// когда начинается процесс перемещения - перестаём определять перемещение объекта
				var lx = (x - Editor.MapX)*_zoom1;
				var ly = (y - Editor.MapY)*_zoom1;
				_targetedProcessor = FindNearestProcessor(lx, ly);
			}
		}

		public override void DragStart(int x, int y)
		{
			//if (_targetedProcessor == null){// отменяем перемещение
			//	DragCancel(); _dragProcess = false; return;
			//}
			_dragProcess = true;// цель есть - перемещаем её
			_op = EnumOperation.MapMove;
		}

		public override void DragEnd(int relX, int relY)
		{
			if (_op == EnumOperation.MapMove){
				Editor.MapX = Editor.MapX - relX;
				Editor.MapY = Editor.MapY - relY;
			} 
			_dragProcess = false;
			_op = EnumOperation.none;
		}

		protected DataProcessor FindNearestProcessor(int x, int y)
		{
			const int maxdist = 100;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			DataProcessor obj = null;
			foreach (var item in _dp.Data){
				var dist1 = Editor.Distance(x, y, item.Value.PosX, item.Value.PosY);
				if (dist1 < dist) { dist = dist1; obj = item.Value; }
			}
			return obj;
		}

		protected override void Keyboard(object sender, InputEventArgs e)
		{
			base.Keyboard(sender, e);
			if (e.IsKeyPressed(Keys.RButton)){_op = EnumOperation.none;
				_targetedProcessor = null;
				return;}
		}

		private void ResetZoomEH(object sender, EventArgs e)
		{
			_zoom1 = 1;
			Editor.MapX = 0;
			Editor.MapY = 0;
		}

		private void ZoomInEH(object sender, EventArgs e)
		{
			_zoom1--;
			int dx = 0;
			int dy = 0;
			if (_zoom1 < 1) _zoom1 = 1;
			else{
				int w2 = 800 / 2;
				int h2 = 600 / 2;
				dx = (w2/(_zoom1) - w2/(_zoom1 + 1));
				dy = (h2/(_zoom1) - h2/(_zoom1 + 1));
				Editor.MapX -= dx;
				Editor.MapY -= dy;
			}
			s1tmp = "-- " + dx + " " + dy + " (" + Editor.MapX + "," + Editor.MapY + ")";
		}

		private void ZoomOutEH(object sender, EventArgs e)
		{
			_zoom1++;
			int dx = 0;
			int dy = 0;
			if (_zoom1 > 10) _zoom1 = 10;
			else{
				int w2 = 800/2;
				int h2 = 600/2;
				dx = (w2/(_zoom1 - 1) - w2/_zoom1);
				dy = (h2/(_zoom1 - 1) - h2/_zoom1);
				Editor.MapX += dx;
				Editor.MapY += dy;
			}
			s1tmp = "++ " + dx + " " + dy + " (" + Editor.MapX + "," + Editor.MapY + ")";
		}

		private void AddB(Controller controller,int i, string eventName, string caption)
		{
			Button a = Button.CreateButton(controller, 10, i * 30 - 20, 190, 20, eventName, caption, "", Keys.None, "btnZoom"+i.ToString());
			AddControl(a);
		}
	}
}