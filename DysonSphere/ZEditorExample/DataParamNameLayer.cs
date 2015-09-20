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

namespace ZEditorExample
{
	/// <summary>
	/// Вывод и редактирование названий параметров объектов
	/// </summary>
	class DataParamNameLayer : Layer<DataParamName>
	{
		private int _map1 = 0;
		//private int _map1a = 0;
		public DataParamNameLayer(Controller controller, string layerName,Dictionary<int, DataParamName> data) : base(controller, layerName)
		{			Data = data;
			AddB(Controller,1,"AddNewNameParam","Добавить имя нового параметра");
			Controller.AddEventHandler("AddNewNameParam", AddNewNameParamEH);
		}

		private void AddNewNameParamEH(object sender, EventArgs e)
		{
			// добавить новый параметр
			AddObjectOnLayer("New param name");
		}

		private void AddB(Controller controller,int i, string eventName, string caption)
		{
			Button a = Button.CreateButton(controller, 10, i * 30 - 20, 190, 20, eventName, caption, "", Keys.None, "btnAddNewNameParam");
			AddControl(a);
		}

		private DataParamName _targeted;// выделенный объект на экране
		private int _targetedPos = 0;
		private bool _dragProcess;// перемещение при включенном режиме перемещения 
		private EnumOperation _op=EnumOperation.none;

		public override DataParamName CreateObject(string objType)
		{
			var o = new DataParamName();
			return o;
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
			vp.SetColor(Color.AntiqueWhite);
			int row = 0;
			var mp1 = _map1;
			if (_dragProcess) mp1 = mp1 - (CursorPointFrom.Y - CursorPoint.Y);
			foreach (var d in Data){
				var o = d.Value;
				if (_targeted!=o)vp.SetColor(Color.YellowGreen);
				else vp.SetColor(Color.Chartreuse);
				vp.Print(50, row*15 + 50 + mp1, o.ParamName);
				row++;
			}
		}
		
		public override void DrawObjectInBackground(VisualizationProvider vp, int mapX, int mapY){
			// ничего не выводим, параметры будут выводиться связывающим слоем
		}

		public override void MouseClick(int x, int y, Boolean dragStarted)
		{
			if (_op == EnumOperation.none){}
		}

		public override void MouseMove(int x, int y)
		{
			if (!_dragProcess){// когда начинается процесс перемещения - перестаём определять перемещение объекта
				var lx = x;
				var ly = y - _map1;
				_targetedPos = 0;
				_targeted = FindNearestParamName(lx, ly);
			}
		}

		public override void DragStart(int x, int y)
		{
			if (_targeted == null){// отменяем перемещение
				//DragCancel(); _dragProcess = false;
				_dragProcess = true;
				_op = EnumOperation.dragXY;
				return;
			}
			_dragProcess = true;// цель есть - перемещаем её
			//if (_op == EnumOperation.none){// в dragEnd запускаем окно редактирования текста при отпускании мышки
				_op = EnumOperation.EditTxt;
			//}
			//_op = EnumOperation.none;//dragXY;
		}

		public override void DragEnd(int relX, int relY)
		{
			if (_op==EnumOperation.EditTxt){// запускаем окно редактирования текста
				Controller.AddToOperativeStore("ZEEStartEdit2", this, DataParamNameEventArgs.Send(_targeted, _targetedPos));
				_targeted = null;// таким образом не начинается снова редактирование
				_targetedPos = 0;
			}
			if (_op == EnumOperation.dragXY){
				_map1 -= relY;
			}
			_dragProcess = false;
			_op = EnumOperation.none;
		}

		protected DataParamName FindNearestParamName(int x, int y)
		{
			if (x < 40) return null;
			if (x > 400) return null;
			const int maxdist = 30;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			DataParamName obj = null;
			var row = 0;
			foreach (var item in Data){
				var pos = row*15 + 50 + 15;
				var dist1 = Editor.Distance(x, y, x, row*15 + 50 + 15 - _map1);
				if (dist1 < dist){dist = dist1;obj = item.Value;_targetedPos = pos;}
				row++;
			}
			return obj;
		}

		/// <summary>
		/// Добавить объект на слой
		/// </summary>
		private void AddObjectOnLayer(string name)
		{
			var seo = new DataParamName();
			seo.ParamName = name;
			AddObject(seo);
		}

		protected override void Keyboard(object sender, InputEventArgs e)
		{
			base.Keyboard(sender, e);
			if (e.IsKeyPressed(Keys.RButton)){_op = EnumOperation.none;
				return;}
			//if (_targeted != null){
			//	if (e.IsKeyPressed(Keys.Delete)){// есть цель и нажата кнопка удалить
			//		var key = -1;
			//		foreach (var line in Data){
			//			if (line.Value == _targeted) key = line.Key;
			//		}
			//		Data.Remove(key);
			//		_targeted = null;
			//	}
			//}
		}

	}
}
