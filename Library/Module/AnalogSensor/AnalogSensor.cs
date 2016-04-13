using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.Module
{
    /// <summary>
    /// アナログ入力センサーの測定値を返すデリゲート
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void AnalogMeasurementCompleteEventHandler(AnalogSensor sender, AnalogSensor.MeasurementCompleteEventArgs e);

    /// <summary>
    /// アナログ入力センサー
    /// </summary>
    public class AnalogSensor
    {
        /// <summary>
        /// アナログ入力センサーのセンサーでの測定時のイベントハンドラー
        /// </summary>
        public event AnalogMeasurementCompleteEventHandler MeasurementComplete = null;

        /// <summary>
        /// アナログ入力ポート
        /// </summary>
        private readonly AnalogInput _sensor;

        /// <summary>
        /// アナログ入力センサーのデータを定期的に測定するためのタイマー
        /// </summary>
        private Timer _timer = null;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="channel">アナログ入力チャネル</param>
        public AnalogSensor(Cpu.AnalogChannel channel)
        {
            _sensor = new AnalogInput(channel);
        }

        /// <summary>
        /// センサーデータ測定の間隔（<see cref="GrFamily.Module.AnalogSensor.Interval">Interval</see>）のプライベート変数
        /// </summary>
        private int _interval = 0;

        /// <summary>
        /// センサーデータ測定の間隔<br />単位 : ミリ秒
        /// </summary>
        /// <remarks>正の整数でない場合はタイマーを実行しない</remarks>
        public int Interval
        {
            get { return _interval; }
            set
            {
                _interval = value;
                SetTimer();
            }
        }

        /// <summary>
        /// センサーデータ測定のタイマーを有効化するかどうかのプライベート変数（<see cref="GrFamily.Module.AnalogSensor.Enabled">Enabled</see>）のプライベート変数
        /// </summary>
        private bool _enabled = false;

        /// <summary>
        /// センサーデータ測定のタイマーを有効化するかどうか
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                SetTimer();
            }
        }

        /// <summary>
        /// センサーデータ測定のタイマーを実行する
        /// </summary>
        private void SetTimer()
        {
            if (_timer == null)
                _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);

            if (_interval > 0 && _enabled)
                _timer.Change(new TimeSpan(_interval * 10000), new TimeSpan(_interval * 10000));
            else
                _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// アナログ入力センサーから定期的にデータを取得する
        /// </summary>
        /// <param name="state">未使用</param>
        private void Measure_Timer(object state)
        {
            if (MeasurementComplete == null) return;

            var result = new MeasurementCompleteEventArgs
            {
                RawValue = _sensor.ReadRaw(),
                Value = _sensor.Read()
            };
            MeasurementComplete(this, result);
        }

        /// <summary>
        /// アナログ入力センサーの生のデータを取得する
        /// </summary>
        /// <returns>アナログ入力センサーの生のデータ</returns>
        /// <remarks>返す値の範囲はA/D変換の分解能による。GR-PEACHの場合は 0～4095</remarks>
        public int ReadRaw()
        {
            return _sensor.ReadRaw();
        }

        /// <summary>
        /// アナログ入力センサーのデータを取得する
        /// </summary>
        /// <returns>アナログ入力センサーのデータ</returns>
        public double Read()
        {
            return _sensor.Read();
        }

        /// <summary>
        /// データ取得のイベントハンドラーに渡されるアナログ入力センサーのセンサーデータ
        /// </summary>
        public class MeasurementCompleteEventArgs
        {
            /// <summary>アナログ入力センサーの生の測定値</summary>
            public int RawValue;
            /// <summary>アナログ入力センサーの測定値</summary>
            public double Value;
        }
    }
}
