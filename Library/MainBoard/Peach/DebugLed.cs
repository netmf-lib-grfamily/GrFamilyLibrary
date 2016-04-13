using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    /// <summary>
    /// マイコンボード上のデバッグLED (User LED)
    /// </summary>
    /// <remarks>このライブラリでデバッグLEDと呼ぶのはマイコンボード上の以下のLEDのこと<br />
    /// ・GR-PEACH : User LED<br />
    /// ・GR-SAKURA : LED4</remarks>
    public class DebugLed : Led
    {
        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="pin">デバッグLEDが接続されたピン</param>
        public DebugLed(Cpu.Pin pin) : base(pin)
        {
        }

        /// <summary>
        /// デバッグLEDを1回点滅させる
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
        /// デバッグLEDを指定回数、指定間隔で点滅させる
        /// </summary>
        /// <param name="length">点滅間隔（単位：ミリ秒）</param>
        /// <param name="times">点滅回数</param>
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
