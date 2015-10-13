using GrFamily.MainBoard;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    public class SensorBoard
    {
        // サーミスターの入力チャンネル
        private readonly Cpu.AnalogChannel _tempChannel;
        // 加速度センサーのI2Cアドレス
        private readonly ushort _accelerometerAddress = 0x1d;

        private const double Bc = 3435;         // 103ATのB定数
        private const double R25 = 10000;       // 103ATの25度でのゼロ負荷抵抗値

        private const double Adc = 4096;        // ADコンバーターの分解能
        private const double DefaultVr1 = 5000;     // 分圧抵抗値（VR1を中間）

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
