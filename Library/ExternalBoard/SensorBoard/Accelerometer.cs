using System;
using System.Threading;
using GrFamily.Module;

namespace GrFamily.ExternalBoard
{
    /// <summary>
    /// 3�������x�Z���T�[�̑���l��Ԃ��f���Q�[�g
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AccelerometerMeasurementCompleteEventHandler(Accelerometer sender, Accelerometer.MeasurementCompleteEventArgs e);

    /// <summary>
    /// 3�������x�Z���T�[�iADXL345�j
    /// </summary>
    public class Accelerometer : I2CDeviceEx
    {
        /// <summary>
        /// �����x���莞�ɌĂяo�����C�x���g�n���h���[
        /// </summary>
        public event AccelerometerMeasurementCompleteEventHandler MeasurementComplete;

        /// <summary>�d���Ǘ��R�}���h�p�̃��W�X�^�[</summary>
        private const byte PowerCtl = 0x2d;
        /// <summary>����͈͂��w�肷��R�}���h�p�̃��W�X�^�[</summary>
        private const byte DataFormat = 0x31;
        /// <summary>����l�擾�p�̃��W�X�^�[�擪</summary>
        private const byte DataX0 = 0x32; 

        /// <summary>����͈�</summary>
        private Range _range;

        /// <summary>�Z���T�[�f�[�^�̓Ǐo���o�b�t�@</summary>
        private byte[] _xyz = new byte[6];

        /// <summary>�Z���T�[�f�[�^�����I�ɑ��肷�邽�߂̃^�C�}�[</summary>
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
        /// <param name="i2CAddress">I2C�̃A�h���X</param>
        internal Accelerometer(ushort i2CAddress) : base(i2CAddress)
        {
            _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);

            MeasurementRange = Range.FourG;
            ToWakeup();
            
            Thread.Sleep(10);
        }

        /// <summary>
        /// �X���[�v���[�h�ɓ���Ȃ��悤�ɂ���
        /// </summary>
        private void ToWakeup()
        {
            RegWriteMask(PowerCtl, 0x00, 0x04);
        }

        /// <summary>
        /// �f�[�^�t�H�[�}�b�g��ݒ肷��
        /// </summary>
        /// <param name="n">�ݒ�l</param>
        /// <remarks>�ݒ�l�̓r�b�g�t���O<br />
        /// D7: SELF_TEST<br />
        /// D6: SPI<br />
        /// D5: INT_INVERT<br />
        /// D4: 0<br />
        /// D3: FULL_RES<br />
        /// D2: Justfy<br />
        /// D1-D0: Range<br />
        /// 0 - 0: +-2g<br />
        /// 0 - 1: +-4g<br />
        /// 1 - 0: +-8g<br />
        /// 1 - 1: +-16g<br />
        /// </remarks>
        private void SetDataFormat(byte n)
        {
            RegWrite(DataFormat, n);
        }

        /// <summary>
        /// ����͈�
        /// </summary>
        public Range MeasurementRange
        {
            get { return _range; }
            set
            {
                _range = value;
                SetDataFormat((byte)value);
            }
        }

        /// <summary>
        /// �����x�𑪒肷��
        /// </summary>
        private void Measure()
        {
            RegWriteMask(PowerCtl, 0x08, 0x08);
        }

        /// <summary>
        /// X�������̉����x�f�[�^���擾����
        /// </summary>
        /// <returns>X�������̉����x�f�[�^</returns>
        public short GetX()
        {
            Measure();
            RegReads(DataX0, ref _xyz);
            return (short)((_xyz[1] << 8) + _xyz[0]);
        }

        /// <summary>
        /// Y�������̉����x�f�[�^���擾����
        /// </summary>
        /// <returns>Y�������̉����x�f�[�^</returns>
        public short GetY()
        {
            Measure();
            RegReads(DataX0, ref _xyz);
            return (short)((_xyz[3] << 8) + _xyz[2]);
        }

        /// <summary>
        /// Z�������̉����x�f�[�^���擾����
        /// </summary>
        /// <returns>Z�������̉����x�f�[�^</returns>
        public short GetZ()
        {
            Measure();
            RegReads(DataX0, ref _xyz);
            return (short)((_xyz[5] << 8) + _xyz[4]);
        }

        /// <summary>
        /// 3���̉����x�f�[�^���擾����
        /// </summary>
        /// <param name="x">X�������̉����x�f�[�^�̎�M�o�b�t�@</param>
        /// <param name="y">Y�������̉����x�f�[�^�̎�M�o�b�t�@</param>
        /// <param name="z">Z�������̉����x�f�[�^�̎�M�o�b�t�@</param>
        // ReSharper disable once InconsistentNaming
        public void GetXYZ(out short x, out short y, out short z)
        {
            Measure();
            RegReads(DataX0, ref _xyz);
            x = (short)((_xyz[1] << 8) + _xyz[0]);
            y = (short)((_xyz[3] << 8) + _xyz[2]);
            z = (short)((_xyz[5] << 8) + _xyz[4]);
        }

        /// <summary>
        /// ����I�ɉ����x�f�[�^���擾����
        /// </summary>
        /// <param name="state">���g�p</param>
        private void Measure_Timer(object state)
        {
            if (MeasurementComplete == null)
                return;

            short x;
            short y;
            short z;
            GetXYZ(out x, out y, out z);

            MeasurementComplete(this, new MeasurementCompleteEventArgs() { X = x, Y = y, Z = z });
        }

        /// <summary>
        /// �^�C�}�[���N�����āA����I�ɉ����x�f�[�^�̎擾���n�߂�
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
        /// �����x�f�[�^�����I�Ɏ擾���邽�߂̃^�C�}�[���~����
        /// </summary>
        public void StopTakingMeasurements()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// �����x�̑���͈�
        /// </summary>
        public enum Range
        {
            /// <summary>+-2G</summary>
            TwoG = 0x00,
            /// <summary>+-4G</summary>
            FourG = 0x01,
            /// <summary>+-8G</summary>
            EightG = 0x10
        }

        /// <summary>
        /// �f�[�^�擾�̃C�x���g�n���h���[�ɓn���������x�f�[�^
        /// </summary>
        public class MeasurementCompleteEventArgs
        {
            /// <summary>X�������̉����x�f�[�^</summary>
            public short X;
            /// <summary>Y�������̉����x�f�[�^</summary>
            public short Y;
            /// <summary>Z�������̉����x�f�[�^</summary>
            public short Z;
        }
    }
}
