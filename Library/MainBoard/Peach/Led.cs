using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public class Led
    {
        protected readonly OutputPort LedPort;

        public Led(Cpu.Pin pin)
        {
            LedPort = new OutputPort(pin, false);
        }

        public void SetLed(bool on)
        {
            LedPort.Write(on);
        }
    }
}
