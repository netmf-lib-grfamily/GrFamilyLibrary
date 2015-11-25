using System;
using System.Threading;
using Microsoft.SPOT;
using GrFamily.MainBoard;
using GrFamily.ExternalBoard;

namespace SensorTerminalTest
{
    public class Program
    {
        private Peach _peach;
        private SensorTerminal _sensor;

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

            _sensor = (new SensorBoard()).Terminal;
            _sensor.MeasurementComplete += _sensor_MeasurementComplete;
            _sensor.Interval = 1000;

            _testTimer = new Timer(PollingMeasure, null, new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 1));

            while (true) { }
        }

        private int _testCase = 0;

        private void Button_ButtonPressed(GrFamily.MainBoard.Button sender, GrFamily.MainBoard.Button.ButtonState state)
        {
            _testCase = (_testCase + 1) % 2;

            switch (_testCase)
            {
                case 0:
                    _sensor.StopTakingMeasurements();
                    _testTimer.Change(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 1));
                    break;
                default:
                    _testTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    _sensor.StartTakingMeasurements();
                    break;
            }
        }

        private void PollingMeasure(object state)
        {
            Debug.Print("Polling : " + _sensor.ReadRaw().ToString() + ", " + _sensor.Read().ToString());
        }

        private void _sensor_MeasurementComplete(SensorTerminal sender, SensorTerminal.MeasurementCompleteEventArgs e)
        {
            Debug.Print("Event : " + e.RawValue.ToString() + ", " + e.Value.ToString());
        }
    }
}
