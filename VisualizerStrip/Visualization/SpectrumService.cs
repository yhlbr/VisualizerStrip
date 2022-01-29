using CSCore;
using CSCore.DSP;
using CSCore.SoundIn;
using CSCore.Streams;
using System;
using System.Timers;
using WinformsVisualization.Visualization;

namespace VisualizerStrip.Visualization
{

    public class SpectrumService
    {
        private WasapiCapture SoundIn;
        private IWaveSource Source;
        private DataSpectrum DataSpectrum;
        private BasicSpectrumProvider SpectrumProvider;
        private Timer Timer;
        private Action<double> Callbacks;

        public SpectrumService()
        {

            //open the default device 
            SoundIn = new WasapiLoopbackCapture();
            //Our loopback capture opens the default render device by default so the following is not needed
            //_soundIn.Device = MMDeviceEnumerator.DefaultAudioEndpoint(DataFlow.Render, Role.Console);
            SoundIn.Initialize();

            var soundInSource = new SoundInSource(SoundIn);

            SetupSampleSource(soundInSource.ToSampleSource());

            // We need to read from our source otherwise SingleBlockRead is never called and our spectrum provider is not populated
            byte[] buffer = new byte[Source.WaveFormat.BytesPerSecond / 2];
            soundInSource.DataAvailable += (s, aEvent) =>
                {
                    int read;
                    while ((read = Source.Read(buffer, 0, buffer.Length)) > 0) ;
                };
            Timer = new Timer();
            Timer.Interval = 20;
            Timer.Elapsed += Timer_Tick;
        }

        public void Start()
        {
            SoundIn.Start();
            Timer.Start();
        }

        public void AddCallback(Action<double> callback)
        {
            Callbacks += callback;
        }

        public void RemoveCallback(Action<double> callback)
        {
            Callbacks -= callback;
        }

        public void Stop()
        {
            Timer.Stop();
            if (SoundIn != null)
            {
                SoundIn.Stop();
                SoundIn.Dispose();
                SoundIn = null;
            }
            if (Source != null)
            {
                Source.Dispose();
                Source = null;
            }
        }

        private void SetupSampleSource(ISampleSource aSampleSource)
        {
            const FftSize fftSize = FftSize.Fft4096;
            //create a spectrum provider which provides fft data based on some input
            SpectrumProvider = new BasicSpectrumProvider(aSampleSource.WaveFormat.Channels,
                aSampleSource.WaveFormat.SampleRate, fftSize);

            DataSpectrum = new DataSpectrum(fftSize)
            {
                SpectrumProvider = SpectrumProvider,
                UseAverage = true,
                BarCount = SignalSmoother.BARS_COUNT,
                IsXLogScale = true,
                ScalingStrategy = ScalingStrategy.Linear
            };

            //the SingleBlockNotificationStream is used to intercept the played samples
            var notificationSource = new SingleBlockNotificationStream(aSampleSource);
            //pass the intercepted samples as input data to the spectrumprovider (which will calculate a fft based on them)
            notificationSource.SingleBlockRead += (s, a) => SpectrumProvider.Add(a.Left, a.Right);

            Source = notificationSource.ToWaveSource(16);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Callbacks == null)
            {
                return;
            }
            Callbacks.Invoke(DataSpectrum.getIntensity(100));
        }
    }
}
