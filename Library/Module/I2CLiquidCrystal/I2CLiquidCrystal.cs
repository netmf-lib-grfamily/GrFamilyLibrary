using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.Module
{
    public class I2CLiquidCrystal
    {
        //private static readonly ushort _acm1602n1Address = 0x50;     // ACM1602N1-FLW-FBW�̃A�h���X

        private const int DefaultClockRateKhz = 100;
        private const int Timeout = 1000;

        private readonly I2CDevice _i2C;
        private readonly byte[] _adata = new byte[1];
        private readonly byte[] _rdata = new byte[1];
        private readonly byte[] _wdata = new byte[2];
        //private I2CDevice.I2CTransaction[] _trRegRead;
        private I2CDevice.I2CTransaction[] _trRegWrite;

        private bool _displayOn = true;       // �f�B�X�v���C���I���ɂ��邩
        private bool _cursorOn = false;         // �J�[�\����\�����邩�ǂ���
        private bool _blinkOn = false;          // �J�[�\���ʒu�Ńu�����N���邩

        private readonly int _commandWait = 1;            // �R�}���h���s��̃E�F�C�g

        public I2CLiquidCrystal(ushort i2CAddress)
        {
            _i2C = new I2CDevice(new I2CDevice.Configuration(i2CAddress, DefaultClockRateKhz));
            InitModule();
        }

        private void InitModule()
        {
            Thread.Sleep(1000);

            WriteCommand(0x30, 5);      // 8�r�b�g���[�h�ɃZ�b�g + 5ms�E�F�C�g
            WriteCommand(0x30);         // 8�r�b�g���[�h�ɃZ�b�g
            WriteCommand(0x30);         // 8�r�b�g���[�h�ɃZ�b�g
            WriteCommand(0x38);         // �s���ƃt�H���g�̐ݒ�
            WriteCommand(0x80);         // �\���I�t
            WriteCommand(0x01);         // �\���N���A
            WriteCommand(0x06);         // �J�[�\���ƕ\���̃V�t�g�ݒ�
            WriteCommand(0x0f);         // �J�[�\���ƃu�����N�̕\�����I��

            Thread.Sleep(100);
        }

        public void Print(string msg)
        {
            for (var i = 0; i < msg.Length; i++)
            {
                WriteCharactor((byte)msg[i]);
            }
        }

        /// <summary>
        /// �\���N���A
        /// </summary>
        public void Clear()
        {
            WriteCommand(0x01, 5);      // Clear Display�̓E�F�C�g���K�v
        }

        /// <summary>
        /// �J�[�\���������ʒu�ɖ߂�
        /// </summary>
        /// <remarks>�\���͏����Ȃ�</remarks>
        public void Home()
        {
            WriteCommand(0x02, 5);      // Return Home�̓E�F�C�g���K�v
        }

        public void DisplayOn(bool displayOn)
        {
            ControlDisplay(displayOn, _cursorOn, _blinkOn);
            _displayOn = displayOn;
        }

        /// <summary>
        /// �J�[�\���\���I���E�I�t��؂�ւ���
        /// </summary>
        /// <param name="cursorOn"></param>
        public void CursorOn(bool cursorOn)
        {
            ControlDisplay(_displayOn, cursorOn, _blinkOn);
            _cursorOn = cursorOn;
        }

        /// <summary>
        /// �J�[�\���ʒu�̃u�����N�̃I���E�I�t��؂�ւ���
        /// </summary>
        /// <param name="blinkOn"></param>
        public void BlinkOn(bool blinkOn)
        {
            ControlDisplay(_displayOn, _cursorOn, blinkOn);
            _blinkOn = blinkOn;
        }

        public void SetCursor(int row, int col)
        {
            var addr = (byte)(((byte)row) << 6) + (byte)col;
            WriteCommand((byte)(0x80 | addr));
        }

        private void ControlDisplay(bool displayOn, bool cursorOn, bool blinkOn)
        {
            var cmd = (byte)0x08;
            if (displayOn)
                cmd |= 0x04;
            if (cursorOn)
                cmd |= 0x02;
            if (blinkOn)
                cmd |= 0x01;

            WriteCommand(cmd, _commandWait);
        }

        public void WriteCharactor(byte data)
        {
            var reg = (byte)0x80;
            WriteByte(reg, data);
        }

        public void WriteCommand(byte cmd, int wait = 1)
        {
            var reg = (byte)0x00;
            WriteByte(reg, cmd);
            Thread.Sleep(wait);
        }

        public void WriteByte(byte reg, byte val)
        {
            _wdata[0] = reg;
            _wdata[1] = val;
            _trRegWrite = new I2CDevice.I2CTransaction[] { I2CDevice.CreateWriteTransaction(_wdata) };
            _i2C.Execute(_trRegWrite, Timeout);
        }
    }
}
