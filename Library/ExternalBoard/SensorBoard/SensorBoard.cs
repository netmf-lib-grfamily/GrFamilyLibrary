using GrFamily.MainBoard;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    public class SensorBoard
    {
        // �T�[�~�X�^�[�̓��̓`�����l��
        private readonly Cpu.AnalogChannel _tempChannel;
        // �����x�Z���T�[��I2C�A�h���X
        private readonly ushort _accelerometerAddress = 0x1d;

        private const double Bc = 3435;         // 103AT��B�萔
        private const double R25 = 10000;       // 103AT��25�x�ł̃[�����ג�R�l

        private const double Adc = 4096;        // AD�R���o�[�^�[�̕���\
        private const double DefaultVr1 = 5000;     // ������R�l�iVR1�𒆊ԁj

        private const int DefaultAccelClockRateKhz = 100;
        private const int DefaultAccelTimeout = 1000;
        
        private Temperature _temperature;
        private readonly double _vr1;

        private Accelerometer _accelerometer;

        public Temperature Temperature
        {
            get
            {
                return _temperature ?? (_temperature = new Temperature(_tempChannel, Bc, R25, _vr1, Adc));
            }
        }

        public Accelerometer Accelerometer
        {
            get
            {
                if (_accelerometer == null)
                {
                    var i2C = new I2CDevice(new I2CDevice.Configuration(_accelerometerAddress, DefaultAccelClockRateKhz));
                    _accelerometer = new Accelerometer(i2C, 1000);
                }

                return _accelerometer;
            }
        }

        public SensorBoard() : this(DefaultVr1)
        {
        }

        public SensorBoard(double vr1)
        {
            _tempChannel = Pins.ANALOG_5;
            _vr1 = vr1;
        }
    }
}
