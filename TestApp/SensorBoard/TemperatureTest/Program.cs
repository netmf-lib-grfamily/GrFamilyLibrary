using System.Threading;
using GrFamily.ExternalBoard;
using Microsoft.SPOT;

namespace TemperatureTest
{
    public class Program
    {
        public static void Main()
        {
            var temperature = (new SensorBoard()).Temperature;

            while (true)
            {
                var temp = temperature.TakeMeasurement();
                Debug.Print(temp.ToString());
                Thread.Sleep(3000);
            }
        }
    }
}
