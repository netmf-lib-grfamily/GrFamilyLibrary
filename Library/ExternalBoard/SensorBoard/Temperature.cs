using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    public delegate void MeasurementCompleteEventHandler(Temperature sender, Temperature.MeasurementCompleteEventArgs e);

    public class Temperature
    {
        public event MeasurementCompleteEventHandler MeasurementComplete;

        private const double Tk = 273;          // âÎ·xÆÛ·xÆÌ·
        private const double T25 = Tk + 25;     // Û25x

        private readonly AnalogInput _temperatureInput;    // T[~X^[ÌüÍ|[g

        private readonly double _bc;            // Bè
        private readonly double _r25;           // 25xÅÌ[×ïRl
        private readonly double _vrd;           // ª³ïRl
        private readonly double _adc;           // ADRo[^[Ìªð\

        private readonly Timer _timer;

        public int Interval { get; set; } = -1;

        public bool IsEnabled { get; set; } = true;

        internal Temperature(Cpu.AnalogChannel channel, double bc, double r25, double vrd, double adc)
        {
            _temperatureInput = new AnalogInput(channel);
            _bc = bc;
            _r25 = r25;
            _vrd = vrd;
            _adc = adc;

            _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);
        }

        public double TakeMeasurement()
        {
            lock (this)
            {
                var raw = _temperatureInput.ReadRaw();
                return 1 / (Math.Log(_vrd * raw / (_adc - raw) / _r25) / _bc + 1 / T25) - Tk;
            }
        }

        private void Measure_Timer(object state)
        {
            if (MeasurementComplete == null)
                return;

            MeasurementComplete(this, new MeasurementCompleteEventArgs { Temperature = TakeMeasurement() });
        }

        public void StartTakingMeasurements()
        {
            if (Interval > 0 && IsEnabled)
            {
                var ts = new TimeSpan(Interval * 10000);
                _timer.Change(ts, ts);
            }
        }

        public void StopTakingMeasurements()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public class MeasurementCompleteEventArgs
        {
            public double Temperature;
        }
    }
}
