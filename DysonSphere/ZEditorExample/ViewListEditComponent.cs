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
using Engine.Utils;
using Engine.Views;
using ZEditorExample.DataObjects;
using ScrollBar = Engine.Views.Templates.ScrollBar;
using Engine.Views.Templates;

namespace ZEditorExample
{
	/// <summary>
	/// Просмотр двух списков и перекидывание между ними элементов
	/// </summary>
	class ViewListEditComponent : ViewDraggable
	{
		#region Переменные

		private const int L1 = 0;
		private const int L2 = 250;
		/// <summary>
		/// Редактируемый текст
		/// </summary>
		public string Text { get; private set; }

		/// <summary>
		/// Признак активирования элемента
		/// </summary>
		private Boolean _active;

		/// <summary>
		/// Событие, вызываемое по нажатию enter - завершение редактирования
		/// </summary>
		public String _editSave;

		private int _mapL1 = 0;// смещение для первого списка
		private int _mapL2 = 0;// смещение для второго списка
		private int _targetL1=-1;
		private int _targetL1Pos;
		private int _targetL2=-1;
		private int _targetL2Pos;
		private bool _dragProcess=false;// перемещение при включенном режиме перемещения 
		private int _numProcessor;
		private ScrollBar _sbv;
		private DataLinkParamLayer _dataLinkParamsLayer;
		private DataParamNameLayer _dataParamNamesLayer;
		/// <summary>
		/// Выбранные элементы
		/// </summary>
		public List<int> SelectedElements=new List<int>();
		/// <summary>
		/// Остальные элементы
		/// </summary>
		public List<int> AllOtherElements=new List<int>();

		#endregion


		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="numProcessor">редактируемый элемент</param>
		/// <param name="dataLinkParamsLayer"></param>
		/// <param name="dataParamNamesLayer"></param>
		public ViewListEditComponent(Controller controller, int numProcessor, DataLinkParamLayer dataLinkParamsLayer, DataParamNameLayer dataParamNamesLayer, string editSave)
			: base(controller)
		{
			_numProcessor = numProcessor;
			_editSave = editSave;
			_dataLinkParamsLayer = dataLinkParamsLayer;
			_dataParamNamesLayer = dataParamNamesLayer;
			FillElements();
		}

		private void _sbv_CurrValueChanged(int newValue)
		{
			_mapL2 = newValue;
		}

		/// <summary>
		/// Заполняем рабочие элементы значениями, разделяя на 2 списка - выбранные элементы и остальные элементы
		/// </summary>
		private void FillElements()
		{
			foreach (var paramName in _dataParamNamesLayer.Data){
				var num = paramName.Key;
				// ищем в связях такой же элемент, связанный с переданным
				var founded = false;
				foreach (var linkParam in _dataLinkParamsLayer.Data){
					var lp = linkParam.Value;
					if (lp.NumProcessor!=_numProcessor)continue;
					if (lp.NumParam != num) continue;
					founded = true;
					break;
				}
				if (founded) SelectedElements.Add(num);
				else AllOtherElements.Add(num);
			}
		}

		/// <summary>
		/// Сохраняем результат
		/// </summary>
		private void StoreElements()
		{
			var lSelectedParams = new List<int>(SelectedElements);
			var lToDel = new List<int>();// список элементов которые отсутствуют в списке
			// удаляем элементы которых уже нету в списке
			foreach (var linkParam in _dataLinkParamsLayer.Data){
				var lp = linkParam.Value;
				if (lp.NumProcessor != _numProcessor) continue;
				var founded = false;
				foreach (var i in lSelectedParams){
					if (i!=lp.NumParam)continue;
					founded = true;
					break;
				}
				if (!founded)lToDel.Add(lp.Num);
			}
			foreach (var i in lToDel){_dataLinkParamsLayer.Data.Remove(i);}
			// добавляем новые элементы
			var la = new List<int>();// список добаляемых новых параметров
			foreach (var numParam in lSelectedParams){
				var founded = false;
				foreach (var linkparam in _dataLinkParamsLayer.Data){
					var lp = linkparam.Value;
					if (lp.NumParam!=numParam)continue;
					if (lp.NumProcessor != _numProcessor) continue;
					founded = true;
					break;
				}
				if (!founded){// не нашли - надо добавлять
					la.Add(numParam);
				}
			}
			foreach (var numNewParam in la){// добавляем
				var add = new DataLinkParam();
				add.NumParam = numNewParam;
				add.NumProcessor = _numProcessor;
				_dataLinkParamsLayer.AddObject(add);
			}
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			visualizationProvider.LoadTexture("InputView.EditHead", @"..\Resources\EditHead.png");
			visualizationProvider.LoadTexture("ViewListEditComponent.Arrows", @"..\Resources\zEditorExample\arrows01.png");

			// пока не работает скроллбар. он не передаёт позицию и не сохраняет/поддерживает предыдущую при изменении параметров скролбара
			_sbv = new ScrollBar(Controller);
			_sbv.SetParams(520, 50, 30, 230, "ScrollBarVertical1");
			//_sbv.SetParams(320, 390, 300, 30, "ScrollBarVertical1");
			this.AddControl(_sbv);
			_sbv.BringToFront();
			_sbv.CurrValueChanged += _sbv_CurrValueChanged;
			_sbv.SetValues(0, AllOtherElements.Count - 1);
		}

