using GrFamily.MainBoard;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    public class SensorBoard
    {
        // �T�[�~�X�^�[�̓��̓`�����l��
        private readonly Cpu.AnalogChannel _tempChannel;
        // �u���b�N�[�q��̓��̓`�����l��
        private readonly Cpu.AnalogChannel _termChannel;

        // �����x�Z���T�[��I2C�A�h���X
        private const ushort AccelerometerAddress = 0x1d;

        private const double Bc = 3435;         // 103AT��B�萔
        private const double R25 = 10000;       // 103AT��25�x�ł̃[�����ג�R�l

        private const double Adc = 4096;        // AD�R���o�[�^�[�̕���\
        private const double DefaultVr1 = 5000;     // �T�[�~�X�^�[�̕�����R�l�iVR1�𒆊Ԃɐݒ肵����Ԃ��f�t�H���g�Ƃ���j
        private const double DefaultVr2 = 5000;     // �u���b�N�[�q��̕�����R�l (VR2�𒆊Ԃɐݒ肵����Ԃ��f�t�H���g�Ƃ���j

        private const int DefaultClockRateKhz = 100;
        private const int DefaultTimeout = 1000;

        private Temperature _temperature;
        private SensorTerminal _terminal;
        private Accelerometer _accelerometer;

        private readonly double _vr1;
        private readonly double _vr2;

        public Temperature Temperature
        {
            get
            {
                return _temperature ?? (_temperature = new Temperature(_tempChannel, Bc, R25, _vr1, Adc));
            }
        }

        public SensorTerminal Terminal
        {
            get
            {
                return _terminal ?? (_terminal = new SensorTerminal(_termChannel, _vr2));
            }
        }

        public Accelerometer Accelerometer
        {
            get { return _accelerometer ?? (_accelerometer = new Accelerometer(AccelerometerAddress)); }
        }

        public SensorBoard() : this(DefaultVr1, DefaultVr2)
        {
        }

        public SensorBoard(double vr1, double vr2)
        {
            _tempChannel = Pins.ANALOG_5;
            _termChannel = Pins.ANALOG_4;
            _vr1 = vr1;
            _vr2 = vr2;
        }
    }
}
