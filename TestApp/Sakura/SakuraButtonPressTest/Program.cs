using System.Threading;
using GrFamily.MainBoard;
using Microsoft.SPOT;

namespace SakuraButtonPressTest
{
    public class Program
    {
        private static Sakura _sakura;

        public static void Main()
        {
            _sakura = new Sakura();

            while (true)
            {
                if (_sakura.Button.IsPressed)
                {
                    _sakura.SetDebugLed(true);
                    Debug.Print("Button Pressed");
                }
                else
                {
                    _sakura.SetDebugLed(false);
                    Debug.Print("Button Released");
                }

                Thread.Sleep(200);
            }
        }
    }
}
