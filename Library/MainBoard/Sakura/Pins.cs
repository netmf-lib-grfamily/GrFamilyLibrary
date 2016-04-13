using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    /// <summary>
    /// GR-SAKURAのデジタル入出力のピン番号定義
    /// </summary>
    public static class Pins
    {
        /// <summary>デジタル入出力用 D0ピン</summary>
        public const Cpu.Pin GPIO_PIN_D0 = (Cpu.Pin)17;
        /// <summary>デジタル入出力用 D1ピン</summary>
        public const Cpu.Pin GPIO_PIN_D1 = (Cpu.Pin)16;
        /// <summary>デジタル入出力用 D2ピン</summary>
        public const Cpu.Pin GPIO_PIN_D2 = (Cpu.Pin)18;
        /// <summary>デジタル入出力用 D3ピン</summary>
        public const Cpu.Pin GPIO_PIN_D3 = (Cpu.Pin)19;
        /// <summary>デジタル入出力用 D4ピン</summary>
        public const Cpu.Pin GPIO_PIN_D4 = (Cpu.Pin)20;
        /// <summary>デジタル入出力用 D5ピン</summary>
        public const Cpu.Pin GPIO_PIN_D5 = (Cpu.Pin)21;
        /// <summary>デジタル入出力用 D6ピン</summary>
        public const Cpu.Pin GPIO_PIN_D6 = (Cpu.Pin)26;
        /// <summary>デジタル入出力用 D7ピン</summary>
        public const Cpu.Pin GPIO_PIN_D7 = (Cpu.Pin)27;
        /// <summary>デジタル入出力用 D8ピン</summary>
        public const Cpu.Pin GPIO_PIN_D8 = (Cpu.Pin)98;
        /// <summary>デジタル入出力用 D9ピン</summary>
        public const Cpu.Pin GPIO_PIN_D9 = (Cpu.Pin)99;
        /// <summary>デジタル入出力用 D10ピン</summary>
        public const Cpu.Pin GPIO_PIN_D10 = (Cpu.Pin)100;
        /// <summary>デジタル入出力用 D11ピン</summary>
        public const Cpu.Pin GPIO_PIN_D11 = (Cpu.Pin)102;
        /// <summary>デジタル入出力用 D12ピン</summary>
        public const Cpu.Pin GPIO_PIN_D12 = (Cpu.Pin)103;
        /// <summary>デジタル入出力用 D13ピン</summary>
        public const Cpu.Pin GPIO_PIN_D13 = (Cpu.Pin)101;

        /// <summary>アナログ入出力用 A0ピン</summary>
        public const Cpu.Pin GPIO_PIN_A0 = (Cpu.Pin)48;
        /// <summary>アナログ入出力用 A1ピン</summary>
        public const Cpu.Pin GPIO_PIN_A1 = (Cpu.Pin)49;
        /// <summary>アナログ入出力用 A2ピン</summary>
        public const Cpu.Pin GPIO_PIN_A2 = (Cpu.Pin)50;
        /// <summary>アナログ入出力用 A3ピン</summary>
        public const Cpu.Pin GPIO_PIN_A3 = (Cpu.Pin)51;
        /// <summary>アナログ入出力用 A4ピン</summary>
        public const Cpu.Pin GPIO_PIN_A4 = (Cpu.Pin)52;
        /// <summary>アナログ入出力用 A5ピン</summary>
        public const Cpu.Pin GPIO_PIN_A5 = (Cpu.Pin)53;

        // GR-SAKURA のアナログチャンネル
        /// <summary>アナログチャンネル0 (A0用アナログチャンネル)</summary>
        public const Cpu.AnalogChannel ANALOG_0 = (Cpu.AnalogChannel)0;
        /// <summary>アナログチャンネル1 (A1用アナログチャンネル)</summary>
        public const Cpu.AnalogChannel ANALOG_1 = (Cpu.AnalogChannel)1;
        /// <summary>アナログチャンネル2 (A2用アナログチャンネル)</summary>
        public const Cpu.AnalogChannel ANALOG_2 = (Cpu.AnalogChannel)2;
        /// <summary>アナログチャンネル3 (A3用アナログチャンネル)</summary>
        public const Cpu.AnalogChannel ANALOG_3 = (Cpu.AnalogChannel)3;
        /// <summary>アナログチャンネル4 (A4用アナログチャンネル)</summary>
        public const Cpu.AnalogChannel ANALOG_4 = (Cpu.AnalogChannel)4;
        /// <summary>アナログチャンネル5 (A5用アナログチャンネル)</summary>
        public const Cpu.AnalogChannel ANALOG_5 = (Cpu.AnalogChannel)5;
    }
}
