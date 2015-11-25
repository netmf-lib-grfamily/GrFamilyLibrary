using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    public delegate void TerminalMeasurementCompleteEventHandler(SensorTerminal sender, SensorTerminal.MeasurementCompleteEventArgs e);

    public class SensorTerminal
    {
        public event TerminalMeasurementCompleteEventHandler MeasurementComplete = null;

        private readonly AnalogInput _sensor;

        private readonly Timer _timer;

        public SensorTerminal(Cpu.AnalogChannel channel, double vr)
        {
            _sensor = new AnalogInput(channel);

            _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);
        }

        public int Interval { get; set; } = -1;

        private void Measure_Timer(object state)
        {
            if (MeasurementComplete == null)
                return;

            var result = new MeasurementCompleteEventArgs
            {
                RawValue = _sensor.ReadRaw(),
                Value = _sensor.Read()
            };
            MeasurementComplete(this, result);
        }

        public void StartTakingMeasurements()
        {
            if (Interval > 0)
            {
                var ts = new TimeSpan(Interval * 10000);
                _timer.Change(ts, ts);
            }
        }

        public void StopTakingMeasurements()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public int ReadRaw()
        {
            lock (this)
            {
                return _sensor.ReadRaw();
            }
        }

        public double Read()
        {
            lock (this)
            {
                return _sensor.Read();
            }
        }


        public class MeasurementCompleteEventArgs
        {
            public int RawValue;
            public double Value;
        }
    }
}
