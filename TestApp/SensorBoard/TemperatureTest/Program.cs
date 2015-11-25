using System;
using System.Threading;
using GrFamily.ExternalBoard;
using GrFamily.MainBoard;
using Microsoft.SPOT;

namespace TemperatureTest
{
    public class Program
    {
        private Peach _peach;
        private Temperature _temperature;

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

            _temperature = (new SensorBoard()).Temperature;
            _temperature.MeasurementComplete += _temperature_MeasurementComplete;
            _temperature.Interval = 1000;

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
                    _temperature.StopTakingMeasurements();
                    _testTimer.Change(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 1));
                    break;
                default:
                    _testTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _temperature.StartTakingMeasurements();
                    break;
            }
        }

        private void PollingMeasure(object state)
        {
            Debug.Print("Polling : " + _temperature.TakeMeasurement().ToString());
        }

        private void _temperature_MeasurementComplete(Temperature sender, Temperature.MeasurementCompleteEventArgs e)
        {
            Debug.Print("Event : " + e.Temperature.ToString());
        }
    }
}
