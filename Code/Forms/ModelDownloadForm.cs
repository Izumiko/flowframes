using Flowframes.MiscUtils;
using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Windows.Forms;

namespace Flowframes.Forms
{
    public partial class ModelDownloadForm : Form
    {
        public ModelDownloadForm()
        {
            InitializeComponent();
        }

        private void ModelDownloadForm_Load(object sender, EventArgs e)
        {

        }

        public void SetWorking(bool state, bool allowCancel = true)
        {
            Logger.Log($"ModelDownloadForm SetWorking({state})", true);
            SetProgress(-1);
            Control[] controlsToDisable = [downloadModelsBtn];
            Control[] controlsToHide = [closeBtn];
            //progressCircle.Visible = state;

            foreach (Control c in controlsToDisable)
                c.Enabled = !state;

            foreach (Control c in controlsToHide)
                c.Visible = !state;

            Program.busy = state;
            Program.mainForm.UpdateStepByStepControls();
        }

        public void SetProgress(int percent)
        {
            percent = percent.Clamp(0, 100);
            TaskbarManager.Instance.SetProgressValue(percent, 100);
            longProgBar.Value = percent;
            longProgBar.Refresh();
        }

        public void SetStatus(string status)
        {
            statusLabel.Text = status;
        }

        public void SetDownloadBtnEnabled(bool state)
        {
            downloadModelsBtn.Enabled = state;
        }

        private void DownloadModelsBtn_Click(object sender, EventArgs e)
        {
            ModelDownloadFormUtils.form = this;
            bool rifeC = rifeCuda.Checked;
            bool rifeN = rifeNcnn.Checked;
            bool dainN = dainNcnn.Checked;
            bool flavrC = flavrCuda.Checked;
            bool xvfiC = xvfiCuda.Checked;
            ModelDownloadFormUtils.DownloadModels(rifeC, rifeN, dainN, flavrC, xvfiC);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            ModelDownloadFormUtils.Cancel();
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            ModelDownloadFormUtils.Cancel();
            Close();
        }
    }
}
