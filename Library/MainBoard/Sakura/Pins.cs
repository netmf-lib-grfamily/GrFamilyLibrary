using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public static class Pins
    {
        // GR-SAKURA のデジタル入出力のピン番号
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

        // GR-SAKURA のアナログ入出力のピン番号
        public const Cpu.Pin GpioPinA0 = (Cpu.Pin)48;
        public const Cpu.Pin GpioPinA1 = (Cpu.Pin)49;
        public const Cpu.Pin GpioPinA2 = (Cpu.Pin)50;
        public const Cpu.Pin GpioPinA3 = (Cpu.Pin)51;
        public const Cpu.Pin GpioPinA4 = (Cpu.Pin)52;
        public const Cpu.Pin GpioPinA5 = (Cpu.Pin)53;
    }
}
