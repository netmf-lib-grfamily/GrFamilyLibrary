using System;
using System.Threading;
using GrFamily.Module;

namespace GrFamily.ExternalBoard
{
    /// <summary>
    /// 3軸加速度センサーの測定値を返すデリゲート
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AccelerometerMeasurementCompleteEventHandler(Accelerometer sender, Accelerometer.MeasurementCompleteEventArgs e);

    /// <summary>
    /// 3軸加速度センサー（ADXL345）
    /// </summary>
    public class Accelerometer : I2CDeviceEx
    {
        /// <summary>
        /// 加速度測定時に呼び出されるイベントハンドラー
        /// </summary>
        public event AccelerometerMeasurementCompleteEventHandler MeasurementComplete;

        /// <summary>電源管理コマンド用のレジスター</summary>
        private const byte PowerCtl = 0x2d;
        /// <summary>測定範囲を指定するコマンド用のレジスター</summary>
        private const byte DataFormat = 0x31;
        /// <summary>測定値取得用のレジスター先頭</summary>
        private const byte DataX0 = 0x32; 

        /// <summary>測定範囲</summary>
        private Range _range;

        /// <summary>センサーデータの読出しバッファ</summary>
        private byte[] _xyz = new byte[6];

        /// <summary>センサーデータを定期的に測定するためのタイマー</summary>
        private readonly Timer _timer;

        /// <summary>
        /// センサーデータ測定の間隔<br />単位 : ミリ秒
        /// </summary>
        /// <remarks>正の整数でない場合はタイマーを実行しない</remarks>
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }
        private int _interval = -1;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="i2CAddress">I2Cのアドレス</param>
        internal Accelerometer(ushort i2CAddress) : base(i2CAddress)
        {
            _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);

            MeasurementRange = Range.FourG;
            ToWakeup();
            
            Thread.Sleep(10);
        }

        /// <summary>
        /// スリープモードに入らないようにする
        /// </summary>
        private void ToWakeup()
        {
            RegWriteMask(PowerCtl, 0x00, 0x04);
        }

        /// <summary>
        /// データフォーマットを設定する
        /// </summary>
        /// <param name="n">設定値</param>
        /// <remarks>設定値はビットフラグ<br />
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
        /// 測定範囲
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
        /// 加速度を測定する
        /// </summary>
        private void Measure()
        {
            RegWriteMask(PowerCtl, 0x08, 0x08);
        }

        /// <summary>
        /// X軸方向の加速度データを取得する
        /// </summary>
        /// <returns>X軸方向の加速度データ</returns>
        public short GetX()
        {
            Measure();
            RegReads(DataX0, ref _xyz);
            return (short)((_xyz[1] << 8) + _xyz[0]);
        }

        /// <summary>
        /// Y軸方向の加速度データを取得する
        /// </summary>
        /// <returns>Y軸方向の加速度データ</returns>
        public short GetY()
        {
            Measure();
            RegReads(DataX0, ref _xyz);
            return (short)((_xyz[3] << 8) + _xyz[2]);
        }

        /// <summary>
        /// Z軸方向の加速度データを取得する
        /// </summary>
        /// <returns>Z軸方向の加速度データ</returns>
        public short GetZ()
        {
            Measure();
            RegReads(DataX0, ref _xyz);
            return (short)((_xyz[5] << 8) + _xyz[4]);
        }

        /// <summary>
        /// 3軸の加速度データを取得する
        /// </summary>
        /// <param name="x">X軸方向の加速度データの受信バッファ</param>
        /// <param name="y">Y軸方向の加速度データの受信バッファ</param>
        /// <param name="z">Z軸方向の加速度データの受信バッファ</param>
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
        /// 定期的に加速度データを取得する
        /// </summary>
        /// <param name="state">未使用</param>
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
        /// タイマーを起動して、定期的に加速度データの取得を始める
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
        /// 加速度データを定期的に取得するためのタイマーを停止する
        /// </summary>
        public void StopTakingMeasurements()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// 加速度の測定範囲
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
        /// データ取得のイベントハンドラーに渡される加速度データ
        /// </summary>
        public class MeasurementCompleteEventArgs
        {
            /// <summary>X軸方向の加速度データ</summary>
            public short X;
            /// <summary>Y軸方向の加速度データ</summary>
            public short Y;
            /// <summary>Z軸方向の加速度データ</summary>
            public short Z;
        }
    }
}
