using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    public delegate void TemperatureMeasurementCompleteEventHandler(Temperature sender, Temperature.MeasurementCompleteEventArgs e);

    public class Temperature
    {
        public event TemperatureMeasurementCompleteEventHandler MeasurementComplete;

        private const double Tk = 273;          // ��Ή��x�Ɛێ����x�Ƃ̍�
        private const double T25 = Tk + 25;     // �ێ�25�x

        private readonly AnalogInput _temperatureInput;    // �T�[�~�X�^�[�̓��̓|�[�g

        private readonly double _bc;            // B�萔
        private readonly double _r25;           // 25�x�ł̃[�����ג�R�l
        private readonly double _vrd;           // ������R�l
        private readonly double _adc;           // AD�R���o�[�^�[�̕���\

        private readonly Timer _timer;

        public int Interval { get; set; } = -1;

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

        public class MeasurementCompleteEventArgs
        {
            public double Temperature;
        }
    }
}
