using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    /// <summary>
    /// 温度センサー（サーミスター）の測定値を返すデリゲート
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TemperatureMeasurementCompleteEventHandler(Temperature sender, Temperature.MeasurementCompleteEventArgs e);

    /// <summary>
    /// 温度センサー（サーミスター）
    /// </summary>
    public class Temperature
    {
        /// <summary>
        /// 温度測定時にイベントハンドラーを通して返却するセンサーデータ
        /// </summary>
        public event TemperatureMeasurementCompleteEventHandler MeasurementComplete;

        /// <summary>摂氏0度の絶対温度での値</summary>
        private const double Tk = 273;
        /// <summary>摂氏25度の絶対温度での値</summary>
        private const double T25 = Tk + 25; 

        /// <summary>サーミスターの入力ポート</summary>
        private readonly AnalogInput _temperatureInput;

        /// <summary>サーミスターのB定数</summary>
        private readonly double _bc;
        /// <summary>摂氏25度でのゼロ負荷抵抗値</summary>
        private readonly double _r25;
        /// <summary>分圧抵抗値</summary>
        private readonly double _vrd;
        /// <summary>ADコンバーターの分解能</summary>
        private readonly double _adc;

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
        /// <param name="channel">サーミスターを接続するアナログチャンネル</param>
        /// <param name="bc">サーミスターのB定数値</param>
        /// <param name="r25">摂氏25度のゼロ負荷抵抗値</param>
        /// <param name="vrd">分圧する抵抗値</param>
        /// <param name="adc">A/D変換の分解能</param>
        internal Temperature(Cpu.AnalogChannel channel, double bc, double r25, double vrd, double adc)
        {
            _temperatureInput = new AnalogInput(channel);
            _bc = bc;
            _r25 = r25;
            _vrd = vrd;
            _adc = adc;

            _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// 温度を測定する
        /// </summary>
        /// <returns>温度（摂氏）</returns>
        public double TakeMeasurement()
        {
            lock (this)
            {
                var raw = _temperatureInput.ReadRaw();
                return 1 / (Math.Log(_vrd * raw / (_adc - raw) / _r25) / _bc + 1 / T25) - Tk;
            }
        }

        /// <summary>
        /// 定期的に温度を取得する
        /// </summary>
        /// <param name="state">未使用</param>
        private void Measure_Timer(object state)
        {
            if (MeasurementComplete == null)
                return;

            MeasurementComplete(this, new MeasurementCompleteEventArgs { Temperature = TakeMeasurement() });
        }

        /// <summary>
        /// タイマーを起動して、定期的に温度の取得を始める
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
        /// 温度を定期的に取得するためのタイマーを停止する
        /// </summary>
        public void StopTakingMeasurements()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// データ取得のイベントハンドラーに渡される温度データ
        /// </summary>
        public class MeasurementCompleteEventArgs
        {
            /// <summary>温度</summary>
            public double Temperature;
        }
    }
}
