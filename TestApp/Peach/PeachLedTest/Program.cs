using System.Threading;
using GrFamily.MainBoard;
using Microsoft.SPOT;

namespace PeachLedTest
{
    public class Program
    {
        private static Peach _peach;

        #region テスト切り替え
        // テスト用のスレッド
        private static Thread _th;
        // 何番目のテストを実行しているか
        private static int _testIndex = -1;

        // 実施するテスト
        private static readonly ThreadStart[] Tests =
        {
            SetLedTest,
            PulseDebugLedTest,
            PulseDebugLedMultiTest,
            SevenColorsTest,
            SevenColorsTest2
        };

        // テストの個数
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
            // テスト用のスレッドが動作していれば破棄
            if (_th != null && _th.IsAlive)
                _th.Abort();

            // 次のテストの前に全ての LED を消す
            _peach.TurnAllLedsOff();

            // 次のテストを始める
            _testIndex = (_testIndex + 1) % TestMax;
            _th = new Thread(Tests[_testIndex]);
            _th.Start();
        }

        /// <summary>
        /// Lチカ
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
        /// LED のパルス点滅
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
        /// LED のパルス点滅 2 (点灯時間、連続点滅回数の指定あり)
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
        /// 7色 LED 機能
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
        /// 7色 LED 機能 (色名指定)
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
