using System;
using System.Collections.Generic;
using Engine.Controllers;
using Engine.Utils.Editor;
using Engine.Views;
using Engine.Views.Templates;
using ZEditorExample.DataObjects;
using Module = Engine.Module;

namespace ZEditorExample
{
	public class EditorExample01: Module
	{
		private View1 _scr;
		private Editor _ed;
		private DataProcessorLayer _l1;
		private DataLineLayer _l2;
		private DataParamNameLayer _l3;
		private DataLinkParamLayer _l4;
		private DataZoomViewLayer _l5;
		private Dictionary<int, DataProcessor> _data = new Dictionary<int, DataProcessor>();
		private Dictionary<int, DataLine> _dataLines = new Dictionary<int, DataLine>();
		private Dictionary<int, DataParamName> _dataParamNames = new Dictionary<int, DataParamName>();
		private Dictionary<int, DataLinkParam> _dataLinkParam = new Dictionary<int, DataLinkParam>();
		private InputView _iv;
		private ViewListEditComponent _vlec;

		protected override void SetUpView(View view, Controller controller)
		{
			Controller.AddEventHandler("exit", Exit1);
			Controller.AddEventHandler("Objs", Objs1);
			Controller.AddEventHandler("Lines", Lines1);
			Controller.AddEventHandler("ParamNames", ParamNames1);
			Controller.AddEventHandler("LinkParam", LinkParam1);
			Controller.AddEventHandler("Zoom", Zoom1);

			// создаём объект визуализации
			_scr = new View1(controller);
			_scr.SetParams(0, 0, 1024, 768, "MainScreen");
			_scr.Show();
			view.AddObject(_scr);

			_ed = new Editor(Controller);
			_ed.Show();
			view.AddObject(_ed);
			_l1 = new DataProcessorLayer(Controller, "objects",_data);
			_ed.AddNewLayer(_l1);
			_l2 = new DataLineLayer(Controller, "lines", _dataLines, _data);
			_ed.AddNewLayer(_l2);
			_l3 = new DataParamNameLayer(Controller, "paramNames", _dataParamNames);
			_ed.AddNewLayer(_l3);
			_l4 = new DataLinkParamLayer(Controller, "linkParam", _dataLinkParam);
			_ed.AddNewLayer(_l4);
			_l5 = new DataZoomViewLayer(Controller, "zoom");
			_l5.CanStore = false;
			_ed.AddNewLayer(_l5);

			_l1.SetDataLayer(_l2);// обмениваем ссылками, что бы каждый из них умел рисовать другие слои
			_l2.SetDataLayer(_l1);// сделать у слоя дополнительный метод - DrawObjectsInBackground - этот метод будет вызываться для прорисовки другого слоя
			_l4.SetDataLayer(_l1, _l2, _l3);
			_l5.SetDataLayer(_l1, _l2, _l4, _l3);

			_ed.SetActiveLayer("zoom");
			_data.Clear();
			
			_ed.Load("zEditorExample");
			_l2.ReSetPoints();
			Controller.AddEventHandler("ZEEStartEdit", (o, args) => ZEEStartEdit(o, args as DataProcessorEventArgs));
			Controller.AddEventHandler("ZEESaveValue1", ZEESaveValue1EH);
			Controller.AddEventHandler("ZEEStartEdit2", (o, args) => ZEEStartEdit2EH(o, args as DataParamNameEventArgs));
			Controller.AddEventHandler("ZEESaveValue2", ZEESaveValue2EH);
			Controller.AddEventHandler("ZEEStartEdit3", (o, args) => ZEEStartEdit3EH(o, args as DataLinkParamEventArgs));
			Controller.AddEventHandler("ZEESaveValue3", ZEESaveValue3EH);
		}

		private void Zoom1(object sender, EventArgs e)
		{
			_ed.SetActiveLayer("zoom");
		}

		private void LinkParam1(object sender, EventArgs e)
		{
			_ed.SetActiveLayer("linkParam");
		}

		private void ParamNames1(object sender, EventArgs e)
		{
			_ed.SetActiveLayer("paramNames");
		}

		private void Lines1(object sender, EventArgs e)
		{
			_ed.SetActiveLayer("lines");
		}

		private void Objs1(object sender, EventArgs e)
		{
			_ed.SetActiveLayer("objects");
		}

		private void Exit1(object sender, EventArgs e)
		{
			_ed.Save("zEditorExample");
			Controller.StartEvent("systemExit", this, EventArgs.Empty);
		}

		private void ZEESaveValue1EH(object sender, EventArgs e)
		{
			// проверяем переменную и удаляем объект редактирования
			_dp.Text = _iv.Text;
			_scr.RemoveControl(_iv);
			_iv.Dispose();
			_iv = null;
			_dp = null;
		}

		private DataProcessor _dp;

		private void ZEEStartEdit(object sender, DataProcessorEventArgs e)
		{
			// вместо _scr можно будет использвать parent - вроде заполнено должно быть
			_dp = e.DataProcessor;
			_iv = new InputView(Controller);
			_iv.SetParams(100, 100, 300, 200, "_iv");
			_iv.Init(_dp.Text, "ZEESaveValue1");
			_iv.ActivateInput(_iv, EventArgs.Empty);// сразу активируем для ввода
			_scr.AddControl(_iv);
		}

		private void ZEESaveValue2EH(object sender, EventArgs e)
		{
			// проверяем переменную и удаляем объект редактирования
			_dppn.ParamName = _iv.Text;
			_scr.RemoveControl(_iv);
			_iv.Dispose();
			_iv = null;
			_dppn = null;
		}

		private DataParamName _dppn;

		private void ZEEStartEdit2EH(object sender, DataParamNameEventArgs e)
		{
			// вместо _scr можно будет использвать parent - вроде заполнено должно быть
			_dppn = e.DataParamName;
			_iv = new InputView(Controller);
			_iv.SetParams(40,e.EditPos-25, 600, 50, "_iv");
			_iv.Init(_dppn.ParamName, "ZEESaveValue2");
			_iv.ActivateInput(_iv, EventArgs.Empty);// сразу активируем для ввода
			_scr.AddControl(_iv);
		}

		private void ZEESaveValue3EH(object sender, EventArgs e)
		{
			// проверяем переменную и удаляем объект редактирования
			_scr.RemoveControl(_vlec);
			_vlec.Dispose();
			_vlec = null;
		}

		private void ZEEStartEdit3EH(object sender, DataLinkParamEventArgs e)
		{
			// вместо _scr можно будет использвать parent - вроде заполнено должно быть
			_vlec = new ViewListEditComponent(Controller, e.NumProcessor, e.DataLinkParamsLayer, e.DataParamNamesLayer, "ZEESaveValue3");
			_vlec.SetParams(40, 40, 700, 500, "_vlec");
			//_vlec.Init(_dppn.ParamName, "ZEESaveValue2");
			_vlec.ActivateInput(_vlec, EventArgs.Empty);// сразу активируем для ввода
			_scr.AddControl(_vlec);
		}

	}

}