using System;
using System.Threading;
using Microsoft.SPOT;
using GrFamily.ExternalBoard;
using GrFamily.MainBoard;

namespace AccelerometerTest
{
    public class Program
    {
        private Peach _peach;
        private static Accelerometer _accelerometer;

        private Timer _testTimer = null;

        public static void Main()
        {
            var prog = new Program();
            prog.Run();
        }

        private void Run()
        {
            _peach = new Peach();
            _peach.Button.ButtonPressed += Button_ButtonPressed;

            _accelerometer = (new SensorBoard()).Accelerometer;
            _accelerometer.MeasurementRange = Accelerometer.Range.FourG;
            _accelerometer.MeasurementComplete += _accelerometer_MeasurementComplete;
            _accelerometer.Interval = 1000;

            _testTimer = new Timer(PollingMeasure, null, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 1));

            while (true) { }
        }

        private int _testCase = 0;

        private void Button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            _testCase = (_testCase + 1) % 2;

            switch (_testCase)
            {
                case 0:
                    _accelerometer.StopTakingMeasurements();
                    _testTimer.Change(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 1));
                    break;
                default:
                    _testTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _accelerometer.StartTakingMeasurements();
                    break;
            }
        }

        private void PollingMeasure(object state)
        {
            Int16 x;
            Int16 y;
            Int16 z;
            _accelerometer.GetXYZ(out x, out y, out z);

            Debug.Print("Polling : x = " + x.ToString() + ", y = " + y.ToString() + ", z = " + z.ToString());
        }

        private void _accelerometer_MeasurementComplete(Accelerometer sender, Accelerometer.MeasurementCompleteEventArgs e)
        {
            Debug.Print("Event : x = " + e.X.ToString() + ", y = " + e.Y.ToString() + ", z = " + e.Z.ToString());
        }
    }
}
