using System;
using System.Collections.Generic;
using System.Drawing;
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
	class DataLineLayer:Layer<DataLine>
	{
		private DataLine _targeted;// выделенный объект на экране
		private DataProcessor _targetedProcessor;// выделенный объект на экране
		private DataProcessor _targetedProcessorStored;// сохраненный объект на экране
		private bool _dragProcess;// перемещение при включенном режиме перемещения 
		private Boolean _shiftPressed;
		private EnumOperation _op=EnumOperation.none;
		private Dictionary<int, DataProcessor> _dataProcessors;
		private DataProcessorLayer _dp;

		public DataLineLayer(Controller controller, string layerName, Dictionary<int, DataLine> data, Dictionary<int, DataProcessor> dataProcessors)
			: base(controller, layerName)
		{
			Data = data;
			_dataProcessors = dataProcessors;
		}

		public void SetDataLayer(DataProcessorLayer dp)
		{
			_dp = dp;
		}

		public override DataLine CreateObject(string objType)
		{
			var o = new DataLine();
			return o;
		}

		public override int AddObject(IDataHolder obj)
		{
			var counter=base.AddObject(obj);
			return counter;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			SetCoordinates(0, 0, 0);
			SetSize(visualizationProvider.CanvasWidth, visualizationProvider.CanvasHeight);
			//visualizationProvider.LoadTexture("ZEEmenu01", @"..\Resources\zEditorExample\menu01.png");
		}

		/// <summary>
		/// Переустановить точки
		/// </summary>
		public void ReSetPoints()
		{
			foreach (var kvline in Data){
				var line = kvline.Value;
				foreach (var kvprocessor in _dataProcessors){
					if (kvprocessor.Key == line.P1Num){// получаем данные
						var proc = kvprocessor.Value;
						line.dp1 = proc;
						break;
					}
				}
				foreach (var kvprocessor in _dataProcessors){
					if (kvprocessor.Key == line.P2Num){// получаем данные
						var proc = kvprocessor.Value;
						line.dp2 = proc;
						break;
					}
				}
			}
			// определяем смещение, которое будет определять параметры безье и т.п.
			foreach (var kvline in Data){
				kvline.Value.InitValuesFromDataProcessors();
			}
		}

		public override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			_dp.DrawObjectInBackground(vp, Editor.MapX, Editor.MapY);
			vp.SetColor(Color.AntiqueWhite);
			vp.Print(900, 380, " M(" + Editor.MapX + "," + Editor.MapY + ")");
			vp.Print(900, 395, " C(" + CursorPoint.X + "," + CursorPoint.Y + ")");
			vp.Print(900, 410, "CF(" + CursorPointFrom.X + "," + CursorPointFrom.Y + ")");
			vp.Print(900, 425, "" + (IsCanStartDrag ? "Перемещение" : "Запрет перемещения"));
			var s = "none";
			if (_op == EnumOperation.SelectPoint2) s = "selP2";
			vp.Print(900, 445, " op= " + s);

			DrawObjectInBackground(vp, Editor.MapX, Editor.MapY);

			if (_targeted != null){
				vp.SetColor(Color.BurlyWood);
				var x1 = _targeted.x1 + Editor.MapX;
				var y1 = _targeted.y1 + Editor.MapY;
				var x2 = _targeted.x2 + Editor.MapX;
				var y2 = _targeted.y2 + Editor.MapY;
				vp.Circle(x1, y1, 38);
				vp.Circle(x2, y2, 38);
				vp.Line(x1, y1, x2, y2);
			}
			if (_targetedProcessor != null){
				vp.SetColor(Color.BurlyWood);
				var x1 = _targetedProcessor.PosX + Editor.MapX;
				var y1 = _targetedProcessor.PosY + Editor.MapY;
				vp.Circle(x1, y1, 38);
			}
			if (_op == EnumOperation.SelectPoint2){
				if ((_targetedProcessorStored != null) && (_targetedProcessor != null) && (_targetedProcessor != _targetedProcessorStored)){
					var x1 = _targetedProcessor.PosX + Editor.MapX;
					var y1 = _targetedProcessor.PosY + Editor.MapY;
					var x2 = _targetedProcessorStored.PosX + Editor.MapX;
					var y2 = _targetedProcessorStored.PosY + Editor.MapY;
					vp.Circle(x1, y1, 38);
					vp.SetColor(Color.LightGreen);
					vp.Circle(x2, y2, 38);
					vp.Line(x1, y1, x2, y2);
				}
			}
		}

		public override void DrawObjectInBackground(VisualizationProvider vp, int mapX, int mapY)
		{
			base.DrawObjectInBackground(vp, mapX, mapY);
			foreach (var d in Data){
				var o = d.Value;
				vp.SetColor(Color.YellowGreen);
				if (_targeted == o) vp.SetColor(Color.Yellow);
				foreach (Point pt in o.Path.Points()){
					vp.Circle(pt.X + mapX, pt.Y + mapY, 1);
				}
				vp.SetColor(Color.OrangeRed);
				foreach (Point pt in o.basePoints){
					vp.Circle(pt.X + mapX, pt.Y + mapY, 6);
				}
				vp.SetColor(Color.Cyan);
				DrawLineAdd1(vp,o.basePoints,mapX,mapY);
				o.Cur++;
				vp.SetColor(Color.YellowGreen);
				if (o.Cur > o.Path.CountPoints-1) o.Cur = 0;
				var ptC = o.Path[o.Cur];
				vp.Circle(ptC.X + mapX, ptC.Y + mapY, 3);
			}
		}

		private void DrawLineAdd1(VisualizationProvider vp, List<Point> points, int mapX, int mapY)
		{
			vp.Line(points[1].X + mapX, points[1].Y + mapY, points[2].X + mapX, points[2].Y + mapY);
		}

		/// <summary>
		/// пересчитываем путь в случае перемещения объекта
		/// </summary>
		/// <param name="dp"></param>
		public void ChangedObject(DataProcessor dp)
		{
			foreach (var line in Data){
				var l = line.Value;
				if (l.dp1 == dp) { l.InitValuesFromDataProcessors(); }
				if (l.dp2 == dp) { l.InitValuesFromDataProcessors(); }
			}
		}
		public override void MouseClick(int x, int y, Boolean dragStarted)
		{
			if (_op == EnumOperation.SelectPoint2){
				// добавляем линию
				if (_targetedProcessor == _targetedProcessorStored) return;
				AddObjectOnLayer(_targetedProcessorStored, _targetedProcessor);
				_op = EnumOperation.none;
				_targetedProcessorStored = null;
				_targetedProcessor = null;
				return;
			}
			if (_op == EnumOperation.none){
				if (_targetedProcessor != null){
					_op = EnumOperation.SelectPoint2;
					_targetedProcessorStored = _targetedProcessor;
				}
			}
			if (_shiftPressed){
				//AddObjectOnLayer(x, y);
			}
		}

		public override void MouseMove(int x, int y)
		{
			if (!_dragProcess){// когда начинается процесс перемещения - перестаём определять перемещение объекта
				var lx = x - Editor.MapX;
				var ly = y - Editor.MapY;
				_targetedProcessor = FindNearestProcessor(lx, ly);
				_targeted = FindNearestLine(lx, ly);
				/*if (_targeted == null) return;
				if (_targetedProcessor == null) return;
				var pt = _targeted.Path[_targeted.Path.CountPoints / 2];
				var dist1 = Distance(lx, ly, pt.X, pt.Y);
				var dist2 = Distance(lx, ly, _targetedProcessor.PosX, _targetedProcessor.PosY);
				if (dist1 < dist2){
					_targetedProcessor = null;_op=EnumOperation.none;
				}
				else{
					_targeted = null;
				}*/
			}
		}

		public override void DragStart(int x, int y)
		{
			if (_targeted == null){// отменяем перемещение
				DragCancel(); _dragProcess = false; return;
			}
			_dragProcess = true;// цель есть - перемещаем её
			//_op = EnumOperation.none;//dragXY;
		}

		public override void DragEnd(int relX, int relY)
		{
			if (_op == EnumOperation.dragXY){
				//_targeted.PosX = RoundX(_targeted.PosX - relX + MapX);
				//_targeted.PosY = RoundY(_targeted.PosY - relY + MapY);
			}
			if (_op==EnumOperation.EditTxt){// запускаем окно редактирования текста
				//Controller.AddToOperativeStore("ZEEStartEdit", this, DataProcessorEventArgs.Send(_targeted));
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

		protected DataProcessor FindNearestProcessor(int x, int y)
		{
			const int maxdist = 100;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			DataProcessor obj = null;
			foreach (var item in _dataProcessors){
				var dist1 = Distance(x, y, item.Value.PosX, item.Value.PosY);
				if (dist1 < dist) { dist = dist1; obj = item.Value; }
			}
			return obj;
		}


		protected DataLine FindNearestLine(int x, int y)
		{
			const int maxdist = 100;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			DataLine obj = null;
			foreach (var item in Data){
				var pt = item.Value.Path[item.Value.Path.CountPoints/2];
				var dist1 = Distance(x, y, pt.X, pt.Y);
				if (dist1 < dist) { dist = dist1; obj = item.Value; }
			}
			return obj;
		}

		/// <summary>
		/// Добавить объект на слой
		/// </summary>
		private void AddObjectOnLayer(DataProcessor dp1, DataProcessor dp2)
		{
			if (dp1 == null) return;
			if (dp2 == null) return;
			var f = SearchData(dp1, dp2);
			if (f != null) return; // есть такой объект - выходим

			var seo = new DataLine();
			seo.dp1 = dp1;
			seo.dp2 = dp2;
			seo.InitValuesFromDataProcessors();
			AddObject(seo);
		}

		/// <summary>
		/// Получить линию по переданным объектам
		/// </summary>
		/// <param name="dp1"></param>
		/// <param name="dp2"></param>
		/// <returns></returns>
		private DataLine SearchData(DataProcessor dp1, DataProcessor dp2)
		{
			DataLine ret = null;
			foreach (var dataLine in Data){
				if (dataLine.Value.dp1 != dp1) continue;
				if (dataLine.Value.dp2 != dp2) continue;
				ret = dataLine.Value;
				break;
			}
			return ret;
		}

		protected override void Keyboard(object sender, InputEventArgs e)
		{
			base.Keyboard(sender, e);
			if (e.IsKeyPressed(Keys.RButton)){_op = EnumOperation.none;
				_targetedProcessor = null;
				_targetedProcessorStored = null;
				return;}
			_shiftPressed = false;
			if (e.IsKeyPressed(Keys.ShiftKey)) _shiftPressed = true;
			if (_targeted != null){
				if (e.IsKeyPressed(Keys.Delete)){// есть цель и нажата кнопка удалить
					var key = -1;
					foreach (var line in Data){
						if (line.Value == _targeted) key = line.Key;
					}
					Data.Remove(key);
					_targeted = null;
				}
			}
		}
	}
}