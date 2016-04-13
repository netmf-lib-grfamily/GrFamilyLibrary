using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    /// <summary>
    /// �{�^�����������^��������Ԃ��C�x���g�n���h���[
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="state">�{�^���̏��</param>
    public delegate void ButtonEventHandler(Button sender, Button.ButtonState state);

    /// <summary>
    /// GR-PEACH�}�C�R���{�[�h��̃{�^���i�X�C�b�`�j
    /// </summary>
    public class Button
    {
        /// <summary>
        /// �{�^���̏��
        /// </summary>
        public enum ButtonState
        {
            /// <summary>������Ă���</summary>
            Pressed,
            /// <summary>������Ă���</summary>
            Released
        }

        /// <summary>GR-PEACH��̃{�^���̃f�W�^�����̓|�[�g</summary>
        protected readonly InterruptPort ButtonPort;

        /// <summary>�O��̑��莞�Ƀ{�^����������Ă������ǂ���</summary>
        protected bool PrevPressed;

        /// <summary>�{�^���������ꂽ���̃C�x���g�n���h���[</summary>
        public event ButtonEventHandler ButtonPressed;
        /// <summary>�{�^���������ꂽ���̃C�x���g�n���h���[</summary>
        public event ButtonEventHandler ButtonReleased;

        /// <summary>
        /// �R���X�g���N�^�[
        /// </summary>
        /// <param name="pin">�{�^�����ڑ�����Ă���f�W�^�����̓s��</param>
        public Button(Cpu.Pin pin) 
        {
            ButtonPort = new InterruptPort(pin, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
            ButtonPort.OnInterrupt += ButtonPort_OnInterrupt;
        }

        /// <summary>
        /// �{�^���̉�����Ԃ��ς�������ɌĂяo�����C�x���g�n���h���[
        /// </summary>
        /// <param name="data1">��ڂ̃f�[�^</param>
        /// <param name="data2">��ڂ̃f�[�^</param>
        /// <param name="time">�{�^���̏�Ԃ��ς�������Ƀ^�C���X�^���v</param>
        /// <remarks>�{�^������������ data2 �� 0 �ɂȂ�A�����łȂ����� data2 �� 0�ȊO�̒l�ɂȂ�</remarks>
        void ButtonPort_OnInterrupt(uint data1, uint data2, System.DateTime time)
        {
            var isPressed = data2 == 0;

            if (isPressed && !PrevPressed && ButtonPressed != null)
                ButtonPressed(this, ButtonState.Pressed);
            else if (!isPressed && PrevPressed && ButtonReleased != null)
                ButtonReleased(this, ButtonState.Released);

            PrevPressed = isPressed;
        }

        /// <summary>
        /// �{�^����������Ă��邩�ǂ�����Ԃ�   
        /// </summary>
        public bool IsPressed
        {
            get
            {
                lock (this)
                {
                    return !ButtonPort.Read();
                }
            }
        }
    }
}
