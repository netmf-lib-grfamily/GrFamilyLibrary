using Microsoft.SPOT.Hardware;

namespace GrFamily.Module
{
    /// <summary>
    /// I2Cデバイスの拡張クラス
    /// </summary>
    /// <remarks>本クラスは I2CDeviceクラスに高級なメソッドを追加する</remarks>
    public class I2CDeviceEx
    {
        /// <summary>I2Cデバイス</summary>
        private readonly I2CDevice _i2C;
        /// <summary>I2Cで通信失敗したときのタイムアウト値</summary>
        private readonly int _timeout;
        /// <summary>アドレス送信用バッファ</summary>
        private readonly byte[] _adata = new byte[1];
        /// <summary>読み込み用バッファ</summary>
        private readonly byte[] _rdata = new byte[1];
        /// <summary>書き込み用バッファ</summary>
        private readonly byte[] _wdata = new byte[2];

        /// <summary>読み込み用のI2Cトランザクション</summary>
        private I2CDevice.I2CTransaction[] _trRegRead;
        /// <summary>書き込み用のI2Cトランザクション</summary>
        private I2CDevice.I2CTransaction[] _trRegWrite;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="i2CAddress">I2Cアドレス</param>
        /// <param name="clockRateKhz">データ転送のクロックレート</param>
        /// <param name="timeout">I2C通信失敗時のタイムアウト値</param>
        public I2CDeviceEx(ushort i2CAddress, int clockRateKhz = 100, int timeout = 1000)
        {
            _i2C = new I2CDevice(new I2CDevice.Configuration(i2CAddress, clockRateKhz));
            _timeout = timeout;

        }

        /// <summary>
        /// 指定アドレスのデータを取得する
        /// </summary>
        /// <param name="reg">データ取得対象のアドレス</param>
        /// <returns>取得データ</returns>
        protected byte RegRead(byte reg)
        {
            _adata[0] = reg;
            _trRegRead = new I2CDevice.I2CTransaction[] {
                I2CDevice.CreateWriteTransaction(_adata),
                I2CDevice.CreateReadTransaction(_rdata) };
            _i2C.Execute(_trRegRead, _timeout);
            return _rdata[0];
        }

        /// <summary>
        /// 指定アドレスを起点として複数バイトデータを取得する
        /// </summary>
        /// <param name="reg">データ取得対象の先頭アドレス</param>
        /// <param name="data">取得データ</param>
        protected void RegReads(byte reg, ref byte[] data)
        {
            _adata[0] = reg;
            _trRegRead = new I2CDevice.I2CTransaction[] {
                I2CDevice.CreateWriteTransaction(_adata),
                I2CDevice.CreateReadTransaction(data) };
            _i2C.Execute(_trRegRead, _timeout);
        }

        /// <summary>
        /// 指定アドレスにデータを設定する
        /// </summary>
        /// <param name="reg">データ設定対象のアドレス</param>
        /// <param name="val">設定するデータ</param>
        protected void RegWrite(byte reg, byte val)
        {
            _wdata[0] = reg;
            _wdata[1] = val;
            _trRegWrite = new I2CDevice.I2CTransaction[] { I2CDevice.CreateWriteTransaction(_wdata) };
            _i2C.Execute(_trRegWrite, _timeout);
        }

        /// <summary>
        /// 指定アドレスの指定ビットに値を書き込む
        /// </summary>
        /// <param name="reg">指定ビットを含むバイト領域のアドレス</param>
        /// <param name="val">設定するデータ</param>
        /// <param name="mask">設定するデータを書き込むためのビットマスク</param>
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
