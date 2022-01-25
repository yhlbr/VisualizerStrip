using System;
using System.ComponentModel;
using VisualizerStrip.Visualization;

namespace WinformsVisualization
{
    public partial class Form : System.Windows.Forms.Form
    {
        private SpectrumService _service;

        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            ValueReadyCallback callback = onCallback;

            _service = new SpectrumService();
            _service.SetCallback(callback);
            _service.Start();
        }

        private void onCallback(double response)
        {
            int intensity = (int)response;
            string intensity_string = new String('=', intensity);
            label.Invoke(new Action(() => { label.Text = intensity_string; }));
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (_service != null)
            {
                _service.Stop();
            }
        }

    }
}
