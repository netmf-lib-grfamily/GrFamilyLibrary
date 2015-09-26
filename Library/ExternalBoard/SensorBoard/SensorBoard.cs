using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    public class SensorBoard
    {
        // �T�[�~�X�^�[�̓��̓`�����l��
        private const Cpu.AnalogChannel TempChannel = (Cpu.AnalogChannel) 7;
        // �����x�Z���T�[�̃A�h���X
        private const byte AccelerometerAddress = 0x1d;


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
                return _temperature ?? (_temperature = new Temperature(TempChannel, Bc, R25, _vr1, Adc));
            }
        }

        public Accelerometer Accelerometer
        {
            get
            {
                if (_accelerometer == null)
                {
                    var i2C = new I2CDevice(new I2CDevice.Configuration((ushort)AccelerometerAddress, DefaultAccelClockRateKhz));
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
            _vr1 = vr1;
        }
    }
}
