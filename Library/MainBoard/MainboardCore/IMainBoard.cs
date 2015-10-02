using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public interface IMainBoard
    {
        Cpu.Pin DigitalPin(int pin);

        Cpu.Pin AnalogPin(int port);

        Cpu.AnalogChannel AnalogChannel(int port);
    }
}
