﻿using System;

namespace Engine.Utils
{
	public class StateOneTime : StateOne
	{
		/// <summary>
		/// Счётчик
		/// </summary>
		private int _counter;

		/// <summary>
		/// Начальное значение
		/// </summary>
		private int _counterInit;

		/// <summary>
		/// Проверяем нажатие
		/// </summary>
		/// <param name="isPressed">Нажата ли нужная кнопка</param>
		/// <returns>Если состояние изменилось = true</returns>
		/// <remarks>Если кнопка нажата - состояние переключаем на "нажато" и ждём пока кнопку отпустят
		/// Если кнопка отжата - состояние переключаем на отпущено и ждём пока опять нажмут</remarks>
		public override StatesEnum Check(Boolean isPressed)
		{
			var res = base.Check(isPressed);
			if (CurrentState == StatesEnum.On){// как только кнопку нажали - считаем 
				_counter--;
				if (_counter < 0){
					_counter = _counterInit;// устанавливаем счётчик обратно
					CurrentState = StatesEnum.Off;// переключаем состояние на сброшено и сигналим о том что состояние изменилось
					res = CurrentState;
					if (StateOff != null) StateOff(this, EventArgs.Empty);
				}
			}
			return res;
		}

		/// <summary>
		/// Обработчики и пауза
		/// </summary>
		/// <param name="stateOn"></param>
		/// <param name="stateOff"></param>
		/// <param name="counterInit"></param>
		/// <returns></returns>
		public static StateOneTime Init(EventHandler stateOn, EventHandler stateOff, int counterInit)
		{
			var s = new StateOneTime();
			s._counter = counterInit;
			s._counterInit = counterInit;
			s.StateOn = stateOn;
			s.StateOff = stateOff;
			return s;
		}

		/// <summary>
		/// Без обработчиков, используем только паузу
		/// </summary>
		/// <param name="counterInit"></param>
		/// <returns></returns>
		public static StateOneTime Init(int counterInit)
		{
			return Init(null, null, counterInit);
		}

	}
}
