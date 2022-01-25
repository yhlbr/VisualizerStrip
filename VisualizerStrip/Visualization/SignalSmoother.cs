using System;
using System.Collections.Generic;
using System.Linq;
using static WinformsVisualization.Visualization.SpectrumBase;

namespace VisualizerStrip.Visualization
{
    public class SignalSmoother
    {
        public static double getSmoothedValue(SpectrumPointData[] data, double maximum)
        {
            double value = convertValue(data, maximum);
            return applyMovingAverage(value);
        }

        public const int BARS_COUNT = 80;
        private const int HIGHEST_BARS_COUNT = 5;
        private const double GENERAL_MULTIPLIER = 1.75;
        private static double convertValue(SpectrumPointData[] data, double maximum)
        {
            List<double> list = new List<double>();
            for (int index = 0; index < data.Length; index++)
            {
                list.Add(data[index].Value);
            }
            var highest_bars = list.OrderByDescending(w => w).Take(HIGHEST_BARS_COUNT);
            var average = highest_bars.Average();

            average *= GENERAL_MULTIPLIER;
            return Math.Min(Math.Max(average, 0), maximum);
        }


        private const int MOVING_AVERAGE_COUNT = 4;
        private static List<double> lastValues = new List<double>();
        private static double applyMovingAverage(double value)
        {
            lastValues.Add(value);
            if (lastValues.Count > MOVING_AVERAGE_COUNT)
            {
                lastValues.RemoveAt(0);
            }

            return lastValues.Average();
        }
    }
}
