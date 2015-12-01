using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.Module
{
    public delegate void AnalogMeasurementCompleteEventHandler(AnalogSensor sender, AnalogSensor.MeasurementCompleteEventArgs e);

    public class AnalogSensor
    {
        public event AnalogMeasurementCompleteEventHandler MeasurementComplete = null;

        private readonly AnalogInput _sensor;

        private Timer _timer = null;

        public AnalogSensor(Cpu.AnalogChannel channel)
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
                _timer.Change(new TimeSpan(_interval * 10000), new TimeSpan(_interval * 10000));
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
