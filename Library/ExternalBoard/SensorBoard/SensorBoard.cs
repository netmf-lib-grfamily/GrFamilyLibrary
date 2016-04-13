using GrFamily.MainBoard;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    /// <summary>
    /// PinKit センサーボード
    /// </summary>
    public class SensorBoard
    {
        /// <summary>サーミスターの入力チャンネル</summary>
        private readonly Cpu.AnalogChannel _tempChannel;
        /// <summary>ブロック端子台の入力チャンネル</summary>
        private readonly Cpu.AnalogChannel _termChannel;

        /// <summary>加速度センサーのI2Cアドレス</summary>
        private const ushort AccelerometerAddress = 0x1d;

        /// <summary>103ATのB定数</summary>
        private const double Bc = 3435;
        /// <summary>103ATの25度でのゼロ負荷抵抗値</summary>
        private const double R25 = 10000;

        /// <summary>ADコンバーターの分解能</summary>
        private const double Adc = 4096;
        /// <summary>サーミスターの分圧抵抗値（VR1を中間に設定した状態をデフォルトとする）</summary>
        private const double DefaultVr1 = 5000;
        /// <summary>ブロック端子台の分圧抵抗値（VR2を中間に設定した状態をデフォルトとする）</summary>
        private const double DefaultVr2 = 5000;

        /// <summary>I2Cのクロック周波数</summary>
        // ReSharper disable once UnusedMember.Local
        private const int DefaultClockRateKhz = 100;
        /// <summary>I2Cのタイムアウト時間</summary>
        // ReSharper disable once UnusedMember.Local
        private const int DefaultTimeout = 1000;

        /// <summary>温度センサー（サーミスター）</summary>
        /// <remarks>103AT</remarks>
        private Temperature _temperature;
        /// <summary>加速度センサー</summary>
        /// <remarks>ADSL345</remarks>
        private Accelerometer _accelerometer;
        /// <summary>端子台</summary>
        private SensorTerminal _terminal;

        /// <summary>分圧抵抗値（サーミスター用）</summary>
        private readonly double _vr1;
        /// <summary>分圧抵抗値（端子台用）</summary>
        private readonly double _vr2;

        /// <summary>
        /// 温度センサーのコンストラクター
        /// </summary>
        public Temperature Temperature => _temperature ?? (_temperature = new Temperature(_tempChannel, Bc, R25, _vr1, Adc));

        /// <summary>
        /// 加速度センサーのコンストラクター
        /// </summary>
        public Accelerometer Accelerometer => _accelerometer ?? (_accelerometer = new Accelerometer(AccelerometerAddress));

        /// <summary>
        /// 端子台のコンストラクター
        /// </summary>
        public SensorTerminal Terminal => _terminal ?? (_terminal = new SensorTerminal(_termChannel));

        /// <summary>
        /// PinKit センサーボードのコンストラクター
        /// </summary>
        public SensorBoard() : this(DefaultVr1, DefaultVr2)
        {
        }

        /// <summary>
        /// PinKit センサーボードのコンストラクター
        /// </summary>
        /// <param name="vr1">分圧抵抗値（サーミスター用）</param>
        /// <param name="vr2">分圧抵抗値（端子台用）</param>
        public SensorBoard(double vr1, double vr2)
        {
            _tempChannel = Pins.ANALOG_5;   // サーミスターはAnalog 5番ピン
            _termChannel = Pins.ANALOG_4;   // 端子台はAnalog 4番ピン
            _vr1 = vr1;
            _vr2 = vr2;
        }
    }
}
