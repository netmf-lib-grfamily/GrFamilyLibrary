using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Time;
using GrFamily.Utility;

namespace SystemTimeInitializerTest
{
    public class Program
    {
        private Timer _timer = new Timer(_timer_tick, null, Timeout.Infinite, Timeout.Infinite);

        public static void Main()
        {
            var prog = new Program();
            prog.Run();
        }

        private void Run()
        {
            var ipAddress = NetworkUtility.InitNetwork();
            if (ipAddress == "0.0.0.0")
            {
                Debug.Print("Cannot Get IP Address.");
                return;
            }

            SystemTimeInitializer.InitSystemTime();

            _timer.Change(new TimeSpan(0, 0, 1), new TimeSpan(0, 0, 1));

            while (true) { }
        }

        private static void _timer_tick(object state)
        {
            Debug.Print(DateTime.Now.ToString());
        }
    }
}
