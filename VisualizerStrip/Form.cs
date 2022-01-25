using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using WinformsVisualization.Visualization;
using System.Linq;

namespace WinformsVisualization
{
    public partial class Form : System.Windows.Forms.Form
    {
        private WasapiCapture _soundIn;
        private IWaveSource _source;
        private DataSpectrum _dataSpectrum;
        private BasicSpectrumProvider _spectrumProvider;

        public Form()
        {
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            Stop();

            //open the default device 
            _soundIn = new WasapiLoopbackCapture();
            //Our loopback capture opens the default render device by default so the following is not needed
            //_soundIn.Device = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Console);
            _soundIn.Initialize();

            var soundInSource = new SoundInSource(_soundIn);

            SetupSampleSource(soundInSource.ToSampleSource());

            // We need to read from our source otherwise SingleBlockRead is never called and our spectrum provider is not populated
            byte[] buffer = new byte[_source.WaveFormat.BytesPerSecond / 2];
            soundInSource.DataAvailable += (s, aEvent) =>
            {
                int read;
                while ((read = _source.Read(buffer, 0, buffer.Length)) > 0) ;
            };


            //play the audio
            _soundIn.Start();

            timer.Start();
        }

        private void Stop()
        {
            timer.Stop();
            if (_soundIn != null)
            {
                _soundIn.Stop();
                _soundIn.Dispose();
                _soundIn = null;
            }
            if (_source != null)
            {
                _source.Dispose();
                _source = null;
            }
        }

        private void SetupSampleSource(ISampleSource aSampleSource)
        {
            const FftSize fftSize = FftSize.Fft4096;
            //create a spectrum provider which provides fft data based on some input
            _spectrumProvider = new BasicSpectrumProvider(aSampleSource.WaveFormat.Channels,
                aSampleSource.WaveFormat.SampleRate, fftSize);

            _dataSpectrum = new DataSpectrum(fftSize)
            {
                SpectrumProvider = _spectrumProvider,
                UseAverage = true,
                BarCount = 80,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Linear
            };

            //the SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(aSampleSource);
            //pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
            notificationSource.SingleBlockRead += (s, a) => _spectrumProvider.Add(a.Left, a.Right);

            _source = notificationSource.ToWaveSource(16);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Stop();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int intensity = (int)_dataSpectrum.getIntensity(100);
            String intensity_string = new String('=', intensity);
            label.Text = intensity_string;
        }
    }
}
