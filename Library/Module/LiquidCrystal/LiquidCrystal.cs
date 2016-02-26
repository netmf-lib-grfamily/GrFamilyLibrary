using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.Module
{
    public class LiquidCrystal
    {
        private readonly OutputPort _pinRs;
        private readonly OutputPort _pinEnable;
        private readonly OutputPort _pinDb7;
        private readonly OutputPort _pinDb6;
        private readonly OutputPort _pinDb5;
        private readonly OutputPort _pinDb4;

        private bool _displayOn = true;       // �f�B�X�v���C���I���ɂ��邩
        private bool _cursorOn = false;         // �J�[�\����\�����邩�ǂ���
        private bool _blinkOn = false;          // �J�[�\���ʒu�Ńu�����N���邩

        private int _commandWait = 1;            // �R�}���h���s��̃E�F�C�g


        public LiquidCrystal(Cpu.Pin rsPort, Cpu.Pin enablePort, Cpu.Pin db4Port, Cpu.Pin db5Port, Cpu.Pin db6Port, Cpu.Pin db7Port)
        {
            _pinRs = new OutputPort(rsPort, false);
            _pinEnable = new OutputPort(enablePort, false);
            _pinDb4 = new OutputPort(db4Port, false);
            _pinDb5 = new OutputPort(db5Port, false);
            _pinDb6 = new OutputPort(db6Port, false);
            _pinDb7 = new OutputPort(db7Port, false);
        }

        public void InitDevice(int wait = 0)
        {
            Thread.Sleep(1000);

            WriteCommand(false, false, true, true, wait + 5);       // 8�r�b�g���[�h�ɃZ�b�g + 5ms�E�F�C�g
            WriteCommand(false, false, true, true, wait);           // 8�r�b�g���[�h�ɃZ�b�g
            WriteCommand(false, false, true, true, wait);           // 8�r�b�g���[�h�ɃZ�b�g (2���)
            WriteCommand(false, false, true, false, wait);          // 4�r�b�g���[�h�ɃZ�b�g
            WriteCommand(false, false, true, false, wait);          // �s���ƃt�H���g�̐ݒ�1
            WriteCommand(true, false, false, false, wait);          // �s���ƃt�H���g�̐ݒ�2
            WriteCommand(false, false, false, false, wait);         // �\�����I�t1
            WriteCommand(true, false, false, false, wait);          // �\�����I�t2
            WriteCommand(false, false, false, false, wait);         // �\���f�[�^���N���A1
            WriteCommand(false, false, false, true, wait);          // �\���f�[�^���N���A2
            WriteCommand(false, false, false, false, wait);         // �J�[�\���ƕ\���̃V�t�g�ݒ�1
            WriteCommand(false, true, true, false, wait);           // �J�[�\���ƕ\���̃V�t�g�ݒ�2
            WriteCommand(false, false, false, false, wait);             // �\�����I��1
            WriteCommand(true, _displayOn, _cursorOn, _blinkOn, wait);  // �\�����I��2

            _commandWait = wait;

            Thread.Sleep(100);
        }

        public void Print(string msg)
        {
            for (var i = 0; i < msg.Length; i++)
            {
                Write((byte)msg[i]);
            }
        }

        /// <summary>
        /// �\���N���A
        /// </summary>
        public void Clear()
        {
            WriteCommand(false, false, false, false);
            WriteCommand(false, false, false, true, 5);     // Clear Display�̓E�F�C�g���K�v
        }

        /// <summary>
        /// �J�[�\���������ʒu�ɖ߂�
        /// </summary>
        /// <remarks>�\���͏����Ȃ�</remarks>
        public void Home()
        {
            WriteCommand(false, false, false, false);
            WriteCommand(false, false, true, false, 5);     // Return Home�̓E�F�C�g���K�v
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

        /// <summary>
        /// �J�[�\���ʒu���ړ�
        /// </summary>
        /// <param name="row">�s</param>
        /// <param name="col">��</param>
        /// <remarks>row, col�Ƃ��� 0 �n�܂�</remarks>
        public void SetCursor(int row, int col)
        {
            var addr = (byte)(((byte)row) << 2);
            WriteCommand(true, (addr & 0x04) != 0, (addr & 0x02) != 0, (addr & 0x01) != 0);

            addr = (byte)col;
            WriteCommand((addr & 0x08) != 0, (addr & 0x04) != 0, (addr & 0x02) != 0, (addr & 0x01) != 0);
        }

        /// <summary>
        /// ���݈ʒu�ɂP�����o��
        /// </summary>
        /// <param name="data">�o�͂��镶���R�[�h</param>
        public void Write(byte data)
        {
            _pinRs.Write(true);

            _pinEnable.Write(true);
            _pinDb7.Write((data & 0x80) != 0);
            _pinDb6.Write((data & 0x40) != 0);
            _pinDb5.Write((data & 0x20) != 0);
            _pinDb4.Write((data & 0x10) != 0);
            _pinEnable.Write(false);

            _pinEnable.Write(true);
            _pinDb7.Write((data & 0x08) != 0);
            _pinDb6.Write((data & 0x04) != 0);
            _pinDb5.Write((data & 0x02) != 0);
            _pinDb4.Write((data & 0x01) != 0);
            _pinEnable.Write(false);
        }

        private void ControlDisplay(bool displayOn, bool cursorOn, bool blinkOn)
        {
            WriteCommand(false, false, false, false, _commandWait);             // �\�����I��1
            WriteCommand(true, displayOn, cursorOn, blinkOn, _commandWait);  // �\�����I��2
        }

        private void WriteCommand(bool db7, bool db6, bool db5, bool db4, int wait = 1)
        {
            _pinRs.Write(false);

            _pinEnable.Write(true);
            _pinDb7.Write(db7);
            _pinDb6.Write(db6);
            _pinDb5.Write(db5);
            _pinDb4.Write(db4);

            _pinEnable.Write(false);
            Thread.Sleep(wait);
        }
    }
}
