using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    /// <summary>
    /// �}�C�R���{�[�h��̃f�o�b�OLED (User LED)
    /// </summary>
    /// <remarks>���̃��C�u�����Ńf�o�b�OLED�ƌĂԂ̂̓}�C�R���{�[�h��̈ȉ���LED�̂���<br />
    /// �EGR-PEACH : User LED<br />
    /// �EGR-SAKURA : LED4</remarks>
    public class DebugLed : Led
    {
        /// <summary>
        /// �R���X�g���N�^�[
        /// </summary>
        /// <param name="pin">�f�o�b�OLED���ڑ����ꂽ�s��</param>
        public DebugLed(Cpu.Pin pin) : base(pin)
        {
        }

        /// <summary>
        /// �f�o�b�OLED��1��_�ł�����
        /// </summary>
        public void PulseDebugLed()
        {
            new Thread(() =>
            {
                SetLed(true);
                Thread.Sleep(100);
                SetLed(false);
            }).Start();
        }

        /// <summary>
        /// �f�o�b�OLED���w��񐔁A�w��Ԋu�œ_�ł�����
        /// </summary>
        /// <param name="length">�_�ŊԊu�i�P�ʁF�~���b�j</param>
        /// <param name="times">�_�ŉ�</param>
        public void PulseDebugLed(int length, int times)
        {
            new Thread(() =>
            {
                for (var i = 0; i < times; i++)
                {
                    SetLed(true);
                    Thread.Sleep(length);
                    SetLed(false);
                    Thread.Sleep(length);
                }
            }).Start();
        }
    }
}
