using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    /// <summary>
    /// GR-SAKURA�N���X
    /// </summary>
    public class Sakura
    {
        /// <summary>GR-SAKURA��̃{�^���̃s���ԍ�</summary>
        private const Cpu.Pin ButtonPin = (Cpu.Pin)0x57;

        /// <summary>GR-PEACH���1��LED�̃s���ԍ�</summary>
        private const Cpu.Pin Led1Pin = (Cpu.Pin)0x50;
        /// <summary>GR-PEACH���2��LED�̃s���ԍ�</summary>
        private const Cpu.Pin Led2Pin = (Cpu.Pin)0x51;
        /// <summary>GR-PEACH���3��LED�̃s���ԍ�</summary>
        private const Cpu.Pin Led3Pin = (Cpu.Pin)0x52;
        /// <summary>GR-PEACH���4��LED�̃s���ԍ�</summary>
        private const Cpu.Pin Led4Pin = (Cpu.Pin)0x56;

        /// <summary>1�� <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _led1;
        /// <summary>2�� <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _led2;
        /// <summary>3�� <see cref="GrFamily.MainBoard.Led">LED</see></summary>
        private readonly Led _led3;
        /// <summary>4�� <see cref="GrFamily.MainBoard.DebugLed">LED</see></summary>
        /// <remarks>���̃��C�u�����ł�GR-SAKURA��4��LED���f�o�b�O�pLED�Ƃ��Ă����p����</remarks>
        private readonly DebugLed _debugLed;

        /// <summary>
        /// GR-SAKURA�̃R���X�g���N�^�[
        /// </summary>
        public Sakura()
        {
            _led1 = new Led(Led1Pin);
            _led2 = new Led(Led2Pin);
            _led3 = new Led(Led3Pin);
            _debugLed = new DebugLed(Led4Pin);

            Button = new Button(ButtonPin);
        }

        /// <summary>GR-SAKURA��̃{�^��</summary>
        public Button Button { get; }

        /// <summary>
        /// �S�Ă�LED����������
        /// </summary>
        public void TurnAllLedsOff()
        {
            _led1.SetLed(false);
            _led2.SetLed(false);
            _led3.SetLed(false);
            _debugLed.SetLed(false);
        }

        /// <summary>
        /// �eLED���ʂɓ_���E��������
        /// </summary>
        /// <param name="led1On">1��LED��_���܂��͏�������</param>
        /// <param name="led2On">2��LED��_���܂��͏�������</param>
        /// <param name="led3On">3��LED��_���܂��͏�������</param>
        /// <param name="led4On">4��LED��_���܂��͏�������</param>
        public void SetLeds(bool led1On, bool led2On, bool led3On, bool led4On)
        {
            _led1.SetLed(led1On);
            _led2.SetLed(led2On);
            _led3.SetLed(led3On);
            _debugLed.SetLed(led4On);
        }

        /// <summary>
        /// �����w�肵��LED��_������
        /// </summary>
        /// <param name="count">�_������LED�̌�</param>
        /// <remarks>�w�肵�����ɉ����āA1��LED���珇�ɓ_������</remarks>
        public void SetLeds(int count)
        {
            SetLeds(count >= 1, count >= 2, count >= 3, count >= 4);
        }

        /// <summary>
        /// LED�_���̃A�j���[�V���������s����
        /// </summary>
        /// <param name="switchTime">�A�j���[�V�������s�Ԋu�i�P�� : �~���b�j</param>
        /// <param name="remain">�������ԍ���LED��_�������܂܂ɂ���ꍇ�� true�A�ǂꂩ1��LED������_��������ꍇ�� false</param>
        /// <param name="repeatCount">�J��Ԃ���</param>
        public void Animate(int switchTime, bool remain = true, int repeatCount = 1)
        {
            SetLedsInner(0, false);

            for (var animateCount = 0; animateCount < repeatCount; animateCount++)
            {
                for (var ledNo = 1; ledNo <= 4; ledNo++)
                {
                    SetLedsInner(ledNo, remain);
                    Thread.Sleep(switchTime);
                }

                for (var ledNo = 3; ledNo >= 0; ledNo--)
                {
                    SetLedsInner(ledNo, remain);
                    Thread.Sleep(switchTime);
                }
            }
        }

        /// <summary>
        /// LED�ԍ��Ə������ԍ���LED��_�������邩���w�肵�āA�K�v��LED��_��������
        /// </summary>
        /// <param name="count">�_���Ώۂ�LED�ԍ�</param>
        /// <param name="remain">�_���Ώۂ�菬�����ԍ���LED���ꏏ�ɓ_������ꍇ�� true�A�����łȂ��ꍇ�� false</param>
        private void SetLedsInner(int count, bool remain)
        {
            var led1 = remain ? count >= 1 : count == 1;
            var led2 = remain ? count >= 2 : count == 2;
            var led3 = remain ? count >= 3 : count == 3;
            var led4 = remain ? count >= 4 : count == 4;

            SetLeds(led1, led2, led3, led4);
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
    }
}
