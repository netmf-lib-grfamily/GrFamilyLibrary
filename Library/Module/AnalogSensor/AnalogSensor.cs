using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.Module
{
    /// <summary>
    /// �A�i���O���̓Z���T�[�̑���l��Ԃ��f���Q�[�g
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AnalogMeasurementCompleteEventHandler(AnalogSensor sender, AnalogSensor.MeasurementCompleteEventArgs e);

    /// <summary>
    /// �A�i���O���̓Z���T�[
    /// </summary>
    public class AnalogSensor
    {
        /// <summary>
        /// �A�i���O���̓Z���T�[�̃Z���T�[�ł̑��莞�̃C�x���g�n���h���[
        /// </summary>
        public event AnalogMeasurementCompleteEventHandler MeasurementComplete = null;

        /// <summary>
        /// �A�i���O���̓|�[�g
        /// </summary>
        private readonly AnalogInput _sensor;

        /// <summary>
        /// �A�i���O���̓Z���T�[�̃f�[�^�����I�ɑ��肷�邽�߂̃^�C�}�[
        /// </summary>
        private Timer _timer = null;

        /// <summary>
        /// �R���X�g���N�^�[
        /// </summary>
        /// <param name="channel">�A�i���O���̓`���l��</param>
        public AnalogSensor(Cpu.AnalogChannel channel)
        {
            _sensor = new AnalogInput(channel);
        }

        /// <summary>
        /// �Z���T�[�f�[�^����̊Ԋu�i<see cref="GrFamily.Module.AnalogSensor.Interval">Interval</see>�j�̃v���C�x�[�g�ϐ�
        /// </summary>
        private int _interval = 0;

        /// <summary>
        /// �Z���T�[�f�[�^����̊Ԋu<br />�P�� : �~���b
        /// </summary>
        /// <remarks>���̐����łȂ��ꍇ�̓^�C�}�[�����s���Ȃ�</remarks>
        public int Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                SetTimer();
            }
        }

        /// <summary>
        /// �Z���T�[�f�[�^����̃^�C�}�[��L�������邩�ǂ����̃v���C�x�[�g�ϐ��i<see cref="GrFamily.Module.AnalogSensor.Enabled">Enabled</see>�j�̃v���C�x�[�g�ϐ�
        /// </summary>
        private bool _enabled = false;

        /// <summary>
        /// �Z���T�[�f�[�^����̃^�C�}�[��L�������邩�ǂ���
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SetTimer();
            }
        }

        /// <summary>
        /// �Z���T�[�f�[�^����̃^�C�}�[�����s����
        /// </summary>
        private void SetTimer()
        {
            if (_timer == null)
                _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);

            if (_interval > 0 && _enabled)
                _timer.Change(new TimeSpan(_interval * 10000), new TimeSpan(_interval * 10000));
            else
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// �A�i���O���̓Z���T�[�������I�Ƀf�[�^���擾����
        /// </summary>
        /// <param name="state">���g�p</param>
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

        /// <summary>
        /// �A�i���O���̓Z���T�[�̐��̃f�[�^���擾����
        /// </summary>
        /// <returns>�A�i���O���̓Z���T�[�̐��̃f�[�^</returns>
        /// <remarks>�Ԃ��l�͈̔͂�A/D�ϊ��̕���\�ɂ��BGR-PEACH�̏ꍇ�� 0�`4095</remarks>
        public int ReadRaw()
        {
            return _sensor.ReadRaw();
        }

        /// <summary>
        /// �A�i���O���̓Z���T�[�̃f�[�^���擾����
        /// </summary>
        /// <returns>�A�i���O���̓Z���T�[�̃f�[�^</returns>
        public double Read()
        {
            return _sensor.Read();
        }

        /// <summary>
        /// �f�[�^�擾�̃C�x���g�n���h���[�ɓn�����A�i���O���̓Z���T�[�̃Z���T�[�f�[�^
        /// </summary>
        public class MeasurementCompleteEventArgs
        {
            /// <summary>�A�i���O���̓Z���T�[�̐��̑���l</summary>
            public int RawValue;
            /// <summary>�A�i���O���̓Z���T�[�̑���l</summary>
            public double Value;
        }
    }
}
