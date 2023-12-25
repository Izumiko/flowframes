using Flowframes.IO;
using Flowframes.Ui;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Flowframes.Forms
{
    public partial class DebugForm : Form
    {
        public bool configGridChanged;

        public DebugForm()
        {
            InitializeComponent();
        }

        private void DebugForm_Shown(object sender, EventArgs e)
        {
            configDataGrid.Font = new Font("Consolas", 9f);
            RefreshLogs();
        }

        void RefreshLogs ()
        {
            DebugFormHelper.FillLogDropdown(logFilesDropdown);
        }

        private void DebugForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!configGridChanged)
                return;

            DialogResult dialogResult = UiUtils.ShowMessageBox($"Save the modified configuration file?", "Save Configuration?", MessageBoxButtons.YesNo);


            if (dialogResult == DialogResult.Yes)
                DebugFormHelper.SaveGrid(configDataGrid);
        }

        private void ConfigDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
                configGridChanged = true;
        }

        private void ConfigDataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            configGridChanged = true;
        }

        private void ConfigDataGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            configGridChanged = true;
        }

        private void LogFilesDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            DebugFormHelper.RefreshLogBox(logBox, logFilesDropdown.Text);
        }

        private void TextWrapBtn_Click(object sender, EventArgs e)
        {
            logBox.WordWrap = !logBox.WordWrap;
        }

        private void OpenLogFolderBtn_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", Paths.GetLogPath());
        }

        private void ClearLogsBtn_Click(object sender, EventArgs e)
        {
            foreach (string str in logFilesDropdown.Items)
                File.WriteAllText(Path.Combine(Paths.GetLogPath(), str), "");

            LogFilesDropdown_SelectedIndexChanged(null, null);
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            RefreshLogs();
        }

        private void MonospaceBtn_Click(object sender, EventArgs e)
        {
            DebugFormHelper.ToggleMonospace(logBox);
        }

        private void CopyTextClipboardBtn_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(logBox.Text);
        }

        private void TabPage2_Enter(object sender, EventArgs e)
        {
            DebugFormHelper.LoadGrid(configDataGrid);
            configGridChanged = false;
        }
    }
}
