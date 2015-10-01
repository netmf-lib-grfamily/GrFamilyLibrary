using System;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public class Peach : IPortDefinitions
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

        public Cpu.Pin GetDigitalPin(int port)
        {
            switch (port)
            {
                case 0:
                    return (Cpu.Pin)0x2f;
                case 1:
                    return (Cpu.Pin)0x2e;
                case 2:
                    return (Cpu.Pin)0x47;
                case 3:
                    return (Cpu.Pin)0x46;
                case 4:
                    return (Cpu.Pin)0x45;
                case 5:
                    return (Cpu.Pin)0x44;
                case 6:
                    return (Cpu.Pin)0x8d;
                case 7:
                    return (Cpu.Pin)0x8b;
                case 8:
                    return (Cpu.Pin)0x8f;
                case 9:
                    return (Cpu.Pin)0x8e;
                case 10:
                    return (Cpu.Pin)0xad;
                case 11:
                    return (Cpu.Pin)0xae;
                case 12:
                    return (Cpu.Pin)0xaf;
                case 13:
                    return (Cpu.Pin)0xac;
                case 14:
                    return (Cpu.Pin)0x13;
                case 15:
                    return (Cpu.Pin)0x12;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Cpu.Pin GetAnalogPin(int port)
        {
            switch (port)
            {
                case 0:
                    return (Cpu.Pin)0x18;
                case 1:
                    return (Cpu.Pin)0x19;
                case 2:
                    return (Cpu.Pin)0x1a;
                case 3:
                    return (Cpu.Pin)0x1b;
                case 4:
                    return (Cpu.Pin)0x1d;
                case 5:
                    return (Cpu.Pin)0x1f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
