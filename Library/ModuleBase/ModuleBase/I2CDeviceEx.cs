using Microsoft.SPOT.Hardware;

namespace GrFamily.Module
{
    /// <summary>
    /// I2C�f�o�C�X�̊g���N���X
    /// </summary>
    /// <remarks>�{�N���X�� I2CDevice�N���X�ɍ����ȃ��\�b�h��ǉ�����</remarks>
    public class I2CDeviceEx
    {
        /// <summary>I2C�f�o�C�X</summary>
        private readonly I2CDevice _i2C;
        /// <summary>I2C�ŒʐM���s�����Ƃ��̃^�C���A�E�g�l</summary>
        private readonly int _timeout;
        /// <summary>�A�h���X���M�p�o�b�t�@</summary>
        private readonly byte[] _adata = new byte[1];
        /// <summary>�ǂݍ��ݗp�o�b�t�@</summary>
        private readonly byte[] _rdata = new byte[1];
        /// <summary>�������ݗp�o�b�t�@</summary>
        private readonly byte[] _wdata = new byte[2];

        /// <summary>�ǂݍ��ݗp��I2C�g�����U�N�V����</summary>
        private I2CDevice.I2CTransaction[] _trRegRead;
        /// <summary>�������ݗp��I2C�g�����U�N�V����</summary>
        private I2CDevice.I2CTransaction[] _trRegWrite;

        /// <summary>
        /// �R���X�g���N�^�[
        /// </summary>
        /// <param name="i2CAddress">I2C�A�h���X</param>
        /// <param name="clockRateKhz">�f�[�^�]���̃N���b�N���[�g</param>
        /// <param name="timeout">I2C�ʐM���s���̃^�C���A�E�g�l</param>
        public I2CDeviceEx(ushort i2CAddress, int clockRateKhz = 100, int timeout = 1000)
        {
            _i2C = new I2CDevice(new I2CDevice.Configuration(i2CAddress, clockRateKhz));
            _timeout = timeout;

        }

        /// <summary>
        /// �w��A�h���X�̃f�[�^���擾����
        /// </summary>
        /// <param name="reg">�f�[�^�擾�Ώۂ̃A�h���X</param>
        /// <returns>�擾�f�[�^</returns>
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
        /// �w��A�h���X���N�_�Ƃ��ĕ����o�C�g�f�[�^���擾����
        /// </summary>
        /// <param name="reg">�f�[�^�擾�Ώۂ̐擪�A�h���X</param>
        /// <param name="data">�擾�f�[�^</param>
        protected void RegReads(byte reg, ref byte[] data)
        {
            _adata[0] = reg;
            _trRegRead = new I2CDevice.I2CTransaction[] {
                I2CDevice.CreateWriteTransaction(_adata),
                I2CDevice.CreateReadTransaction(data) };
            _i2C.Execute(_trRegRead, _timeout);
        }

        /// <summary>
        /// �w��A�h���X�Ƀf�[�^��ݒ肷��
        /// </summary>
        /// <param name="reg">�f�[�^�ݒ�Ώۂ̃A�h���X</param>
        /// <param name="val">�ݒ肷��f�[�^</param>
        protected void RegWrite(byte reg, byte val)
        {
            _wdata[0] = reg;
            _wdata[1] = val;
            _trRegWrite = new I2CDevice.I2CTransaction[] { I2CDevice.CreateWriteTransaction(_wdata) };
            _i2C.Execute(_trRegWrite, _timeout);
        }

        /// <summary>
        /// �w��A�h���X�̎w��r�b�g�ɒl����������
        /// </summary>
        /// <param name="reg">�w��r�b�g���܂ރo�C�g�̈�̃A�h���X</param>
        /// <param name="val">�ݒ肷��f�[�^</param>
        /// <param name="mask">�ݒ肷��f�[�^���������ނ��߂̃r�b�g�}�X�N</param>
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
