using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public static class Pins
    {
        // GR-SAKURA のデジタル入出力のピン番号
        public const Cpu.Pin GPIO_PIN_D0 = (Cpu.Pin)17;
        public const Cpu.Pin GPIO_PIN_D1 = (Cpu.Pin)16;
        public const Cpu.Pin GPIO_PIN_D2 = (Cpu.Pin)18;
        public const Cpu.Pin GPIO_PIN_D3 = (Cpu.Pin)19;
        public const Cpu.Pin GPIO_PIN_D4 = (Cpu.Pin)20;
        public const Cpu.Pin GPIO_PIN_D5 = (Cpu.Pin)21;
        public const Cpu.Pin GPIO_PIN_D6 = (Cpu.Pin)26;
        public const Cpu.Pin GPIO_PIN_D7 = (Cpu.Pin)27;
        public const Cpu.Pin GPIO_PIN_D8 = (Cpu.Pin)98;
        public const Cpu.Pin GPIO_PIN_D9 = (Cpu.Pin)99;
        public const Cpu.Pin GPIO_PIN_D10 = (Cpu.Pin)100;
        public const Cpu.Pin GPIO_PIN_D11 = (Cpu.Pin)102;
        public const Cpu.Pin GPIO_PIN_D12 = (Cpu.Pin)103;
        public const Cpu.Pin GPIO_PIN_D13 = (Cpu.Pin)101;

        // GR-SAKURA のアナログ入出力のピン番号
        public const Cpu.Pin GPIO_PIN_A0 = (Cpu.Pin)48;
        public const Cpu.Pin GPIO_PIN_A1 = (Cpu.Pin)49;
        public const Cpu.Pin GPIO_PIN_A2 = (Cpu.Pin)50;
        public const Cpu.Pin GPIO_PIN_A3 = (Cpu.Pin)51;
        public const Cpu.Pin GPIO_PIN_A4 = (Cpu.Pin)52;
        public const Cpu.Pin GPIO_PIN_A5 = (Cpu.Pin)53;

        // GR-SAKURA のアナログチャンネル
        public const Cpu.AnalogChannel ANALOG_0 = (Cpu.AnalogChannel)0;
        public const Cpu.AnalogChannel ANALOG_1 = (Cpu.AnalogChannel)1;
        public const Cpu.AnalogChannel ANALOG_2 = (Cpu.AnalogChannel)2;
        public const Cpu.AnalogChannel ANALOG_3 = (Cpu.AnalogChannel)3;
        public const Cpu.AnalogChannel ANALOG_4 = (Cpu.AnalogChannel)4;
        public const Cpu.AnalogChannel ANALOG_5 = (Cpu.AnalogChannel)5;
    }
}
