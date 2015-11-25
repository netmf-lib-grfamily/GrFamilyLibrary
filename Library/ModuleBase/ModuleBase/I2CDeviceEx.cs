using Microsoft.SPOT.Hardware;

namespace GrFamily.Module
{
    public class I2CDeviceEx
    {
        private readonly I2CDevice _i2C;
        private readonly int _timeout;
        private readonly byte[] _adata = new byte[1];
        private readonly byte[] _rdata = new byte[1];
        private readonly byte[] _wdata = new byte[2];

        private I2CDevice.I2CTransaction[] _trRegRead;
        private I2CDevice.I2CTransaction[] _trRegWrite;

        public I2CDeviceEx(ushort i2CAddress, int clockRateKhz = 100, int timeout = 1000)
        {
            _i2C = new I2CDevice(new I2CDevice.Configuration(i2CAddress, clockRateKhz));
            _timeout = timeout;

        }

        protected byte RegRead(byte reg)
        {
            _adata[0] = reg;
            _trRegRead = new I2CDevice.I2CTransaction[] {
                I2CDevice.CreateWriteTransaction(_adata),
                I2CDevice.CreateReadTransaction(_rdata) };
            _i2C.Execute(_trRegRead, _timeout);
            return _rdata[0];
        }

        protected void RegReads(byte reg, ref byte[] data)
        {
            _adata[0] = reg;
            _trRegRead = new I2CDevice.I2CTransaction[] {
                I2CDevice.CreateWriteTransaction(_adata),
                I2CDevice.CreateReadTransaction(data) };
            _i2C.Execute(_trRegRead, _timeout);
        }

        protected void RegWrite(byte reg, byte val)
        {
            _wdata[0] = reg;
            _wdata[1] = val;
            _trRegWrite = new I2CDevice.I2CTransaction[] { I2CDevice.CreateWriteTransaction(_wdata) };
            _i2C.Execute(_trRegWrite, _timeout);
        }

        protected void RegWriteMask(byte reg, byte val, byte mask)
        {
            var tmp = RegRead(reg);
            _wdata[0] = reg;
            _wdata[1] = (byte)(tmp & ~(int)mask | ((int)val & (int)mask));
            _trRegWrite = new I2CDevice.I2CTransaction[] { I2CDevice.CreateWriteTransaction(_wdata) };
            _i2C.Execute(_trRegWrite, _timeout);
        }
    }
}
