using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    public delegate void MeasurementCompleteEventHandler(Temperature sender, Temperature.MeasurementCompleteEventArgs e);

    public class Temperature
    {
        public event MeasurementCompleteEventHandler MeasurementComplete;

        private const double Tk = 273;          // 絶対温度と摂氏温度との差
        private const double T25 = Tk + 25;     // 摂氏25度

        private readonly AnalogInput _temperatureInput;    // サーミスターの入力ポート

        private readonly double _bc;            // B定数
        private readonly double _r25;           // 25度でのゼロ負荷抵抗値
        private readonly double _vrd;           // 分圧抵抗値
        private readonly double _adc;           // ADコンバーターの分解能

        internal Temperature(Cpu.AnalogChannel channel, double bc, double r25, double vrd, double adc)
        {
            _temperatureInput = new AnalogInput(channel);
            _bc = bc;
            _r25 = r25;
            _vrd = vrd;
            _adc = adc;
        }

        public double TakeMeasurement()
        {
            var raw = _temperatureInput.ReadRaw();
            return 1 / (Math.Log(_vrd * raw / (_adc - raw) / _r25) / _bc + 1 / T25) - Tk;
        }

        public TimeSpan MeasurementInterval { get; set; }

        private Thread _measureThread;

        public void StartTakingMeasurements()
        {
            if (MeasurementInterval.Ticks == 0)
                throw new InvalidOperationException("MeasurementInterval must be set before calling StartTakingMeasurements()");

            if (_measureThread != null)
            {
                switch (_measureThread.ThreadState)
                {
                    case ThreadState.Running:
                        return;
                    case ThreadState.Suspended:
                        _measureThread.Resume();
                        return;
                }
            }

            var interval = MeasurementInterval.Days * 24 * 60 * 60 * 1000
                + MeasurementInterval.Hours * 60 * 60 * 1000
                + MeasurementInterval.Minutes * 60 * 1000
                + MeasurementInterval.Seconds * 1000
                + MeasurementInterval.Milliseconds;

            _measureThread = new Thread(() =>
            {
                while (true)
                {
                    if (MeasurementComplete != null)
                        MeasurementComplete(this, new MeasurementCompleteEventArgs { Temperature = TakeMeasurement() });

                    Thread.Sleep(interval);
                }
            });

            _measureThread.Start();
        }

        public void StopTakingMeasurements()
        {
            if (_measureThread != null)
            {
                _measureThread.Abort();
                _measureThread = null;
            }
        }

        public class MeasurementCompleteEventArgs
        {
            public double Temperature;
        }
    }
}
