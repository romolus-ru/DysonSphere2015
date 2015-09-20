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

namespace ZEditorExample
{
	/// <summary>
	/// Связывание параметров и процессоров
	/// </summary>
	class DataLinkParamLayer:Layer<DataLinkParam>
	{
		private DataProcessor _targetedProcessor;// выделенный объект на экране
		private bool _dragProcess;// перемещение при включенном режиме перемещения 
		private EnumOperation _op=EnumOperation.none;
		private DataProcessorLayer _dp;
		private DataLineLayer _dl;
		private DataParamNameLayer _dn;

		public DataLinkParamLayer(Controller controller, string layerName, Dictionary<int, DataLinkParam> data)
			: base(controller, layerName)
		{
			Data = data;
		}

		public void SetDataLayer(DataProcessorLayer dp, DataLineLayer dl, DataParamNameLayer dn)
		{
			_dp = dp;
			_dl = dl;
			_dn = dn;
		}

		public override DataLinkParam CreateObject(string objType)
		{
			var o = new DataLinkParam();
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

		public override void DrawObject(VisualizationProvider vp)
		{
			base.DrawObject(vp);
			_dp.DrawObjectInBackground(vp, Editor.MapX, Editor.MapY);
			_dl.DrawObjectInBackground(vp, Editor.MapX, Editor.MapY);
			vp.SetColor(Color.AntiqueWhite);
			vp.Print(900, 380, " M(" + Editor.MapX + "," + Editor.MapY + ")");
			vp.Print(900, 395, " C(" + CursorPoint.X + "," + CursorPoint.Y + ")");
			vp.Print(900, 410, "CF(" + CursorPointFrom.X + "," + CursorPointFrom.Y + ")");
			vp.Print(900, 425, "" + (IsCanStartDrag ? "Перемещение" : "Запрет перемещения"));

			if (_targetedProcessor != null){
				vp.SetColor(Color.BurlyWood);
				var x1 = _targetedProcessor.PosX + Editor.MapX+20;
				var y1 = _targetedProcessor.PosY + Editor.MapY-50;
				//vp.Circle(x1, y1, 38);
				var row = 0;
				foreach (var param in Data){
					var lp = param.Value;
					if (lp.NumProcessor != _targetedProcessor.Num) continue;
					vp.Print(x1,y1+row*12,_dn.Data[lp.NumParam].ParamName);
					row++;
				}

			}
		}

		public override void MouseClick(int x, int y, Boolean dragStarted)
		{
			if (_op == EnumOperation.none){
				if (_targetedProcessor != null){
					// запускаем компонент для вывода 
					//_op = EnumOperation.SelectPoint2;
					//_targetedProcessorStored = _targetedProcessor;
				}
			}
		}

		public override void MouseMove(int x, int y)
		{
			if (!_dragProcess){// когда начинается процесс перемещения - перестаём определять перемещение объекта
				var lx = x - Editor.MapX;
				var ly = y - Editor.MapY;
				_targetedProcessor = FindNearestProcessor(lx, ly);
			}
		}

		public override void DragStart(int x, int y)
		{
			if (_targetedProcessor == null){// отменяем перемещение
				DragCancel(); _dragProcess = false; return;
			}
			_dragProcess = true;// цель есть - перемещаем её
			_op = EnumOperation.EditTxt;//dragXY;
		}

		public override void DragEnd(int relX, int relY)
		{
			if (_op == EnumOperation.dragXY){
				//_targeted.PosX = RoundX(_targeted.PosX - relX + MapX);
				//_targeted.PosY = RoundY(_targeted.PosY - relY + MapY);
			}
			if (_op==EnumOperation.EditTxt){// запускаем окно редактирования связанных с объектом параметров
				Controller.AddToOperativeStore("ZEEStartEdit3", this, DataLinkParamEventArgs.Send(_targetedProcessor.Num, this,_dn));
				_targetedProcessor = null;
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
			//_shiftPressed = false;
			//if (e.IsKeyPressed(Keys.ShiftKey)) _shiftPressed = true;
		}
	}
}