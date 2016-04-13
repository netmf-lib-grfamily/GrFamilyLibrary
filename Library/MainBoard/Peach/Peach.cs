using System;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    /// <summary>
    /// GR-PEACH�N���X
    /// </summary>
    public class Peach
    {
        /// <summary>GR-PEACH��̃{�^���̃s���ԍ�</summary>
        private const Cpu.Pin ButtonPin = (Cpu.Pin)0x60;

        /// <summary>GR-PEACH��̐ԐFLED�̃s���ԍ�</summary>
        private const Cpu.Pin RedPin = (Cpu.Pin)0x6d;
        /// <summary>GR-PEACH��̗ΐFLED�̃s���ԍ�</summary>
        private const Cpu.Pin GreenPin = (Cpu.Pin)0x6e;
        /// <summary>GR-PEACH��̐FLED�̃s���ԍ�</summary>
        private const Cpu.Pin BluePin = (Cpu.Pin)0x6f;
        /// <summary>GR-PEACH��̃��[�U�[LED�̃s���ԍ�</summary>
        private const Cpu.Pin UserPin = (Cpu.Pin)0x6c;

        /// <summary>�ԐF <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _redLed;
        /// <summary>�ΐF <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _greenLed;
        /// <summary>�F <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _blueLed;
        /// <summary>���[�U�[ <see cref="GrFamily.MainBoard.DebugLed">LED</see></summary>
        private readonly DebugLed _debugLed;

        /// <summary>
        /// GR-PEACH�̃R���X�g���N�^�[
        /// </summary>
        public Peach()
        {
            _redLed = new Led(RedPin);
            _greenLed = new Led(GreenPin);
            _blueLed = new Led(BluePin);
            _debugLed = new DebugLed(UserPin);

            Button = new Button(ButtonPin);
        }

        /// <summary>GR-PEACH��̃{�^��</summary>
        public Button Button { get; }

        /// <summary>
        /// �S�Ă�LED����������i�ԁE�΁E�E�f�o�b�OLED�j
        /// </summary>
        public void TurnAllLedsOff()
        {
            _redLed.SetLed(false);
            _greenLed.SetLed(false);
            _blueLed.SetLed(false);
            _debugLed.SetLed(false);
        }

        /// <summary>
        /// �e�F��LED���ʂɓ_���E��������
        /// </summary>
        /// <param name="redOn">�ԐFLED��_���܂��͏�������</param>
        /// <param name="greenOn">�ΐFLED��_���܂��͏�������</param>
        /// <param name="blueOn">�FLED��_���܂��͏�������</param>
        public void SetLeds(bool redOn, bool greenOn, bool blueOn)
        {
            _redLed.SetLed(redOn);
            _greenLed.SetLed(greenOn);
            _blueLed.SetLed(blueOn);
        }

        /// <summary>
        /// �F�����w�肵�ăJ���[LED��_������
        /// </summary>
        /// <param name="color">�_���������F��</param>
        public void SetColor(Color color)
        {
            _redLed.SetLed((color & Color.Red) != 0);
            _greenLed.SetLed((color & Color.Green) != 0);
            _blueLed.SetLed((color & Color.Blue) != 0);
        }

        /// <summary>
        /// �f�o�b�OLED��_���܂��͏�������
        /// </summary>
        /// <param name="lightOn">�_������ꍇ�� true�A��������ꍇ�� false</param>
        public void SetDebugLed(bool lightOn)
        {
            _debugLed.SetLed(lightOn);
        }

        /// <summary>
        /// �f�o�b�OLED��1��_�ł���
        /// </summary>
        public void PulseDebugLed()
        {
            _debugLed.PulseDebugLed();
        }

        /// <summary>
        /// �f�o�b�OLED���w��񐔁A�w��Ԋu�œ_�ł�����
        /// </summary>
        /// <param name="length">�_�ŊԊu�i�P�ʁF�~���b�j</param>
        /// <param name="times">�_�ŉ�</param>
        public void PulseDebugLed(int length, int times)
        {
            _debugLed.PulseDebugLed(length, times);
        }

        /// <summary>
        /// �F���w��
        /// </summary>
        [Flags]
        public enum Color
        {
            /// <summary>���i�J���[LED��S�ď�������j</summary>
            Black = 0,
            /// <summary>��</summary>
            Red = 1,
            /// <summary>��</summary>
            Green = 2,
            /// <summary>��</summary>
            Blue = 4,

            /// <summary>��</summary>
            Yellow = Red + Green,
            /// <summary>���i�}�[���^�j</summary>
            Magenta = Red + Blue,
            /// <summary>�΁i�V�A���j</summary>
            Cyan = Green + Blue,
            /// <summary>��</summary>
            White = Red + Green + Blue
        }
    }
}
