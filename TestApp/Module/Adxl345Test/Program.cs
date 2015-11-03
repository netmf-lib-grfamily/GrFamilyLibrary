using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GrFamily.MainBoard;
using GrFamily.Module;

namespace Adxl345Test
{
    public class Program
    {
        public static void Main()
        {
            var accel = new Adxl345 {MeasurementRange = Adxl345.Range.FourG};

            while (true)
            {
                short x;
                short y;
                short z;

                accel.GetXYZ(out x, out y, out z);
                Debug.Print("x = " + x.ToString() + ", y = " + y.ToString() + ", z = " + z.ToString());

                Thread.Sleep(3000);
            }
        }
    }
}
