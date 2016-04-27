using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    /// <summary>
    /// ���x�Z���T�[�i�T�[�~�X�^�[�j�̑���l��Ԃ��f���Q�[�g
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TemperatureMeasurementCompleteEventHandler(Temperature sender, Temperature.MeasurementCompleteEventArgs e);

    /// <summary>
    /// ���x�Z���T�[�i�T�[�~�X�^�[�j
    /// </summary>
    public class Temperature
    {
        /// <summary>
        /// ���x���莞�ɃC�x���g�n���h���[��ʂ��ĕԋp����Z���T�[�f�[�^
        /// </summary>
        public event TemperatureMeasurementCompleteEventHandler MeasurementComplete;

        /// <summary>�ێ�0�x�̐�Ή��x�ł̒l</summary>
        private const double Tk = 273;
        /// <summary>�ێ�25�x�̐�Ή��x�ł̒l</summary>
        private const double T25 = Tk + 25; 

        /// <summary>�T�[�~�X�^�[�̓��̓|�[�g</summary>
        private readonly AnalogInput _temperatureInput;

        /// <summary>�T�[�~�X�^�[��B�萔</summary>
        private readonly double _bc;
        /// <summary>�ێ�25�x�ł̃[�����ג�R�l</summary>
        private readonly double _r25;
        /// <summary>������R�l</summary>
        private readonly double _vrd;
        /// <summary>AD�R���o�[�^�[�̕���\</summary>
        private readonly double _adc;

        private readonly Timer _timer;

        /// <summary>
        /// �Z���T�[�f�[�^����̊Ԋu<br />�P�� : �~���b
        /// </summary>
        /// <remarks>���̐����łȂ��ꍇ�̓^�C�}�[�����s���Ȃ�</remarks>
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }
        private int _interval = -1;

        /// <summary>
        /// �R���X�g���N�^�[
        /// </summary>
        /// <param name="channel">�T�[�~�X�^�[��ڑ�����A�i���O�`�����l��</param>
        /// <param name="bc">�T�[�~�X�^�[��B�萔�l</param>
        /// <param name="r25">�ێ�25�x�̃[�����ג�R�l</param>
        /// <param name="vrd">���������R�l</param>
        /// <param name="adc">A/D�ϊ��̕���\</param>
        internal Temperature(Cpu.AnalogChannel channel, double bc, double r25, double vrd, double adc)
        {
            _temperatureInput = new AnalogInput(channel);
            _bc = bc;
            _r25 = r25;
            _vrd = vrd;
            _adc = adc;

            _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// ���x�𑪒肷��
        /// </summary>
        /// <returns>���x�i�ێ��j</returns>
        public double TakeMeasurement()
        {
            lock (this)
            {
                var raw = _temperatureInput.ReadRaw();
                return 1 / (Math.Log(_vrd * raw / (_adc - raw) / _r25) / _bc + 1 / T25) - Tk;
            }
        }

        /// <summary>
        /// ����I�ɉ��x���擾����
        /// </summary>
        /// <param name="state">���g�p</param>
        private void Measure_Timer(object state)
        {
            if (MeasurementComplete == null)
                return;

            MeasurementComplete(this, new MeasurementCompleteEventArgs { Temperature = TakeMeasurement() });
        }

        /// <summary>
        /// �^�C�}�[���N�����āA����I�ɉ��x�̎擾���n�߂�
        /// </summary>
        public void StartTakingMeasurements()
        {
            if (Interval > 0)
            {
                var ts = new TimeSpan(Interval * 10000);
                _timer.Change(ts, ts);
            }
        }

        /// <summary>
        /// ���x�����I�Ɏ擾���邽�߂̃^�C�}�[���~����
        /// </summary>
        public void StopTakingMeasurements()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// �f�[�^�擾�̃C�x���g�n���h���[�ɓn����鉷�x�f�[�^
        /// </summary>
        public class MeasurementCompleteEventArgs
        {
            /// <summary>���x</summary>
            public double Temperature;
        }
    }
}