		/// <summary>
		/// Активировать редактор
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void ActivateInput(object sender, EventArgs e)
		{
			if (sender == this) _active = true;
			else _active = false;
			ModalStart();
		}

		public override void MouseMove(int x, int y)
		{
			if (!_dragProcess){// когда начинается процесс перемещения - перестаём определять перемещение объекта
				var lx = x-X;
				var ly = y-Y;
				_targetL1 = FindNearestList1(lx, ly - _mapL1);
				_targetL2 = FindNearestList2(lx, ly - _mapL2);
			}
		}

		protected override void Keyboard(object sender, InputEventArgs e)
		{
			var sLButton = e.IsKeyPressed(Keys.LButton);
			// (если нажата кнопка мыши и мышка находится над кнопкой)
			if (sLButton && CursorOver && !_active){
				Controller.StartEvent("InputActivate", this, EventArgs.Empty);
				return;
			}

			if (!_active) return;
			if (!CanDraw) return;
			
			bool endEdit = e.IsKeyPressed(Keys.Escape);

			// нажата мышка за пределами поля редактирования или нажат enter - сохраняем
			if ((sLButton && !CursorOver) || e.IsKeyPressed(Keys.Enter)) endEdit = true;

			if (e.IsKeyPressed(Keys.RButton)) endEdit = true;

			if (endEdit){
				StoreElements();
				ModalStop();
				Controller.AddToOperativeStore(_editSave, this, EventArgs.Empty);
				return;
			}

			if (sLButton){
				if (_targetL1 != -1){
					SelectedElements.Remove(_targetL1);
					AllOtherElements.Add(_targetL1);
				}
				if (_targetL2 != -1){
					AllOtherElements.Remove(_targetL2);
					SelectedElements.Add(_targetL2);
				}
				_targetL1 = -1;
				_targetL2 = -1;
				SelectedElements.Sort();
				AllOtherElements.Sort();
				_sbv.SetValues(0,AllOtherElements.Count-1);
			}

		}

		public override void DrawObject(VisualizationProvider visualizationProvider)
		{
			String txt = Text;
			visualizationProvider.RotateReset();
			visualizationProvider.SetColor(Color.OrangeRed, 25);// фон
			visualizationProvider.Box(X, Y, Width, Height);
			if (CursorOver) visualizationProvider.SetColor(Color.Yellow);
			else visualizationProvider.SetColor(Color.Green);
			visualizationProvider.Rectangle(X, Y, Width, Height);// граница
			visualizationProvider.SetColor(Color.Gray);
			visualizationProvider.Box(450+X,Y+50,50,20);
			//visualizationProvider.Print(X + 10, Y + 10, txt);// текст
			//visualizationProvider.DrawTexturePart(X + 128, Y + 14, "ViewListEditComponent.Arrows", 32, 32, 0);
			
			DrawList(visualizationProvider, SelectedElements, L1, _targetL1, _targetL1Pos);
			DrawList(visualizationProvider, AllOtherElements, L2, _targetL2, _targetL2Pos);
		}

		private void DrawList(VisualizationProvider visualizationProvider, List<int> list, int offsetX, int target, int targetPos)
		{
			var row = 0;
			visualizationProvider.SetColor(Color.White);
			foreach (var element in list){
				row++;
				visualizationProvider.Print(X+offsetX, Y+row*15 + 50, _dataParamNamesLayer.Data[element].ParamName);
			}
			if (target != -1){
				visualizationProvider.SetColor(Color.Orange);
				visualizationProvider.Print(X+offsetX, Y+targetPos-10, _dataParamNamesLayer.Data[target].ParamName);
			}
		}

		protected override void HandlersAdd()
		{
			base.HandlersAdd();
			Controller.AddEventHandler("InputActivate", ActivateInput);
		}

		protected override void HandlersRemove()
		{
			Controller.RemoveEventHandler("InputActivate", ActivateInput);
			base.HandlersRemove();
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

		private int FindNearestList1(int x, int y)
		{
			int obj = -1;
			if (x < L1-10) return obj;
			if (x > L1+200) return obj;
			const int maxdist = 30;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			var row = 0;
			foreach (var item in SelectedElements){
				var pos = row * 15 + 50 + 25;
				var dist1 = Distance(x, y, x, pos - _mapL1);
				if (dist1 < dist) { dist = dist1; obj = item;_targetL1Pos = pos;}
				row++;
			}
			return obj;
		}
		private int FindNearestList2(int x, int y)
		{
			int obj = -1;
			if (x < L2-10) return obj;
			if (x > L2+200) return obj;
			const int maxdist = 30;// максимальная дистанция
			float dist = maxdist;// устанавливаем сразу "максимальную" дальность
			var row = 0;
			foreach (var item in AllOtherElements){
				var pos = row * 15 + 50 + 25;
				var dist1 = Distance(x, y, x, pos - _mapL2);
				if (dist1 < dist) { dist = dist1; obj = item; _targetL2Pos = pos; }
				row++;
			}
			return obj;
		}

	}
}
