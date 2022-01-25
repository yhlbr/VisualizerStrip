using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using System;
using System.Timers;
using WinformsVisualization.Visualization;

namespace VisualizerStrip.Visualization
{
    public delegate void ValueReadyCallback(double result);

    public class SpectrumService
    {
        private WasapiCapture _soundIn;
        private IWaveSource _source;
        private DataSpectrum _dataSpectrum;
        private BasicSpectrumProvider _spectrumProvider;
        private Timer timer;
        private ValueReadyCallback callback;

        public SpectrumService()
        {

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
            timer = new Timer();
            timer.Interval = 30;
            timer.Elapsed += timer_Tick;
        }

        public void Start()
        {
            //play the audio
            _soundIn.Start();
            timer.Start();
        }

        public void SetCallback(ValueReadyCallback callback)
        {
            this.callback = callback;
        }


        public void Stop()
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
                BarCount = SignalSmoother.BARS_COUNT,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Linear
            };

            //the SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(aSampleSource);
            //pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
            notificationSource.SingleBlockRead += (s, a) => _spectrumProvider.Add(a.Left, a.Right);

            _source = notificationSource.ToWaveSource(16);
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            this.callback(_dataSpectrum.getIntensity(100));
        }
    }
}
