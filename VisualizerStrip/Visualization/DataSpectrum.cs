using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using CSCore.DSP;
using VisualizerStrip.Visualization;

namespace WinformsVisualization.Visualization
{
    public class DataSpectrum : SpectrumBase
    {
        private int _barCount;
        private Size _currentSize;

        public DataSpectrum(FftSize fftSize)
        {
            FftSize = fftSize;
        }

        public int BarCount
        {
            get { return _barCount; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value");
                _barCount = value;
                SpectrumResolution = value;
                UpdateFrequencyMapping();

                RaisePropertyChanged("BarCount");
                RaisePropertyChanged("BarWidth");
            }
        }

        [BrowsableAttribute(false)]
        public Size CurrentSize
        {
            get { return _currentSize; }
            protected set
            {
                _currentSize = value;
                RaisePropertyChanged("CurrentSize");
            }
        }

        public SpectrumPointData[] GetSpectrumData(int maxValue)
        {
            var fftBuffer = new float[(int)FftSize];

            //get the fft result from the spectrum provider
            if (SpectrumProvider.GetFftData(fftBuffer, this))
            {
                //prepare the fft result for rendering 
                return CalculateSpectrumPoints(maxValue, fftBuffer);
            }
            return null;
        }




        public double getIntensity(int maxValue)
        {
            var data = GetSpectrumData(maxValue);
            if (data == null)
            {
                return 0;
            }
            return SignalSmoother.getSmoothedValue(data, maxValue);
        }
    }
}