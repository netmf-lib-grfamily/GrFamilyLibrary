using System.Threading;
using GrFamily.MainBoard;
using Microsoft.SPOT;

namespace PeachButtonPressTest
{
    public class Program
    {
        private static Peach _peach;

        public static void Main()
        {
            _peach = new Peach();

            while (true)
            {
                if (_peach.Button.IsPressed)
                {
                    _peach.SetDebugLed(true);
                    Debug.Print("Button Pressed");
                }
                else
                {
                    _peach.SetDebugLed(false);
                    Debug.Print("Button Released");
                }

                Thread.Sleep(200);
            }
        }
    }
}
