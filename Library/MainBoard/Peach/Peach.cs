using System;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    /// <summary>
    /// GR-PEACHクラス
    /// </summary>
    public class Peach
    {
        /// <summary>GR-PEACH上のボタンのピン番号</summary>
        private const Cpu.Pin ButtonPin = (Cpu.Pin)0x60;

        /// <summary>GR-PEACH上の赤色LEDのピン番号</summary>
        private const Cpu.Pin RedPin = (Cpu.Pin)0x6d;
        /// <summary>GR-PEACH上の緑色LEDのピン番号</summary>
        private const Cpu.Pin GreenPin = (Cpu.Pin)0x6e;
        /// <summary>GR-PEACH上の青色LEDのピン番号</summary>
        private const Cpu.Pin BluePin = (Cpu.Pin)0x6f;
        /// <summary>GR-PEACH上のユーザーLEDのピン番号</summary>
        private const Cpu.Pin UserPin = (Cpu.Pin)0x6c;

        /// <summary>赤色 <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _redLed;
        /// <summary>緑色 <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _greenLed;
        /// <summary>青色 <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _blueLed;
        /// <summary>ユーザー <see cref="GrFamily.MainBoard.DebugLed">LED</see></summary>
        private readonly DebugLed _debugLed;

        /// <summary>
        /// GR-PEACHのコンストラクター
        /// </summary>
        public Peach()
        {
            _redLed = new Led(RedPin);
            _greenLed = new Led(GreenPin);
            _blueLed = new Led(BluePin);
            _debugLed = new DebugLed(UserPin);

            Button = new Button(ButtonPin);
        }

        /// <summary>GR-PEACH上のボタン</summary>
        public Button Button { get; }

        /// <summary>
        /// 全てのLEDを消灯する（赤・緑・青・デバッグLED）
        /// </summary>
        public void TurnAllLedsOff()
        {
            _redLed.SetLed(false);
            _greenLed.SetLed(false);
            _blueLed.SetLed(false);
            _debugLed.SetLed(false);
        }

        /// <summary>
        /// 各色のLEDを個別に点灯・消灯する
        /// </summary>
        /// <param name="redOn">赤色LEDを点灯または消灯する</param>
        /// <param name="greenOn">緑色LEDを点灯または消灯する</param>
        /// <param name="blueOn">青色LEDを点灯または消灯する</param>
        public void SetLeds(bool redOn, bool greenOn, bool blueOn)
        {
            _redLed.SetLed(redOn);
            _greenLed.SetLed(greenOn);
            _blueLed.SetLed(blueOn);
        }

        /// <summary>
        /// 色名を指定してカラーLEDを点灯する
        /// </summary>
        /// <param name="color">点灯したい色名</param>
        public void SetColor(Color color)
        {
            _redLed.SetLed((color & Color.Red) != 0);
            _greenLed.SetLed((color & Color.Green) != 0);
            _blueLed.SetLed((color & Color.Blue) != 0);
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

        /// <summary>
        /// 色名指定
        /// </summary>
        [Flags]
        public enum Color
        {
            /// <summary>黒（カラーLEDを全て消灯する）</summary>
            Black = 0,
            /// <summary>赤</summary>
            Red = 1,
            /// <summary>緑</summary>
            Green = 2,
            /// <summary>青</summary>
            Blue = 4,

            /// <summary>黄</summary>
            Yellow = Red + Green,
            /// <summary>紫（マゼンタ）</summary>
            Magenta = Red + Blue,
            /// <summary>青緑（シアン）</summary>
            Cyan = Green + Blue,
            /// <summary>白</summary>
            White = Red + Green + Blue
        }
    }
}
