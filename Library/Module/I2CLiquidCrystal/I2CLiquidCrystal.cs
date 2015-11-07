using System.Threading;
using Microsoft.SPOT.Hardware;
using GrFamily.Module;

namespace GrFamily.Module
{
    public class I2CLiquidCrystal : I2CDeviceEx
    {
        private bool _displayOn = true;       // �f�B�X�v���C���I���ɂ��邩
        private bool _cursorOn = false;         // �J�[�\����\�����邩�ǂ���
        private bool _blinkOn = false;          // �J�[�\���ʒu�Ńu�����N���邩

        private readonly bool _printWithStopBit;
        private readonly bool _useExFunctionSet;

        private readonly int _commandWait = 1;            // �R�}���h���s��̃E�F�C�g

        public I2CLiquidCrystal(ushort i2CAddress, bool printWithStopBit = false, bool useExFunctionSet = false) : base(i2CAddress)
        {
            _printWithStopBit = printWithStopBit;
            _useExFunctionSet = useExFunctionSet;
            InitModule();
        }

        private void InitModule()
        {
            Thread.Sleep(1000);

            if (_useExFunctionSet)
            {   // �g���@�\�Z�b�g���g�p���郂�W���[��
                WriteCommand(0x38);         // �W���@�\�Z�b�g���w��
                WriteCommand(0x39);         // �g���@�\�Z�b�g���w��
                WriteCommand(0x14);         // �����I�V���[�^�[���g�����w��
                WriteCommand(0x70);         // �R���g���X�g���w��i����4�r�b�g�j
                WriteCommand(0x56);         // �R���g���X�g���w�� (���4�r�b�g)
                WriteCommand(0x6c);         // �t�H�����[�R���g���[��
                WriteCommand(0x38);         // �W���@�\�Z�b�g�ɖ߂�
                WriteCommand(0x0c);         // �\���I��
            }
            else
            {   // �W���@�\�Z�b�g���g�p���郂�W���[��
                WriteCommand(0x30, 5);      // 8�r�b�g���[�h�ɃZ�b�g + 5ms�E�F�C�g
                WriteCommand(0x30);         // 8�r�b�g���[�h�ɃZ�b�g
                WriteCommand(0x30);         // 8�r�b�g���[�h�ɃZ�b�g
                WriteCommand(0x38);         // �s���ƃt�H���g�̐ݒ�
                WriteCommand(0x80);         // �\���I�t
            }

            WriteCommand(0x01);         // �\���N���A
            WriteCommand(0x06);         // �J�[�\���ƕ\���̃V�t�g�ݒ�
            WriteCommand(0x0c);         // �J�[�\���ƃu�����N�̕\�����I�t
            //WriteCommand(0x0f);         // �J�[�\���ƃu�����N�̕\�����I��

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

        public void SetContrast(byte contrast)
        {
            WriteCommand(0x39);
            WriteCommand(0x14);
            WriteCommand((byte)(0x70 | contrast));
            WriteCommand(0x6c);
            WriteCommand(0x38);
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
            var reg = _printWithStopBit ? (byte) 0x40 : (byte)0x80;
            RegWrite(reg, data);
        }

        public void WriteCommand(byte cmd, int wait = 1)
        {
            var reg = (byte)0x00;
            RegWrite(reg, cmd);
            Thread.Sleep(wait);
        }
    }
}
