using System;
using System.Threading;
using Microsoft.SPOT;
using GrFamily.MainBoard;
using GrFamily.Module;
using Microsoft.SPOT.Hardware;

namespace LiquidCrystalTest
{
    public class Program
    {
        public static void Main()
        {
            // 開発時の標準ピンアサイン
            var lcd = new LiquidCrystal(Pins.GPIO_PIN_D9, Pins.GPIO_PIN_D8,
                Pins.GPIO_PIN_D7, Pins.GPIO_PIN_D6, Pins.GPIO_PIN_D5, Pins.GPIO_PIN_D4);
            // ピン変更テスト
            //var lcd = new LiquidCrystal(Pins.GPIO_PIN_D9, Pins.GPIO_PIN_D8,
            //    Pins.GPIO_PIN_D3, Pins.GPIO_PIN_D2, Pins.GPIO_PIN_D1, Pins.GPIO_PIN_D0);

            while (true)
            {
                lcd.InitDevice();
                lcd.Print("GRFamily Library");
                lcd.SetCursor(1, 0);
                lcd.Print("LiquidCrystal !!");

                Thread.Sleep(3000);

                lcd.SetCursor(0, 0);
                lcd.Print("Hello, NETMF !! ");
                lcd.SetCursor(1, 0);
                lcd.Print("Lib for GR-PEACH");

                Thread.Sleep(3000);
            }
        }
    }
}
