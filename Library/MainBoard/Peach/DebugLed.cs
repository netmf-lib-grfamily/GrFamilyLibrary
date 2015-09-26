using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public class DebugLed : Led
    {
        public DebugLed(Cpu.Pin pin) : base(pin)
        {
        }

        public void PulseDebugLed()
        {
            (new Thread(() =>
            {
                SetLed(true);
                Thread.Sleep(100);
                SetLed(false);
            })).Start();
        }

        public void PulseDebugLed(int length, int times)
        {
            (new Thread(() =>
            {
                for (var i = 0; i < times; i++)
                {
                    SetLed(true);
                    Thread.Sleep(length);
                    SetLed(false);
                    Thread.Sleep(length);
                }
            })).Start();
        }
    }
}
