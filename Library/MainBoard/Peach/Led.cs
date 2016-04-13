using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    /// <summary>
    /// LEDクラス
    /// </summary>
    public class Led
    {
        /// <summary>LEDが接続されたピン</summary>
        protected readonly OutputPort LedPort;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="pin">LEDが接続されたピン</param>
        public Led(Cpu.Pin pin)
        {
            LedPort = new OutputPort(pin, false);
        }

        /// <summary>
        /// LEDを点灯／消灯する
        /// </summary>
        /// <param name="on">LEDを点灯する場合は true、消灯する場合は false</param>
        public void SetLed(bool on)
        {
            LedPort.Write(on);
        }
    }
}
