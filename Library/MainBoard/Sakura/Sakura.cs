using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public class Sakura : IMainBoard
    {
        // デジタル入出力のピン番号
        public const Cpu.Pin GpioPinD0 = (Cpu.Pin)17;
        public const Cpu.Pin GpioPinD1 = (Cpu.Pin)16;
        public const Cpu.Pin GpioPinD2 = (Cpu.Pin)18;
        public const Cpu.Pin GpioPinD3 = (Cpu.Pin)19;
        public const Cpu.Pin GpioPinD4 = (Cpu.Pin)20;
        public const Cpu.Pin GpioPinD5 = (Cpu.Pin)21;
        public const Cpu.Pin GpioPinD6 = (Cpu.Pin)26;
        public const Cpu.Pin GpioPinD7 = (Cpu.Pin)27;
        public const Cpu.Pin GpioPinD8 = (Cpu.Pin)98;
        public const Cpu.Pin GpioPinD9 = (Cpu.Pin)99;
        public const Cpu.Pin GpioPinD10 = (Cpu.Pin)100;
        public const Cpu.Pin GpioPinD11 = (Cpu.Pin)102;
        public const Cpu.Pin GpioPinD12 = (Cpu.Pin)103;
        public const Cpu.Pin GpioPinD13 = (Cpu.Pin)101;

        // アナログ入出力のピン番号
        public const Cpu.Pin GpioPinA0 = (Cpu.Pin)48;
        public const Cpu.Pin GpioPinA1 = (Cpu.Pin)49;
        public const Cpu.Pin GpioPinA2 = (Cpu.Pin)50;
        public const Cpu.Pin GpioPinA3 = (Cpu.Pin)51;
        public const Cpu.Pin GpioPinA4 = (Cpu.Pin)52;
        public const Cpu.Pin GpioPinA5 = (Cpu.Pin)53;

        // Buttonのピン番号
        private const Cpu.Pin ButtonPin = (Cpu.Pin)0x57;

        // 各 LED のピン番号
        private const Cpu.Pin Led1Pin = (Cpu.Pin)0x50;
        private const Cpu.Pin Led2Pin = (Cpu.Pin)0x51;
        private const Cpu.Pin Led3Pin = (Cpu.Pin)0x52;
        private const Cpu.Pin Led4Pin = (Cpu.Pin)0x56;

        private readonly Led _led1;
        private readonly Led _led2;
        private readonly Led _led3;
        private readonly DebugLed _debugLed;

        public Sakura()
        {
            _led1 = new Led(Led1Pin);
            _led2 = new Led(Led2Pin);
            _led3 = new Led(Led3Pin);
            _debugLed = new DebugLed(Led4Pin);

            Button = new Button(ButtonPin);
        }

        public Button Button { get; }

        public void TurnAllLedsOff()
        {
            _led1.SetLed(false);
            _led2.SetLed(false);
            _led3.SetLed(false);
            _debugLed.SetLed(false);
        }

        public void SetLeds(bool led1On, bool led2On, bool led3On, bool led4On)
        {
            _led1.SetLed(led1On);
            _led2.SetLed(led2On);
            _led3.SetLed(led3On);
            _debugLed.SetLed(led4On);
        }

        public void SetLeds(int count)
        {
            SetLeds(count >= 1, count >= 2, count >= 3, count >= 4);
        }

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

        private void SetLedsInner(int count, bool remain)
        {
            var led1 = remain ? count >= 1 : count == 1;
            var led2 = remain ? count >= 2 : count == 2;
            var led3 = remain ? count >= 3 : count == 3;
            var led4 = remain ? count >= 4 : count == 4;

            SetLeds(led1, led2, led3, led4);
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
                    return (Cpu.AnalogChannel)0;
                case 1:
                    return (Cpu.AnalogChannel)1;
                case 2:
                    return (Cpu.AnalogChannel)2;
                case 3:
                    return (Cpu.AnalogChannel)3;
                case 4:
                    return (Cpu.AnalogChannel)5;
                case 5:
                    return (Cpu.AnalogChannel)7;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
