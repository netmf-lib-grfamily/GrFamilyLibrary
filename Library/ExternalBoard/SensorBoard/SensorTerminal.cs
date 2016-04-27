using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    /// <summary>
    /// �[�q��ɐڑ������Z���T�[�̑���l��Ԃ��f���Q�[�g
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TerminalMeasurementCompleteEventHandler(SensorTerminal sender, SensorTerminal.MeasurementCompleteEventArgs e);

    /// <summary>
    /// �[�q����g�p�����Z���T�[
    /// </summary>
    public class SensorTerminal
    {
        /// <summary>
        /// �[�q��ɐڑ������Z���T�[�ł̑��莞�̃C�x���g�n���h���[
        /// </summary>
        public event TerminalMeasurementCompleteEventHandler MeasurementComplete;

        /// <summary>
        /// �A�i���O���̓s��
        /// </summary>
        private readonly AnalogInput _sensor;

        /// <summary>
        /// �[�q��Z���T�[�̃f�[�^�����I�ɑ��肷�邽�߂̃^�C�}�[
        /// </summary>
        private readonly Timer _timer;

        private int _interval = -1;

        /// <summary>
        /// �R���X�g���N�^�[
        /// </summary>
        /// <param name="channel">�A�i���O�`�����l��</param>
        public SensorTerminal(Cpu.AnalogChannel channel)
        {
            _sensor = new AnalogInput(channel);

            _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// �Z���T�[�f�[�^����̊Ԋu<br />�P�� : �~���b
        /// </summary>
        /// <remarks>���̐����łȂ��ꍇ�̓^�C�}�[�����s���Ȃ�</remarks>
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        /// <summary>
        /// �[�q��ɐڑ������Z���T�[�������I�Ƀf�[�^���擾����
        /// </summary>
        /// <param name="state">���g�p</param>
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

        /// <summary>
        /// �^�C�}�[���N�����āA�[�q��ɐڑ������Z���T�[�������I�Ƀf�[�^�̎擾���n�߂�
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
        /// �[�q��ɐڑ������Z���T�[�������I�Ƀf�[�^���擾���邽�߂̃^�C�}�[���~����
        /// </summary>
        public void StopTakingMeasurements()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// �[�q��ɐڑ������Z���T�[�̐��̑���l���擾����
        /// </summary>
        /// <returns>0�`4095�̒l</returns>
        /// <remarks>GR-PEACh�ł�A/D�ϊ��̕���\��4096</remarks>
        public int ReadRaw()
        {
            lock (this)
            {
                return _sensor.ReadRaw();
            }
        }

        /// <summary>
        /// �[�q��ɐڑ������Z���T�[�̑���l���擾����
        /// </summary>
        /// <returns>0�`1.0�̒l</returns>
        public double Read()
        {
            lock (this)
            {
                return _sensor.Read();
            }
        }


        /// <summary>
        /// �f�[�^�擾�̃C�x���g�n���h���[�ɓn�����Z���T�[�f�[�^�i�[�q��ɐڑ������Z���T�[�j
        /// </summary>
        public class MeasurementCompleteEventArgs
        {
            /// <summary>0�`4095�̒l</summary>
            public int RawValue;
            /// <summary>0�`1.0�̒l</summary>
            public double Value;
        }
    }
}
