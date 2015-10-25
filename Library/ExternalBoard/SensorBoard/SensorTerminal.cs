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

        private Timer _timer = null;

        public SensorTerminal(Cpu.AnalogChannel channel, double vr)
        {
            _sensor = new AnalogInput(channel);
        }

        private int _interval = 0;

        public int Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                SetTimer();
            }
        }

        private bool _enabled = false;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SetTimer();
            }
        }

        private void SetTimer()
        {
            if (_timer == null)
                _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);

            if (_interval > 0 && _enabled)
                _timer.Change(new TimeSpan(0, 0, 0, 0, _interval), new TimeSpan(0, 0, 0, 0, _interval));
            else
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void Measure_Timer(object state)
        {
            if (MeasurementComplete == null) return;

            var result = new MeasurementCompleteEventArgs
            {
                RawValue = _sensor.ReadRaw(),
                Value = _sensor.Read()
            };
            MeasurementComplete(this, result);
        }

        public int ReadRaw()
        {
            return _sensor.ReadRaw();
        }

        public double Read()
        {
            return _sensor.Read();
        }


        public class MeasurementCompleteEventArgs
        {
            public int RawValue;
            public double Value;
        }
    }
}
