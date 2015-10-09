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
            var lcd = new LiquidCrystal(Peach.GpioPinD9, Peach.GpioPinD8,
                Peach.GpioPinD7, Peach.GpioPinD6, Peach.GpioPinD5, Peach.GpioPinD4);
            //// ピン変更テスト
            //var lcd = new LiquidCrystal(Peach.GpioPinD9, Peach.GpioPinD8,
            //    Peach.GpioPinD3, Peach.GpioPinD2, Peach.GpioPinD1, Peach.GpioPinD0);

            //// GR-SAKURAのテスト
            //var lcd = new LiquidCrystal(Sakura.GpioPinD9, Sakura.GpioPinD8,
            //    Sakura.GpioPinD7, Sakura.GpioPinD6, Sakura.GpioPinD5, Sakura.GpioPinD4);

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
