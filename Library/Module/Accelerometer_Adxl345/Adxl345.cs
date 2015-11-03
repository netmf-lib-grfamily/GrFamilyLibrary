using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace GrFamily.Module
{
    public class Adxl345
    {
        // 加速度センサーのI2Cアドレス
        private readonly ushort _accelerometerAddress = 0x1d;

        private const byte PowerCtl = 0x2d;
        private const byte DataFormat = 0x31;
        private const byte DataX0 = 0x32;

        private const int DefaultClockRateKhz = 100;
        private const int DefaultTimeout = 1000;

        private readonly I2CDevice _i2C;
        private readonly int _timeout;
        private readonly byte[] _adata = new byte[1];
        private readonly byte[] _rdata = new byte[1];
        private readonly byte[] _wdata = new byte[2];

        private I2CDevice.I2CTransaction[] _trRegRead;
        private I2CDevice.I2CTransaction[] _trRegWrite;

        private Range _range;

        private byte[] _xyz = new byte[6];

        public Adxl345()
        {
            _i2C = new I2CDevice(new I2CDevice.Configuration(_accelerometerAddress, DefaultClockRateKhz));
            _timeout = DefaultTimeout;
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

        public enum Range
        {
            TwoG = (byte)0x00,
            FourG = (byte)0x01,
            EightG = (byte)0x10
        }
    }
}
