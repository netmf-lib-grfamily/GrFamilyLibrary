using System;
using System.Threading;
using GrFamily.ExternalBoard;
using Microsoft.SPOT;

namespace TemperatureIntervalTest
{
    public class Program
    {
        private static Temperature _temperature;

        public static void Main()
        {
            _temperature = (new SensorBoard()).Temperature;
            //_temperature.Interval = new TimeSpan(0, 0, 1);
            //_temperature.IsEnabled = true;
            //_temperature.MeasurementComplete += temperature_MeasurementComplete;

            //Debug.Print("StartTakingMeasurements");
            _temperature.StartTakingMeasurements();

            while (true) { }
        }

        private static int _measureCount = 0;

        static void temperature_MeasurementComplete(Temperature sender, Temperature.MeasurementCompleteEventArgs e)
        {
            if (_measureCount >= 30)
            {
                _temperature.StopTakingMeasurements();
                return;
            }

            var temp = e.Temperature;
            Debug.Print(temp.ToString());

            _measureCount++;
        }
    }
}
