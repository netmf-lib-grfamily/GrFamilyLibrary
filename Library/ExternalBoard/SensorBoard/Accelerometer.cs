using System;
using System.Threading;
using Microsoft.SPOT.Hardware;
using GrFamily.Module;

namespace GrFamily.ExternalBoard
{
    public class Accelerometer : I2CDeviceEx
    {
        private const byte PowerCtl = 0x2d;
        private const byte DataFormat = 0x31;
        private const byte DataX0 = 0x32;

        private Range _range;

        private byte[] _xyz = new byte[6];

        internal Accelerometer(ushort i2CAddress) : base(i2CAddress)
        {
            MeasurementRange = Range.FourG;

            ToWakeup();
            
            Thread.Sleep(10);
        }

        public void ToWakeup()
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
        public void SetDataFormat(byte n)
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

        public enum Range
        {
            TwoG = (byte)0x00,
            FourG = (byte)0x01,
            EightG = (byte)0x10
        }
    }
}
