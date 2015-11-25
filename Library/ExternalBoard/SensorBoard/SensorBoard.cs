using GrFamily.MainBoard;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    public class SensorBoard
    {
        // サーミスターの入力チャンネル
        private readonly Cpu.AnalogChannel _tempChannel;
        // ブロック端子台の入力チャンネル
        private readonly Cpu.AnalogChannel _termChannel;

        // 加速度センサーのI2Cアドレス
        private const ushort AccelerometerAddress = 0x1d;

        private const double Bc = 3435;         // 103ATのB定数
        private const double R25 = 10000;       // 103ATの25度でのゼロ負荷抵抗値

        private const double Adc = 4096;        // ADコンバーターの分解能
        private const double DefaultVr1 = 5000;     // サーミスターの分圧抵抗値（VR1を中間に設定した状態をデフォルトとする）
        private const double DefaultVr2 = 5000;     // ブロック端子台の分圧抵抗値 (VR2を中間に設定した状態をデフォルトとする）

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
