using System.Threading;
using GrFamily.Module;

namespace LiquidCrystalTest
{
    public class LcdTestRunner
    {
        private readonly LiquidCrystal _lcd;

        public LcdTestRunner(LiquidCrystal lcd)
        {
            _lcd = lcd;
        }

        public void Run()
        {
            _lcd.Clear();
            _lcd.Print("LCD Test Start !");
            for (int i = 5; i > 0; i--)
            {
                _lcd.SetCursor(1, 0);
                _lcd.Print(i.ToString());
                Thread.Sleep(1000);
            }

            _lcd.Clear();
            _lcd.Print("Display Test");
            for (int i = 0; i < 3; i++)
            {
                _lcd.DisplayOn(false);
                Thread.Sleep(2000);
                _lcd.DisplayOn(true);
                Thread.Sleep(2000);
            }

            _lcd.Clear();
            _lcd.Print("Cursor Test");
            for (int i = 0; i < 3; i++)
            {
                _lcd.CursorOn(true);
                Thread.Sleep(2000);
                _lcd.CursorOn(false);
                Thread.Sleep(2000);
            }

            _lcd.Clear();
            _lcd.Print("Blink Test");
            for (int i = 0; i < 3; i++)
            {
                _lcd.BlinkOn(true);
                Thread.Sleep(2000);
                _lcd.BlinkOn(false);
                Thread.Sleep(2000);
            }

            _lcd.Clear();
            _lcd.SetCursor(0, 2);
            _lcd.Print("SetCursor 0, 2");
            _lcd.SetCursor(1, 4);
            _lcd.Print("1, 4");
            Thread.Sleep(3000);

            _lcd.DisplayOn(true);
            _lcd.CursorOn(false);
            _lcd.BlinkOn(false);
            while (true)
            {
                PrintLines("GRFamily Library", "LiquidCrystal !!");
                Thread.Sleep(2000);
                PrintLines("Hello, NETMF !! ", "Lib for GR-PEACH");
                Thread.Sleep(2000);
            }
        }

        private void PrintLines(string line1, string line2)
        {
            _lcd.SetCursor(0, 0);
            _lcd.Print(line1);
            _lcd.SetCursor(1, 0);
            _lcd.Print(line2);
        }
    }
}
