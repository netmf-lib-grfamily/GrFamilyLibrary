using System;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public class Peach : IMainBoard
    {
        // デジタル入出力のピン番号
        public const Cpu.Pin GpioPinD0 = (Cpu.Pin)0x2f;
        public const Cpu.Pin GpioPinD1 = (Cpu.Pin)0x2e;
        public const Cpu.Pin GpioPinD2 = (Cpu.Pin)0x47;
        public const Cpu.Pin GpioPinD3 = (Cpu.Pin)0x46;
        public const Cpu.Pin GpioPinD4 = (Cpu.Pin)0x45;
        public const Cpu.Pin GpioPinD5 = (Cpu.Pin)0x44;
        public const Cpu.Pin GpioPinD6 = (Cpu.Pin)0x8d;
        public const Cpu.Pin GpioPinD7 = (Cpu.Pin)0x8b;
        public const Cpu.Pin GpioPinD8 = (Cpu.Pin)0x8f;
        public const Cpu.Pin GpioPinD9 = (Cpu.Pin)0x8e;
        public const Cpu.Pin GpioPinD10 = (Cpu.Pin)0xad;
        public const Cpu.Pin GpioPinD11 = (Cpu.Pin)0xae;
        public const Cpu.Pin GpioPinD12 = (Cpu.Pin)0xaf;
        public const Cpu.Pin GpioPinD13 = (Cpu.Pin)0xac;
        public const Cpu.Pin GpioPinD14 = (Cpu.Pin)0x13;
        public const Cpu.Pin GpioPinD15 = (Cpu.Pin)0x12;

        // アナログ入出力のピン番号
        public const Cpu.Pin GpioPinA0 = (Cpu.Pin)0x18;
        public const Cpu.Pin GpioPinA1 = (Cpu.Pin)0x19;
        public const Cpu.Pin GpioPinA2 = (Cpu.Pin)0x1a;
        public const Cpu.Pin GpioPinA3 = (Cpu.Pin)0x1b;
        public const Cpu.Pin GpioPinA4 = (Cpu.Pin)0x1d;
        public const Cpu.Pin GpioPinA5 = (Cpu.Pin)0x1f;

        // Buttonのピン番号
        private const Cpu.Pin ButtonPin = (Cpu.Pin)0x60;

        // 各 LED のピン番号
        private const Cpu.Pin RedPin = (Cpu.Pin)0x6d;       // 赤
        private const Cpu.Pin GreenPin = (Cpu.Pin)0x6e;     // 緑
        private const Cpu.Pin BluePin = (Cpu.Pin)0x6f;      // 青
        private const Cpu.Pin UserPin = (Cpu.Pin)0x6c;      // ユーザー

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

            Button = new Button(ButtonPin);
        }

        public Button Button { get; }

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

        public Cpu.Pin DigitalPin(int pin)
        {
            switch (pin)
            {
                case 0:
                    return GpioPinD0;
                case 1:
                    return GpioPinD1;
                case 2:
                    return GpioPinD2;
                case 3:
                    return GpioPinD3;
                case 4:
                    return GpioPinD4;
                case 5:
                    return GpioPinD5;
                case 6:
                    return GpioPinD6;
                case 7:
                    return GpioPinD7;
                case 8:
                    return GpioPinD8;
                case 9:
                    return GpioPinD9;
                case 10:
                    return GpioPinD10;
                case 11:
                    return GpioPinD11;
                case 12:
                    return GpioPinD12;
                case 13:
                    return GpioPinD13;
                case 14:
                    return GpioPinD14;
                case 15:
                    return GpioPinD15;
                default:
                    throw new ArgumentException();
            }
        }

        public Cpu.Pin AnalogPin(int port)
        {
            switch (port)
            {
                case 0:
                    return GpioPinA0;
                case 1:
                    return GpioPinA1;
                case 2:
                    return GpioPinA2;
                case 3:
                    return GpioPinA3;
                case 4:
                    return GpioPinA4;
                case 5:
                    return GpioPinA5;
                default:
                    throw new ArgumentException();
            }
        }

        public Cpu.AnalogChannel AnalogChannel(int port)
        {
            switch (port)
            {
                case 0:
                    return (Cpu.AnalogChannel) 0;
                case 1:
                    return (Cpu.AnalogChannel) 1;
                case 2:
                    return (Cpu.AnalogChannel) 2;
                case 3:
                    return (Cpu.AnalogChannel) 3;
                case 4:
                    return (Cpu.AnalogChannel) 5;
                case 5:
                    return (Cpu.AnalogChannel) 7;
                default:
                    throw new ArgumentException();
            }
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
