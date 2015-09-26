using System;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public class Peach
    {
        private const Cpu.Pin ButtonPin = (Cpu.Pin)0x60;

        // 各 LED のピン番号
        private const Cpu.Pin RedPin = (Cpu.Pin)0x6d;       // 赤
        private const Cpu.Pin GreenPin = (Cpu.Pin)0x6e;     // 緑
        private const Cpu.Pin BluePin = (Cpu.Pin)0x6f;      // 青
        private const Cpu.Pin UserPin = (Cpu.Pin)0x6c;      // ユーザー

        private readonly Button _button;

        private readonly Led _redLed;
        private readonly Led _greenLed;
        private readonly Led _blueLed;
        private readonly DebugLed _debugLed;

        public Peach()
        {
            _redLed = new Led(RedPin);
            _greenLed = new Led(GreenPin);
            _blueLed = new Led(BluePin);
            _debugLed = new DebugLed(UserPin);

            _button = new Button(ButtonPin);
        }

        public Button Button { get { return _button; } }

        public void TurnAllLedsOff()
        {
            _redLed.SetLed(false);
            _greenLed.SetLed(false);
            _blueLed.SetLed(false);
            _debugLed.SetLed(false);
        }

        public void SetLeds(bool redOn, bool greenOn, bool blueOn)
        {
            _redLed.SetLed(redOn);
            _greenLed.SetLed(greenOn);
            _blueLed.SetLed(blueOn);
        }

        public void SetColor(Color color)
        {
            _redLed.SetLed((color & Color.Red) != 0);
            _greenLed.SetLed((color & Color.Green) != 0);
            _blueLed.SetLed((color & Color.Blue) != 0);
        }

        public void SetDebugLed(bool lightOn)
        {
            _debugLed.SetLed(lightOn);
        }

        public void PulseDebugLed()
        {
            _debugLed.PulseDebugLed();
        }

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
            Black = 0,
            Red = 1,
            Green = 2,
            Blue = 4,

            Yellow = Red + Green,
            Magenta = Red + Blue,
            Cyan = Green + Blue,
            White = Red + Green + Blue
        }
    }
}
