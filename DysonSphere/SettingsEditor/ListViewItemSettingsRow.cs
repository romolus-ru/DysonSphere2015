﻿using System.Windows.Forms;
using Engine.Utils.Settings;

namespace SettingsEditor
{
	/// <summary>
	/// Элемент списка со строкой настройки
	/// </summary>
	public class ListViewItemSettingsRow : ListViewItem
	{
		public SettingsRow Row;

		public ListViewItemSettingsRow(SettingsRow row)
		{
			Row = row;
			FillFromRow();
		}

		/// <summary>
		/// Получить элемент с данными
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		public static ListViewItemSettingsRow Get(SettingsRow row)
		{
			var lvi = new ListViewItemSettingsRow(row);
			return lvi;
		}

		/// <summary>
		/// Заполнить элемент данными из переданной строки
		/// </summary>
		public void FillFromRow()
		{
			SubItems.Clear();
			Text = Row.Section;
			SubItems.Add(Row.Name);
			SubItems.Add(Row.Value);
			SubItems.Add(Row.Hint);
		}

		/// <summary>
		/// заполнить переданную строку данными элемента
		/// </summary>
		public void FillFromItem()
		{
			Row.Section = Text;
			Row.Name = SubItems[1].Text;
			Row.Value = SubItems[2].Text;
			Row.Hint = SubItems[3].Text;
		}

	}
}
