﻿namespace SettingsEditor
{
	partial class SettingsMainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.cbVisualizationServer = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.cbInput = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbSound = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cbVisualization = new System.Windows.Forms.ComboBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.btnScan = new System.Windows.Forms.Button();
			this.btnEdit = new System.Windows.Forms.Button();
			this.btnNewElem = new System.Windows.Forms.Button();
			this.btnDelElem = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cbRunModule = new System.Windows.Forms.ComboBox();
			this.lblName = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnLoad = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.SuspendLayout();
			// 
			// cbVisualizationServer
			// 
			this.cbVisualizationServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbVisualizationServer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVisualizationServer.FormattingEnabled = true;
			this.cbVisualizationServer.Location = new System.Drawing.Point(398, 64);
			this.cbVisualizationServer.Name = "cbVisualizationServer";
			this.cbVisualizationServer.Size = new System.Drawing.Size(247, 21);
			this.cbVisualizationServer.TabIndex = 52;
			this.cbVisualizationServer.SelectedIndexChanged += new System.EventHandler(this.cbVisualizationServer_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(275, 67);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(124, 13);
			this.label5.TabIndex = 53;
			this.label5.Text = "Визуализация сервера";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(293, 119);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 13);
			this.label4.TabIndex = 51;
			this.label4.Text = "Устройство ввода";
			// 
			// cbInput
			// 
			this.cbInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbInput.FormattingEnabled = true;
			this.cbInput.Location = new System.Drawing.Point(397, 116);
			this.cbInput.Name = "cbInput";
			this.cbInput.Size = new System.Drawing.Size(247, 21);
			this.cbInput.TabIndex = 50;
			this.cbInput.SelectedIndexChanged += new System.EventHandler(this.cbInput_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(358, 92);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 13);
			this.label3.TabIndex = 49;
			this.label3.Text = "Звук";
			// 
			// cbSound
			// 
			this.cbSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbSound.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSound.FormattingEnabled = true;
			this.cbSound.Location = new System.Drawing.Point(397, 89);
			this.cbSound.Name = "cbSound";
			this.cbSound.Size = new System.Drawing.Size(247, 21);
			this.cbSound.TabIndex = 48;
			this.cbSound.SelectedIndexChanged += new System.EventHandler(this.cbSound_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(312, 42);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(79, 13);
			this.label2.TabIndex = 47;
			this.label2.Text = "Визуализация";
			// 
			// cbVisualization
			// 
			this.cbVisualization.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbVisualization.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVisualization.FormattingEnabled = true;
			this.cbVisualization.Location = new System.Drawing.Point(397, 39);
			this.cbVisualization.Name = "cbVisualization";
			this.cbVisualization.Size = new System.Drawing.Size(247, 21);
			this.cbVisualization.TabIndex = 46;
			this.cbVisualization.SelectedIndexChanged += new System.EventHandler(this.cbVisualization_SelectedIndexChanged);
			// 
			// listView1
			// 
			this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
			this.listView1.FullRowSelect = true;
			this.listView1.GridLines = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(13, 141);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(631, 316);
			this.listView1.TabIndex = 45;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Секция";
			this.columnHeader1.Width = 103;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Название";
			this.columnHeader2.Width = 125;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Значение";
			this.columnHeader3.Width = 127;
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Подсказка";
			this.columnHeader4.Width = 202;
			// 
			// btnScan
			// 
			this.btnScan.Location = new System.Drawing.Point(12, 83);
			this.btnScan.Name = "btnScan";
			this.btnScan.Size = new System.Drawing.Size(298, 31);
			this.btnScan.TabIndex = 44;
			this.btnScan.Text = "сканировать расширения";
			this.btnScan.UseVisualStyleBackColor = true;
			this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
			// 
			// btnEdit
			// 
			this.btnEdit.Location = new System.Drawing.Point(93, 54);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(101, 23);
			this.btnEdit.TabIndex = 43;
			this.btnEdit.Text = "Редактировать";
			this.btnEdit.UseVisualStyleBackColor = true;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// btnNewElem
			// 
			this.btnNewElem.Location = new System.Drawing.Point(12, 54);
			this.btnNewElem.Name = "btnNewElem";
			this.btnNewElem.Size = new System.Drawing.Size(75, 23);
			this.btnNewElem.TabIndex = 42;
			this.btnNewElem.Text = "Добавить";
			this.btnNewElem.UseVisualStyleBackColor = true;
			this.btnNewElem.Click += new System.EventHandler(this.btnNewElem_Click);
			// 
			// btnDelElem
			// 
			this.btnDelElem.Enabled = false;
			this.btnDelElem.Location = new System.Drawing.Point(200, 54);
			this.btnDelElem.Name = "btnDelElem";
			this.btnDelElem.Size = new System.Drawing.Size(75, 23);
			this.btnDelElem.TabIndex = 41;
			this.btnDelElem.Text = "Удалить";
			this.btnDelElem.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(274, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(117, 13);
			this.label1.TabIndex = 40;
			this.label1.Text = "Запускаемый модуль";
			// 
			// cbRunModule
			// 
			this.cbRunModule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cbRunModule.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbRunModule.FormattingEnabled = true;
			this.cbRunModule.Location = new System.Drawing.Point(397, 12);
			this.cbRunModule.Name = "cbRunModule";
			this.cbRunModule.Size = new System.Drawing.Size(247, 21);
			this.cbRunModule.TabIndex = 39;
			this.cbRunModule.SelectedIndexChanged += new System.EventHandler(this.cbRunModule_SelectedIndexChanged);
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(12, 38);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(112, 13);
			this.lblName.TabIndex = 38;
			this.lblName.Text = "имя файла настроек";
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(93, 12);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 37;
			this.btnSave.Text = "Сохранить";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnLoad
			// 
			this.btnLoad.Location = new System.Drawing.Point(12, 12);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(75, 23);
			this.btnLoad.TabIndex = 36;
			this.btnLoad.Text = "Загрузить";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Title = "Открыть файл настроек";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Title = "Сохранить настройки";
			// 
			// SettingsMainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(656, 469);
			this.Controls.Add(this.cbVisualizationServer);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cbInput);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cbSound);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbVisualization);
			this.Controls.Add(this.listView1);
			this.Controls.Add(this.btnScan);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.btnNewElem);
			this.Controls.Add(this.btnDelElem);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbRunModule);
			this.Controls.Add(this.lblName);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnLoad);
			this.Name = "SettingsMainForm";
			this.Text = "Настройки";
			this.Load += new System.EventHandler(this.SettingsMainForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cbVisualizationServer;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox cbInput;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox cbSound;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cbVisualization;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.Button btnScan;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Button btnNewElem;
		private System.Windows.Forms.Button btnDelElem;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbRunModule;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnLoad;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
	}
}

