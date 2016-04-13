using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    /// <summary>
    /// GR-SAKURAクラス
    /// </summary>
    public class Sakura
    {
        /// <summary>GR-SAKURA上のボタンのピン番号</summary>
        private const Cpu.Pin ButtonPin = (Cpu.Pin)0x57;

        /// <summary>GR-PEACH上の1番LEDのピン番号</summary>
        private const Cpu.Pin Led1Pin = (Cpu.Pin)0x50;
        /// <summary>GR-PEACH上の2番LEDのピン番号</summary>
        private const Cpu.Pin Led2Pin = (Cpu.Pin)0x51;
        /// <summary>GR-PEACH上の3番LEDのピン番号</summary>
        private const Cpu.Pin Led3Pin = (Cpu.Pin)0x52;
        /// <summary>GR-PEACH上の4番LEDのピン番号</summary>
        private const Cpu.Pin Led4Pin = (Cpu.Pin)0x56;

        /// <summary>1番 <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _led1;
        /// <summary>2番 <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _led2;
        /// <summary>3番 <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _led3;
        /// <summary>4番 <see cref="GrFamily.MainBoard.DebugLed">LED</see></summary>
        /// <remarks>このライブラリではGR-SAKURAの4番LEDをデバッグ用LEDとしても利用する</remarks>
        private readonly DebugLed _debugLed;

        /// <summary>
        /// GR-SAKURAのコンストラクター
        /// </summary>
        public Sakura()
        {
            _led1 = new Led(Led1Pin);
            _led2 = new Led(Led2Pin);
            _led3 = new Led(Led3Pin);
            _debugLed = new DebugLed(Led4Pin);

            Button = new Button(ButtonPin);
        }

        /// <summary>GR-SAKURA上のボタン</summary>
        public Button Button { get; }

        /// <summary>
        /// 全てのLEDを消灯する
        /// </summary>
        public void TurnAllLedsOff()
        {
            _led1.SetLed(false);
            _led2.SetLed(false);
            _led3.SetLed(false);
            _debugLed.SetLed(false);
        }

        /// <summary>
        /// 各LEDを個別に点灯・消灯する
        /// </summary>
        /// <param name="led1On">1番LEDを点灯または消灯する</param>
        /// <param name="led2On">2番LEDを点灯または消灯する</param>
        /// <param name="led3On">3番LEDを点灯または消灯する</param>
        /// <param name="led4On">4番LEDを点灯または消灯する</param>
        public void SetLeds(bool led1On, bool led2On, bool led3On, bool led4On)
        {
            _led1.SetLed(led1On);
            _led2.SetLed(led2On);
            _led3.SetLed(led3On);
            _debugLed.SetLed(led4On);
        }

        /// <summary>
        /// 個数を指定してLEDを点灯する
        /// </summary>
        /// <param name="count">点灯するLEDの個数</param>
        /// <remarks>指定した個数に応じて、1番LEDから順に点灯する</remarks>
        public void SetLeds(int count)
        {
            SetLeds(count >= 1, count >= 2, count >= 3, count >= 4);
        }

        /// <summary>
        /// LED点灯のアニメーションを実行する
        /// </summary>
        /// <param name="switchTime">アニメーション実行間隔（単位 : ミリ秒）</param>
        /// <param name="remain">小さい番号のLEDを点灯したままにする場合は true、どれか1個のLEDだけを点灯させる場合は false</param>
        /// <param name="repeatCount">繰り返し回数</param>
        public void Animate(int switchTime, bool remain = true, int repeatCount = 1)
        {
            SetLedsInner(0, false);

            for (var animateCount = 0; animateCount < repeatCount; animateCount++)
            {
                for (var ledNo = 1; ledNo <= 4; ledNo++)
                {
                    SetLedsInner(ledNo, remain);
                    Thread.Sleep(switchTime);
                }

                for (var ledNo = 3; ledNo >= 0; ledNo--)
                {
                    SetLedsInner(ledNo, remain);
                    Thread.Sleep(switchTime);
                }
            }
        }

        /// <summary>
        /// LED番号と小さい番号のLEDを点灯させるかを指定して、必要なLEDを点灯させる
        /// </summary>
        /// <param name="count">点灯対象のLED番号</param>
        /// <param name="remain">点灯対象より小さい番号のLEDを一緒に点灯する場合は true、そうでない場合は false</param>
        private void SetLedsInner(int count, bool remain)
        {
            var led1 = remain ? count >= 1 : count == 1;
            var led2 = remain ? count >= 2 : count == 2;
            var led3 = remain ? count >= 3 : count == 3;
            var led4 = remain ? count >= 4 : count == 4;

            SetLeds(led1, led2, led3, led4);
        }


        /// <summary>
        /// デバッグLEDを点灯または消灯する
        /// </summary>
        /// <param name="lightOn">点灯する場合は true、消灯する場合は false</param>
        public void SetDebugLed(bool lightOn)
        {
            _debugLed.SetLed(lightOn);
        }
        
        /// <summary>
        /// デバッグLEDを1回点滅する
        /// </summary>
        public void PulseDebugLed()
        {
            _debugLed.PulseDebugLed();
        }

        /// <summary>
        /// デバッグLEDを指定回数、指定間隔で点滅させる
        /// </summary>
        /// <param name="length">点滅間隔（単位：ミリ秒）</param>
        /// <param name="times">点滅回数</param>
        public void PulseDebugLed(int length, int times)
        {
            _debugLed.PulseDebugLed(length, times);
        }
    }
}
