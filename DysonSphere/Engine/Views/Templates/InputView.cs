using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Drawing;
using Engine.Controllers;
using Engine.Controllers.Events;
using Engine.Utils;

namespace Engine.Views.Templates
{
	/// <summary>
	/// Элемент ввода данных с клавиатуры
	/// </summary>
	/// <remarks>
	/// для ввода данных нужно кликнуть на строке, это будет сигналом к началу перехвата команд
	/// По идее если пользователь нажал за пределами поля редактирования то это может тоже означать завершение редактирования
	/// </remarks>
	public class InputView : ViewControl
	{
		/// <summary>
		/// Редактируемый текст
		/// </summary>
		public string Text { get; private set; }

		//заменить блокировку на список блокировки
		//брать символы и преобразовывать их, где то уже было такое

		/// <summary>
		/// Блокируем ввод таких же строк, которые уже введены
		/// </summary>
		private Dictionary<string, int> _blockersD = new Dictionary<string, int>();

		/// <summary>
		/// Для блокирования одновременного нажатия многих кнопок
		/// </summary>
		private Dictionary<Keys, StateOne> blockers;

		private StateOneTime _backKey;

		/// <summary>
		/// Сохраняемый текст, если пользователь нажмёт esc он заменит редактируемый пользователем
		/// </summary>
		private string _textOriginal;

		/// <summary>
		/// Признак активирования элемента
		/// </summary>
		private Boolean _active;

		///// <summary>
		///// Текущая позиция курсора
		///// </summary>
		//private int _currentPos = 99;

		/// <summary>
		/// Событие, вызываемое по нажатию enter - завершение редактирования
		/// </summary>
		public String _editSave;

		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="controller"></param>
		public InputView(Controller controller) : base(controller)
		{
			blockers = new Dictionary<Keys, StateOne>();
			blockers.Add(Keys.LButton, StateOne.Init());
			blockers.Add(Keys.Escape, StateOne.Init());
			blockers.Add(Keys.Enter, StateOne.Init());
			_backKey = StateOneTime.Init(5);
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			visualizationProvider.LoadTexture("InputView.EditHead", @"..\Resources\EditHead.png");
		}

		/// <summary>
		/// Активировать строку ввода
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void ActivateInput(object sender, EventArgs e)
		{
			if (sender == this) _active = true;
			else _active = false;
			ModalStart();
		}

		protected override void Keyboard(object sender, InputEventArgs e)
		{
			BlockersDDec();
			var sLButton = GetCheck(e, Keys.LButton);
			// (если нажата кнопка мыши и мышка находится над кнопкой)
			if (sLButton == StatesEnum.On && CursorOver && !_active){
				Controller.StartEvent("InputActivate", this, EventArgs.Empty);
				return;
			}

			if (!_active) return;
			if (!CanDraw) return;

			var endEdit = false;

			var sEsc = GetCheck(e, Keys.Escape);
			var sEnter = GetCheck(e, Keys.Enter);
			var sBack = _backKey.Check(e.IsKeyPressed(Keys.Back));

			// нажата backspace - удаляем лишние символы
			if (sBack == StatesEnum.On){
				if (Text.Length > 0) 
					Text = Text.Remove(Text.Length - 1);
			}

			// нажата отмена - восстанавливаем текст и потом вызываем команду на завершение редактирования
			if (sEsc == StatesEnum.On){
				Text = _textOriginal;
				endEdit = true;
			}

			// нажата мышка за пределами поля редактирования или нажат enter - сохраняем
			if ((sLButton == StatesEnum.On && !CursorOver) || sEnter == StatesEnum.On){
				endEdit = true;
			}

			if (e.IsKeyPressed(Keys.RButton)){
				Text = _textOriginal;
				endEdit = true;
			}

			if (endEdit){
				ModalStop();
				//Controller.StartEvent("InputActivate", null, EventArgs.Empty);
				Controller.AddToOperativeStore(_editSave, this, EventArgs.Empty);
				return;
			}

			var s1 = e.KeyToUnicode();// получаем уникоженную строку
			if (!_blockersD.ContainsKey(s1)){
				Text += s1;
				BlockersDAdd(s1);
			}

		}

		private void BlockersDAdd(string str)
		{
			if (_blockersD.ContainsKey(str))
				_blockersD.Add(str,20);
			_blockersD[str] = 10;
		}

		private void BlockersDDec()
		{
			var k = new List<string>(_blockersD.Keys);
			foreach (var ks in k){
				_blockersD[ks]--;
			}
			BlockersDDelete();
		}

		private void BlockersDDelete()
		{
			var k = new List<string>(_blockersD.Keys);
			foreach (var ks in k){
				if (_blockersD[ks] < 1) _blockersD.Remove(ks);
			}
		}

		/// <summary>
		/// Проверка и изменения состояния кнопки
		/// </summary>
		/// <param name="inputEA"></param>
		/// <param name="key">проверяемая кнопка</param>
		/// <returns></returns>
		private StatesEnum GetCheck(InputEventArgs inputEA, Keys key)
		{
			if (blockers.ContainsKey(key)) return blockers[key].Check(inputEA.IsKeyPressed(key));
			return StatesEnum.Neutral;
		}

		/// <summary>
		/// Инициализация поля ввода
		/// </summary>
		/// <param name="text">Редактируемый текст</param>
		/// <param name="editSave">Событие запускаемое при завершении редактирования</param>
		public void Init(String text, String editSave)
		{
			_active = false;
			Text = text;
			_textOriginal = text;
			_editSave = editSave;
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
			if (_active) visualizationProvider.SetColor(Color.Yellow);
			else visualizationProvider.SetColor(Color.Gray);
			visualizationProvider.Print(X + 10, Y + 10, txt);// текст
			visualizationProvider.DrawTexture(X+128, Y+14, "InputView.EditHead");
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
	}
}
