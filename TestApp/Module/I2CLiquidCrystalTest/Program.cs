using System.Threading;
using GrFamily.Module;

namespace I2CLiquidCrystalTest
{
    public class Program
    {
        private static readonly ushort _acm1602n1Address = 0x50; // ACM1602N1-FLW-FBW‚ÌƒAƒhƒŒƒX

        public static void Main()
        {
            var prog = new Program();
            prog.Run(_acm1602n1Address);
        }

        private void Run(ushort i2CAddress)
        {
            var display = new I2CLiquidCrystal(i2CAddress);

            while (true)
            {
                display.Print("Hello, I2C CLCD!");
                display.SetCursor(1, 1);
                display.Print("GR Family Lib");
                Thread.Sleep(3000);
                display.Home();
                Thread.Sleep(3000);

                display.BlinkOn(false);
                Thread.Sleep(3000);
                display.BlinkOn(true);
                display.CursorOn(false);
                Thread.Sleep(3000);
                display.CursorOn(true);
                Thread.Sleep(3000);

                display.BlinkOn(false);
                display.CursorOn(false);
                display.DisplayOn(false);
                Thread.Sleep(1000);
                display.DisplayOn(true);
                Thread.Sleep(1000);
                display.DisplayOn(false);
                Thread.Sleep(1000);
                display.DisplayOn(true);
                Thread.Sleep(1000);
                display.BlinkOn(true);
                display.CursorOn(true);

                display.Clear();
                Thread.Sleep(3000);
            }
        }
    }
}
