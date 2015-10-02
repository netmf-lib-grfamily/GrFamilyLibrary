using System.Threading;
using GrFamily.ExternalBoard;
using GrFamily.MainBoard;
using Microsoft.SPOT;

namespace TemperatureTest
{
    public class Program
    {
        public static void Main()
        {
            var peach = new Peach();
            var temperature = (new SensorBoard((IMainBoard)peach)).Temperature;

            while (true)
            {
                var temp = temperature.TakeMeasurement();
                Debug.Print(temp.ToString());
                Thread.Sleep(3000);
            }
        }
    }
}
