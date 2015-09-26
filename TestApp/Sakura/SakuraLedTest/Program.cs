using System.Threading;
using GrFamily.MainBoard;
using Microsoft.SPOT;

namespace SakuraLedTest
{
    public class Program
    {
        private static Sakura _sakura;

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
            LineLedTest,
            AnimateLedTest
        };

        // �e�X�g�̌�
        private static readonly int TestMax = Tests.Length;
        #endregion

        public static void Main()
        {
            _sakura = new Sakura();
            _sakura.Button.ButtonPressed += Button_ButtonPressed;

            while (true) { }
        }

        static void Button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            // �e�X�g�p�̃X���b�h�����삵�Ă���Δj��
            if (_th != null && _th.IsAlive)
                _th.Abort();

            // ���̃e�X�g�̑O�ɑS�Ă� LED ������
            _sakura.TurnAllLedsOff();

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
                _sakura.SetDebugLed(true);
                Thread.Sleep(1000);
                _sakura.SetDebugLed(false);
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
                _sakura.PulseDebugLed();
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
                _sakura.PulseDebugLed(250, 3);
                Thread.Sleep(3000);
            }
        }

        private static void LineLedTest()
        {
            Debug.Print("Running LineLedTest()");

            _sakura.SetLeds(0);

            while (true)
            {
                for (var i = 1; i <= 4; i++)
                {
                    _sakura.SetLeds(i);
                    Thread.Sleep(200);
                }

                for (var i = 3; i >= 0; i--)
                {
                    _sakura.SetLeds(i);
                    Thread.Sleep(200);
                }
            }
        }

        private static void AnimateLedTest()
        {
            Debug.Print("Running AnimateLedTest()");

            while (true)
            {
                _sakura.Animate(100, true, 3);
                Thread.Sleep(1000);
                _sakura.Animate(100, false, 3);
                Thread.Sleep(1000);
            }
        }
    }
}
