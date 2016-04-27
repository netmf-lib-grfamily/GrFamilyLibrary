using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    /// <summary>
    /// 端子台に接続したセンサーの測定値を返すデリゲート
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TerminalMeasurementCompleteEventHandler(SensorTerminal sender, SensorTerminal.MeasurementCompleteEventArgs e);

    /// <summary>
    /// 端子台を使用したセンサー
    /// </summary>
    public class SensorTerminal
    {
        /// <summary>
        /// 端子台に接続したセンサーでの測定時のイベントハンドラー
        /// </summary>
        public event TerminalMeasurementCompleteEventHandler MeasurementComplete;

        /// <summary>
        /// アナログ入力ピン
        /// </summary>
        private readonly AnalogInput _sensor;

        /// <summary>
        /// 端子台センサーのデータを定期的に測定するためのタイマー
        /// </summary>
        private readonly Timer _timer;

        private int _interval = -1;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="channel">アナログチャンネル</param>
        public SensorTerminal(Cpu.AnalogChannel channel)
        {
            _sensor = new AnalogInput(channel);

            _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// センサーデータ測定の間隔<br />単位 : ミリ秒
        /// </summary>
        /// <remarks>正の整数でない場合はタイマーを実行しない</remarks>
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        /// <summary>
        /// 端子台に接続したセンサーから定期的にデータを取得する
        /// </summary>
        /// <param name="state">未使用</param>
        private void Measure_Timer(object state)
        {
            if (MeasurementComplete == null)
                return;

            var result = new MeasurementCompleteEventArgs
            {
                RawValue = _sensor.ReadRaw(),
                Value = _sensor.Read()
            };
            MeasurementComplete(this, result);
        }

        /// <summary>
        /// タイマーを起動して、端子台に接続したセンサーから定期的にデータの取得を始める
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
        /// 端子台に接続したセンサーから定期的にデータを取得するためのタイマーを停止する
        /// </summary>
        public void StopTakingMeasurements()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// 端子台に接続したセンサーの生の測定値を取得する
        /// </summary>
        /// <returns>0～4095の値</returns>
        /// <remarks>GR-PEAChではA/D変換の分解能は4096</remarks>
        public int ReadRaw()
        {
            lock (this)
            {
                return _sensor.ReadRaw();
            }
        }

        /// <summary>
        /// 端子台に接続したセンサーの測定値を取得する
        /// </summary>
        /// <returns>0～1.0の値</returns>
        public double Read()
        {
            lock (this)
            {
                return _sensor.Read();
            }
        }


        /// <summary>
        /// データ取得のイベントハンドラーに渡されるセンサーデータ（端子台に接続したセンサー）
        /// </summary>
        public class MeasurementCompleteEventArgs
        {
            /// <summary>0～4095の値</summary>
            public int RawValue;
            /// <summary>0～1.0の値</summary>
            public double Value;
        }
    }
}
