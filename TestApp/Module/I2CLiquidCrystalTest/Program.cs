#define AQM0802A

using System.Threading;
using GrFamily.Module;

namespace I2CLiquidCrystalTest
{
    public class Program
    {
#if AQM0802A
        private static readonly ushort I2CAddress = 0x3e;     // AQM0802A-RN-GBWのアドレス
        private readonly string _msgLine1 = "GrFamily";
        private readonly string _msgLine2 = "I2CLib";
#else
        private static readonly ushort I2CAddress = 0x50;     // ACM1602N1-FLW-FBWのアドレス
        private readonly string _msgLine1 = "Hello, I2C CLCD!";
        private readonly string _msgLine2 = "GR Family Lib";
#endif


        public static void Main()
        {
            var prog = new Program();
            prog.Run();
        }

        private void Run()
        {
#if AQM0802A
            var display = new I2CLiquidCrystal(I2CAddress, true, true);
#else
            var display = new I2CLiquidCrystal(I2CAddress);
#endif

            while (true)
            {
                display.Print(_msgLine1);
                display.SetCursor(1, 1);
                display.Print(_msgLine2);
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
