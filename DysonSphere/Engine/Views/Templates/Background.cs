﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Controllers;

namespace Engine.Views.Templates
{
	/// <summary>
	/// Обязательно должен быть первым в списке визуализации, может скрывать всё что под ним окажется
	/// </summary>
	public class Background : ViewControl
	{
		private string _picName;

		private string _picAddress;

		public Background(Controller controller, string picAddress = null, string picName = null)
			: base(controller)
		{// если параметры фона не заданы - берём по умолчанию
			_picAddress = String.IsNullOrEmpty(picAddress) ? @"..\Resources\hider001.jpg" : picAddress;
			_picName = String.IsNullOrEmpty(picName) ? "hider" : picName;
		}

		protected override void InitObject(VisualizationProvider visualizationProvider)
		{
			base.InitObject(visualizationProvider);
			Width = VisualizationProvider.CanvasWidth;
			Height = VisualizationProvider.CanvasHeight;
			visualizationProvider.LoadTexture(_picName, _picAddress);
			Controller.StartEvent("ViewSendToBack", this, EventArgs.Empty);
		}

		protected override void DrawObject(VisualizationProvider visualizationProvider)
		{
			base.DrawObject(visualizationProvider);
			visualizationProvider.SetColor(Color.White, 80);
			visualizationProvider.DrawTexture(Width / 2, Height / 2, _picName);
		}
	}
}
