using System;
using System.Threading;
using Microsoft.SPOT;
using GrFamily.MainBoard;
using GrFamily.ExternalBoard;

namespace AccelerometerTest
{
    public class Program
    {
        private static Accelerometer _accelerometer;

        public static void Main()
        {
            _accelerometer = (new SensorBoard()).Accelerometer;
            _accelerometer.MeasurementRange = Accelerometer.Range.FourG;

            while (true)
            {
                Int16 x;
                Int16 y;
                Int16 z;

                _accelerometer.Measure();
                _accelerometer.GetXYZ(out x, out y, out z);
                Debug.Print("X = " + x + ", Y = " + y + ", Z = " + z);

                Thread.Sleep(3000);
            }
        }
    }
}
