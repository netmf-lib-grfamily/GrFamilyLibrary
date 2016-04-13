using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    /// <summary>
    /// LED�N���X
    /// </summary>
    public class Led
    {
        /// <summary>LED���ڑ����ꂽ�s��</summary>
        protected readonly OutputPort LedPort;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        /// <param name="pin">LED���ڑ����ꂽ�s��</param>
        public Led(Cpu.Pin pin)
        {
            LedPort = new OutputPort(pin, false);
        }

        /// <summary>
        /// LED��_���^��������
        /// </summary>
        /// <param name="on">LED��_������ꍇ�� true�A��������ꍇ�� false</param>
        public void SetLed(bool on)
        {
            LedPort.Write(on);
        }
    }
}
