﻿using System;
using System.Windows.Forms;

namespace Flowframes.IO
{
    class ConfigParser
    {

		public enum StringMode { Any, Int, Float }

		public static void SaveGuiElement(TextBox textbox, StringMode stringMode = StringMode.Any)
		{
            switch (stringMode)
            {
				case StringMode.Any: Config.Set(textbox.Name, textbox.Text); break;
				case StringMode.Int: Config.Set(textbox.Name, textbox.Text.GetInt().ToString()); break;
				case StringMode.Float: Config.Set(textbox.Name, textbox.Text.GetFloat().ToString()); break;
			}
		}

		public static void SaveGuiElement(ComboBox comboBox, StringMode stringMode = StringMode.Any)
		{
			switch (stringMode)
			{
				case StringMode.Any: Config.Set(comboBox.Name, comboBox.Text); break;
				case StringMode.Int: Config.Set(comboBox.Name, comboBox.Text.Split('(')[0].GetInt().ToString()); break;
				case StringMode.Float: Config.Set(comboBox.Name, comboBox.Text.GetFloat().ToString().Replace(",", ".")); break;
			}
		}

		public static void SaveGuiElement(CheckBox checkbox)
		{
			Config.Set(checkbox.Name, checkbox.Checked.ToString());
		}

		public static void SaveGuiElement(NumericUpDown upDown, StringMode stringMode = StringMode.Any)
		{
			switch (stringMode)
			{
				case StringMode.Any: Config.Set(upDown.Name, ((float)upDown.Value).ToString().Replace(",", ".")); break;
				case StringMode.Int: Config.Set(upDown.Name, ((int)upDown.Value).ToString()); break;
				case StringMode.Float: Config.Set(upDown.Name, ((float)upDown.Value).ToString().Replace(",", ".")); ; break;
			}
		}

		public static void SaveComboxIndex(ComboBox comboBox)
		{
            Config.Set(comboBox.Name, comboBox.SelectedIndex.Clamp(0, int.MaxValue).ToString());
		}

		public static void LoadGuiElement(ComboBox comboBox, string suffix = "")
		{
			comboBox.Text = Config.Get(comboBox.Name) + suffix;
		}

		public static void LoadGuiElement(TextBox textbox, string suffix = "")
		{
			textbox.Text = Config.Get(textbox.Name) + suffix; ;
		}

		public static void LoadGuiElement(CheckBox checkbox)
		{
			checkbox.Checked = Config.GetBool(checkbox.Name);
		}

		public static void LoadGuiElement(NumericUpDown upDown)
		{
			upDown.Value = Convert.ToDecimal(Config.GetFloat(upDown.Name));
		}

		public static void LoadComboxIndex(ComboBox comboBox)
		{
			comboBox.SelectedIndex = Config.GetInt(comboBox.Name).Clamp(0, comboBox.Items.Count - 1);
		}
	}
}
