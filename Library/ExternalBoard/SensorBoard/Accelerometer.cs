using System;
using Microsoft.SPOT.Hardware;

namespace GrFamily.ExternalBoard
{
    public class Accelerometer
    {
        private const byte PowerCtl = 0x2d;
        private const byte DataFormat = 0x31;
        private const byte DataX0 = 0x32;

        private readonly I2CDevice _i2C;
        private readonly int _timeout;
        private readonly byte[] _adata = new byte[1];
        private readonly byte[] _rdata = new byte[1];
        private readonly byte[] _wdata = new byte[2];

        private I2CDevice.I2CTransaction[] _trRegRead;
        private I2CDevice.I2CTransaction[] _trRegWrite;

        private byte[] _xyz = new byte[6];

        internal Accelerometer(I2CDevice i2C, int timeout)
        {
            _i2C = i2C;
            _timeout = timeout;
            MeasurementRange = Range.FourG;

            ToWakeup();
            
            System.Threading.Thread.Sleep(10);
        }

        public void Measure()
        {
            RegWriteMask(PowerCtl, 0x08, 0x08);
        }

        public Int16 GetX()
        {
            RegReads(DataX0, ref _xyz);
            return (Int16)(((UInt16)_xyz[1] << 8) + (UInt16)_xyz[0]);
        }

        public Int16 GetY()
        {
            RegReads(DataX0, ref _xyz);
            return (Int16)(((UInt16)_xyz[3] << 8) + (UInt16)_xyz[2]);
        }

        public Int16 GetZ()
        {
            RegReads(DataX0, ref _xyz);
            return (Int16)(((UInt16)_xyz[5] << 8) + (UInt16)_xyz[4]);
        }

        public void GetXYZ(out Int16 x, out Int16 y, out Int16 z)
        {
            RegReads(DataX0, ref _xyz);
            x = (Int16)(((UInt16)_xyz[1] << 8) + (UInt16)_xyz[0]);
            y = (Int16)(((UInt16)_xyz[3] << 8) + (UInt16)_xyz[2]);
            z = (Int16)(((UInt16)_xyz[5] << 8) + (UInt16)_xyz[4]);
        }

        private Range _range;

        public Range MeasurementRange
        {
            get { return _range; }
            set
            {
                _range = value;
                SetDataFormat((byte)value);
            }
        }

        public void ToWakeup()
        {
            RegWriteMask(PowerCtl, 0x00, 0x04);
        }

        public byte RegRead(byte reg)
        {
            _adata[0] = reg;
            _trRegRead = new I2CDevice.I2CTransaction[] { 
                I2CDevice.CreateWriteTransaction(_adata),
                I2CDevice.CreateReadTransaction(_rdata) };
            _i2C.Execute(_trRegRead, _timeout);
            return _rdata[0];
        }

        public void RegReads(byte reg, ref byte[] data)
        {
            _adata[0] = reg;
            _trRegRead = new I2CDevice.I2CTransaction[] { 
                I2CDevice.CreateWriteTransaction(_adata),
                I2CDevice.CreateReadTransaction(data) };
            _i2C.Execute(_trRegRead, _timeout);
        }

        public void RegWrite(byte reg, byte val)
        {
            _wdata[0] = reg;
            _wdata[1] = val;
            _trRegWrite = new I2CDevice.I2CTransaction[] { I2CDevice.CreateWriteTransaction(_wdata) };
            _i2C.Execute(_trRegWrite, _timeout);
        }

        public void RegWriteMask(byte reg, byte val, byte mask)
        {
            var tmp = RegRead(reg);
            _wdata[0] = reg;
            _wdata[1] = (byte)(((int)tmp & ~(int)mask) | ((int)val & (int)mask));
            _trRegWrite = new I2CDevice.I2CTransaction[] { I2CDevice.CreateWriteTransaction(_wdata) };
            _i2C.Execute(_trRegWrite, _timeout);
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

        public enum Range
        {
            TwoG = (byte)0x00,
            FourG = (byte)0x01,
            EightG = (byte)0x10
        }
    }
}
