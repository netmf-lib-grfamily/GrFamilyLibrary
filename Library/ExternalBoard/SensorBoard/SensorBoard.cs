using GrFamily.MainBoard;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    /// <summary>
    /// PinKit �Z���T�[�{�[�h
    /// </summary>
    public class SensorBoard
    {
        /// <summary>�T�[�~�X�^�[�̓��̓`�����l��</summary>
        private readonly Cpu.AnalogChannel _tempChannel;
        /// <summary>�u���b�N�[�q��̓��̓`�����l��</summary>
        private readonly Cpu.AnalogChannel _termChannel;

        /// <summary>�����x�Z���T�[��I2C�A�h���X</summary>
        private const ushort AccelerometerAddress = 0x1d;

        /// <summary>103AT��B�萔</summary>
        private const double Bc = 3435;
        /// <summary>103AT��25�x�ł̃[�����ג�R�l</summary>
        private const double R25 = 10000;

        /// <summary>AD�R���o�[�^�[�̕���\</summary>
        private const double Adc = 4096;
        /// <summary>�T�[�~�X�^�[�̕�����R�l�iVR1�𒆊Ԃɐݒ肵����Ԃ��f�t�H���g�Ƃ���j</summary>
        private const double DefaultVr1 = 5000;
        /// <summary>�u���b�N�[�q��̕�����R�l�iVR2�𒆊Ԃɐݒ肵����Ԃ��f�t�H���g�Ƃ���j</summary>
        private const double DefaultVr2 = 5000;

        /// <summary>I2C�̃N���b�N���g��</summary>
        // ReSharper disable once UnusedMember.Local
        private const int DefaultClockRateKhz = 100;
        /// <summary>I2C�̃^�C���A�E�g����</summary>
        // ReSharper disable once UnusedMember.Local
        private const int DefaultTimeout = 1000;

        /// <summary>���x�Z���T�[�i�T�[�~�X�^�[�j</summary>
        /// <remarks>103AT</remarks>
        private Temperature _temperature;
        /// <summary>�����x�Z���T�[</summary>
        /// <remarks>ADSL345</remarks>
        private Accelerometer _accelerometer;
        /// <summary>�[�q��</summary>
        private SensorTerminal _terminal;

        /// <summary>������R�l�i�T�[�~�X�^�[�p�j</summary>
        private readonly double _vr1;
        /// <summary>������R�l�i�[�q��p�j</summary>
        private readonly double _vr2;

        /// <summary>
        /// ���x�Z���T�[�̃R���X�g���N�^�[
        /// </summary>
        public Temperature Temperature => _temperature ?? (_temperature = new Temperature(_tempChannel, Bc, R25, _vr1, Adc));

        /// <summary>
        /// �����x�Z���T�[�̃R���X�g���N�^�[
        /// </summary>
        public Accelerometer Accelerometer => _accelerometer ?? (_accelerometer = new Accelerometer(AccelerometerAddress));

        /// <summary>
        /// �[�q��̃R���X�g���N�^�[
        /// </summary>
        public SensorTerminal Terminal => _terminal ?? (_terminal = new SensorTerminal(_termChannel));

        /// <summary>
        /// PinKit �Z���T�[�{�[�h�̃R���X�g���N�^�[
        /// </summary>
        public SensorBoard() : this(DefaultVr1, DefaultVr2)
        {
        }

        /// <summary>
        /// PinKit �Z���T�[�{�[�h�̃R���X�g���N�^�[
        /// </summary>
        /// <param name="vr1">������R�l�i�T�[�~�X�^�[�p�j</param>
        /// <param name="vr2">������R�l�i�[�q��p�j</param>
        public SensorBoard(double vr1, double vr2)
        {
            _tempChannel = Pins.ANALOG_5;   // �T�[�~�X�^�[��Analog 5�ԃs��
            _termChannel = Pins.ANALOG_4;   // �[�q���Analog 4�ԃs��
            _vr1 = vr1;
            _vr2 = vr2;
        }
    }
}
