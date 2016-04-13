using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.Module
{
    /// <summary>
    /// GPIO�ڑ��̉t���L�����N�^�[�f�B�X�v���C
    /// </summary>
    /// <remarks>�{�N���X��4�r�b�g���[�h��p</remarks>
    public class LiquidCrystal
    {
        /// <summary>���W�X�^�[�I��</summary>
        private readonly OutputPort _pinRs;
        /// <summary>���[�h�E���C�g�C�l�[�u���M��</summary>
        private readonly OutputPort _pinEnable;
        /// <summary>�f�[�^�r�b�g7</summary>
        private readonly OutputPort _pinDb7;
        /// <summary>�f�[�^�r�b�g6</summary>
        private readonly OutputPort _pinDb6;
        /// <summary>�f�[�^�r�b�g5</summary>
        private readonly OutputPort _pinDb5;
        /// <summary>�f�[�^�r�b�g4</summary>
        private readonly OutputPort _pinDb4;

        /// <summary>�f�B�X�v���C���I���ɂ��邩�ǂ���</summary>
        private bool _displayOn = true;
        /// <summary>�J�[�\����\�����邩�ǂ���</summary>
        private bool _cursorOn;
        /// <summary>�J�[�\���ʒu�Ńu�����N���邩�ǂ���</summary>
        private bool _blinkOn;

        /// <summary>�R�}���h���s��̃E�F�C�g�^�C���i�P�ʁF�~���b�j</summary>
        private int _commandWait = 1;

        /// <summary>
        /// �R���X�g���N�^�[
        /// </summary>
        /// <param name="rsPort">���W�X�^�[�I���̃s���ԍ�</param>
        /// <param name="enablePort">���[�h�E���C�g�C�l�[�u���M���̃s���ԍ�</param>
        /// <param name="db4Port">�f�[�^�r�b�g4�̃s���ԍ�</param>
        /// <param name="db5Port">�f�[�^�r�b�g5�̃s���ԍ�</param>
        /// <param name="db6Port">�f�[�^�r�b�g6�̃s���ԍ�</param>
        /// <param name="db7Port">�f�[�^�r�b�g7�̃s���ԍ�</param>
        public LiquidCrystal(Cpu.Pin rsPort, Cpu.Pin enablePort, Cpu.Pin db4Port, Cpu.Pin db5Port, Cpu.Pin db6Port, Cpu.Pin db7Port)
        {
            _pinRs = new OutputPort(rsPort, false);
            _pinEnable = new OutputPort(enablePort, false);
            _pinDb4 = new OutputPort(db4Port, false);
            _pinDb5 = new OutputPort(db5Port, false);
            _pinDb6 = new OutputPort(db6Port, false);
            _pinDb7 = new OutputPort(db7Port, false);
        }

        /// <summary>
        /// �t���L�����N�^�[�f�B�X�v���C�̃��W���[��������������
        /// </summary>
        /// <param name="wait">�������R�}���h���M��̃E�F�C�g�^�C��</param>
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

        /// <summary>
        /// ����������݂̃J�[�\���ʒu����o�͂���
        /// </summary>
        /// <param name="msg">�\�����镶����</param>
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

        /// <summary>
        /// �f�B�X�v���C�̕\���E��\����؂�ւ���
        /// </summary>
        /// <param name="displayOn"></param>
        public void DisplayOn(bool displayOn)
        {
            ControlDisplay(displayOn, _cursorOn, _blinkOn);
            _displayOn = displayOn;
        }

        /// <summary>
        /// �J�[�\���\���I���E�I�t��؂�ւ���
        /// </summary>
        /// <param name="cursorOn">�J�[�\���\������ꍇ�� true�A�����łȂ��ꍇ�� false</param>
        public void CursorOn(bool cursorOn)
        {
            ControlDisplay(_displayOn, cursorOn, _blinkOn);
            _cursorOn = cursorOn;
        }

        /// <summary>
        /// �J�[�\���ʒu�̃u�����N�̃I���E�I�t��؂�ւ���
        /// </summary>
        /// <param name="blinkOn">�J�[�\���ʒu�Ńu�����N����ꍇ�� true�A�����łȂ��ꍇ�� false</param>
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

        /// <summary>
        /// �f�B�X�v���C�̕\���Ɋւ���ݒ��ύX����
        /// </summary>
        /// <param name="displayOn">�f�B�X�v���C�\�����s�����ǂ���</param>
        /// <param name="cursorOn">�J�[�\���\�����s�����ǂ���</param>
        /// <param name="blinkOn">�J�[�\���̃u�����N���s�����ǂ���</param>
        private void ControlDisplay(bool displayOn, bool cursorOn, bool blinkOn)
        {
            WriteCommand(false, false, false, false, _commandWait);             // �\�����I��1
            WriteCommand(true, displayOn, cursorOn, blinkOn, _commandWait);  // �\�����I��2
        }

        /// <summary>
        /// ����R�}���h�𑗐M����
        /// </summary>
        /// <param name="db7">�f�[�^�r�b�g7</param>
        /// <param name="db6">�f�[�^�r�b�g6</param>
        /// <param name="db5">�f�[�^�r�b�g5</param>
        /// <param name="db4">�f�[�^�r�b�g4</param>
        /// <param name="wait">�R�}���h���M��̃E�F�C�g�^�C��</param>
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
