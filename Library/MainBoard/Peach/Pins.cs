using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public static class Pins
    {
        // GR-PEACH のデジタル入出力のピン番号
        public const Cpu.Pin GPIO_PIN_D0 = (Cpu.Pin)0x2f;
        public const Cpu.Pin GPIO_PIN_D1 = (Cpu.Pin)0x2e;
        public const Cpu.Pin GPIO_PIN_D2 = (Cpu.Pin)0x47;
        public const Cpu.Pin GPIO_PIN_D3 = (Cpu.Pin)0x46;
        public const Cpu.Pin GPIO_PIN_D4 = (Cpu.Pin)0x45;
        public const Cpu.Pin GPIO_PIN_D5 = (Cpu.Pin)0x44;
        public const Cpu.Pin GPIO_PIN_D6 = (Cpu.Pin)0x8d;
        public const Cpu.Pin GPIO_PIN_D7 = (Cpu.Pin)0x8b;
        public const Cpu.Pin GPIO_PIN_D8 = (Cpu.Pin)0x8f;
        public const Cpu.Pin GPIO_PIN_D9 = (Cpu.Pin)0x8e;
        public const Cpu.Pin GPIO_PIN_D10 = (Cpu.Pin)0xad;
        public const Cpu.Pin GPIO_PIN_D11 = (Cpu.Pin)0xae;
        public const Cpu.Pin GPIO_PIN_D12 = (Cpu.Pin)0xaf;
        public const Cpu.Pin GPIO_PIN_D13 = (Cpu.Pin)0xac;
        public const Cpu.Pin GPIO_PIN_D14 = (Cpu.Pin)0x13;
        public const Cpu.Pin GPIO_PIN_D15 = (Cpu.Pin)0x12;

        // GR-PEACH のアナログ入出力のピン番号
        public const Cpu.Pin GPIO_PIN_A0 = (Cpu.Pin)0x18;
        public const Cpu.Pin GPIO_PIN_A1 = (Cpu.Pin)0x19;
        public const Cpu.Pin GPIO_PIN_A2 = (Cpu.Pin)0x1a;
        public const Cpu.Pin GPIO_PIN_A3 = (Cpu.Pin)0x1b;
        public const Cpu.Pin GPIO_PIN_A4 = (Cpu.Pin)0x1d;
        public const Cpu.Pin GPIO_PIN_A5 = (Cpu.Pin)0x1f;

        // GR-PEACH のアナログチャンネル
        public const Cpu.AnalogChannel ANALOG_0 = (Cpu.AnalogChannel)0;
        public const Cpu.AnalogChannel ANALOG_1 = (Cpu.AnalogChannel)1;
        public const Cpu.AnalogChannel ANALOG_2 = (Cpu.AnalogChannel)2;
        public const Cpu.AnalogChannel ANALOG_3 = (Cpu.AnalogChannel)3;
        public const Cpu.AnalogChannel ANALOG_4 = (Cpu.AnalogChannel)5;
        public const Cpu.AnalogChannel ANALOG_5 = (Cpu.AnalogChannel)7;
    }
}
