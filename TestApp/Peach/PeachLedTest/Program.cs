using System.Threading;
using GrFamily.MainBoard;
using Microsoft.SPOT;

namespace PeachLedTest
{
    public class Program
    {
        private static Peach _peach;

        #region �e�X�g�؂�ւ�
        // �e�X�g�p�̃X���b�h
        private static Thread _th;
        // ���Ԗڂ̃e�X�g�����s���Ă��邩
        private static int _testIndex = -1;

        // ���{����e�X�g
        private static readonly ThreadStart[] Tests =
        {
            SetLedTest,
            PulseDebugLedTest,
            PulseDebugLedMultiTest,
            SevenColorsTest,
            SevenColorsTest2
        };

        // �e�X�g�̌�
        private static readonly int TestMax = Tests.Length;
        #endregion

        public static void Main()
        {
            _peach = new Peach();
            _peach.Button.ButtonPressed += Button_ButtonPressed;

            Debug.Print("Test Started");

            while (true) { }
        }

        static void Button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            // �e�X�g�p�̃X���b�h�����삵�Ă���Δj��
            if (_th != null && _th.IsAlive)
                _th.Abort();

            // ���̃e�X�g�̑O�ɑS�Ă� LED ������
            _peach.TurnAllLedsOff();

            // ���̃e�X�g���n�߂�
            _testIndex = (_testIndex + 1) % TestMax;
            _th = new Thread(Tests[_testIndex]);
            _th.Start();
        }

        /// <summary>
        /// L�`�J
        /// </summary>
        private static void SetLedTest()
        {
            Debug.Print("Running SetLedTest()");

            while (true)
            {
                _peach.SetDebugLed(true);
                Thread.Sleep(1000);
                _peach.SetDebugLed(false);
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// LED �̃p���X�_��
        /// </summary>
        private static void PulseDebugLedTest()
        {
            Debug.Print("Running PulseDebugLedTest()");

            while (true)
            {
                _peach.PulseDebugLed();
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// LED �̃p���X�_�� 2 (�_�����ԁA�A���_�ŉ񐔂̎w�肠��)
        /// </summary>
        private static void PulseDebugLedMultiTest()
        {
            Debug.Print("Running PulseDebugLedMultiTest()");

            while (true)
            {
                _peach.PulseDebugLed(250, 3);
                Thread.Sleep(3000);
            }
        }

        /// <summary>
        /// 7�F LED �@�\
        /// </summary>
        private static void SevenColorsTest()
        {
            Debug.Print("Running SevenColorsTest()");

            while (true)
            {
                _peach.SetLeds(false, false, false);
                Thread.Sleep(1000);
                _peach.SetLeds(true, false, false);
                Thread.Sleep(1000);
                _peach.SetLeds(true, true, false);
                Thread.Sleep(1000);
                _peach.SetLeds(false, true, false);
                Thread.Sleep(1000);
                _peach.SetLeds(false, true, true);
                Thread.Sleep(1000);
                _peach.SetLeds(false, false, true);
                Thread.Sleep(1000);
                _peach.SetLeds(true, false, true);
                Thread.Sleep(1000);
                _peach.SetLeds(true, true, true);
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 7�F LED �@�\ (�F���w��)
        /// </summary>
        private static void SevenColorsTest2()
        {
            Debug.Print("Running SevenColorsTest2()");

            while (true)
            {
                _peach.SetColor(Peach.Color.Red);
                Thread.Sleep(1000);
                _peach.SetColor(Peach.Color.Yellow);
                Thread.Sleep(1000);
                _peach.SetColor(Peach.Color.Green);
                Thread.Sleep(1000);
                _peach.SetColor(Peach.Color.Cyan);
                Thread.Sleep(1000);
                _peach.SetColor(Peach.Color.Blue);
                Thread.Sleep(1000);
                _peach.SetColor(Peach.Color.Magenta);
                Thread.Sleep(1000);
                _peach.SetColor(Peach.Color.White);
                Thread.Sleep(1000);
                _peach.SetColor(Peach.Color.Black);
                Thread.Sleep(1000);
            }
        }
    }
}
