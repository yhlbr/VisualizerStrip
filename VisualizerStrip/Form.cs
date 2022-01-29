using System;
using System.Windows.Forms;
using VisualizerStrip;

namespace WinformsVisualization
{
    public partial class Form : System.Windows.Forms.Form
    {
        private bool _formClosed = false;

        public Form()
        {
            InitializeComponent();
        }

        private void OnCallback(double response)
        {
            int intensity = (int)response;
            string intensity_string = new string('=', intensity);

            VolumeLabel.Invoke(new Action(() =>
            {
                VolumeLabel.Text = intensity_string;

                if (_formClosed)
                {
                    Close();
                }
            }));
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Program.GetContext().GetSpectrumService().AddCallback(OnCallback);
            WebsocketURL.Text = VisualizerStrip.Properties.Settings.Default.WebsocketURL;
        }

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.GetContext().GetSpectrumService().RemoveCallback(OnCallback);

            // At form closing, only continue after last UI Invoke happened
            // If not, an Invoke on the disposed label happens
            if (_formClosed) return;
            e.Cancel = true;
            _formClosed = true;
        }

        private void WebsocketURL_TextChanged(object sender, EventArgs e)
        {
            VisualizerStrip.Properties.Settings.Default.WebsocketURL = WebsocketURL.Text;
            VisualizerStrip.Properties.Settings.Default.Save();

            WSClient.Instance.UpdateUrl();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            WebsocketURL_TextChanged(null, null);
        }
    }
}
