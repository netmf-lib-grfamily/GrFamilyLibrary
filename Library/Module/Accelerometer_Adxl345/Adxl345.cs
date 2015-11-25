using System;
using System.Threading;
using Microsoft.SPOT.Hardware;
using GrFamily.Module;

namespace GrFamily.Module
{
    public delegate void Adxl345MeasurementCompleteEventHandler(Adxl345 sender, Adxl345.MeasurementCompleteEventArgs e);

    public class Adxl345 : I2CDeviceEx
    {
        public event Adxl345MeasurementCompleteEventHandler MeasurementComplete;

        // ADXL345‚ÌI2CƒAƒhƒŒƒX
        private static readonly ushort AccelerometerAddress = 0x1d;

        private const byte PowerCtl = 0x2d;
        private const byte DataFormat = 0x31;
        private const byte DataX0 = 0x32;

        private Range _range;

        private byte[] _xyz = new byte[6];

        private readonly Timer _timer;

        public int Interval { get; set; } = -1;

        public Adxl345() : base(AccelerometerAddress)
        {
            _timer = new Timer(Measure_Timer, null, Timeout.Infinite, Timeout.Infinite);

            MeasurementRange = Range.FourG;
            ToWakeup();

            Thread.Sleep(10);
        }

        private void ToWakeup()
        {
            RegWriteMask(PowerCtl, 0x00, 0x04);
        }

        // D7: SELF_TEST
        // D6: SPI
        // D5: INT_INVERT
        // D4: 0
        // D3: FULL_RES
        // D2: Justfy
        // D1-D0: Range
        // 0 - 0: +-2g
        // 0 - 1: +-4g
        // 1 - 0: +-8g
        // 1 - 1: +-16g
        private void SetDataFormat(byte n)
        {
            RegWrite(DataFormat, n);
        }

        public Range MeasurementRange
        {
            get { return _range; }
            set
            {
                _range = value;
                SetDataFormat((byte)value);
            }
        }

        private void Measure()
        {
            RegWriteMask(PowerCtl, 0x08, 0x08);
        }

        public Int16 GetX()
        {
            Measure();
            RegReads(DataX0, ref _xyz);
            return (Int16)(((UInt16)_xyz[1] << 8) + (UInt16)_xyz[0]);
        }

        public Int16 GetY()
        {
            Measure();
            RegReads(DataX0, ref _xyz);
            return (Int16)(((UInt16)_xyz[3] << 8) + (UInt16)_xyz[2]);
        }

        public Int16 GetZ()
        {
            Measure();
            RegReads(DataX0, ref _xyz);
            return (Int16)(((UInt16)_xyz[5] << 8) + (UInt16)_xyz[4]);
        }

        public void GetXYZ(out Int16 x, out Int16 y, out Int16 z)
        {
            Measure();
            RegReads(DataX0, ref _xyz);
            x = (Int16)(((UInt16)_xyz[1] << 8) + (UInt16)_xyz[0]);
            y = (Int16)(((UInt16)_xyz[3] << 8) + (UInt16)_xyz[2]);
            z = (Int16)(((UInt16)_xyz[5] << 8) + (UInt16)_xyz[4]);
        }

        private void Measure_Timer(object state)
        {
            if (MeasurementComplete == null)
                return;

            Int16 x;
            Int16 y;
            Int16 z;
            GetXYZ(out x, out y, out z);

            MeasurementComplete(this, new MeasurementCompleteEventArgs() { X = x, Y = y, Z = z });
        }

        public void StartTakingMeasurements()
        {
            if (Interval > 0)
            {
                var ts = new TimeSpan(Interval * 10000);
                _timer.Change(ts, ts);
            }
        }

        public void StopTakingMeasurements()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public enum Range
        {
            TwoG = (byte)0x00,
            FourG = (byte)0x01,
            EightG = (byte)0x10
        }

        public class MeasurementCompleteEventArgs
        {
            public Int16 X;
            public Int16 Y;
            public Int16 Z;
        }
    }
}
