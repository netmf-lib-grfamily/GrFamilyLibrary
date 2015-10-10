using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public class Sakura
    {
        // Button‚Ìƒsƒ“”Ô†
        private const Cpu.Pin ButtonPin = (Cpu.Pin)0x57;

        // Še LED ‚Ìƒsƒ“”Ô†
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
    }
}
