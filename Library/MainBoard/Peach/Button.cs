using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    /// <summary>
    /// ボタンを押した／離したを返すイベントハンドラー
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="state">ボタンの状態</param>
    public delegate void ButtonEventHandler(Button sender, Button.ButtonState state);

    /// <summary>
    /// GR-PEACHマイコンボード上のボタン（スイッチ）
    /// </summary>
    public class Button
    {
        /// <summary>
        /// ボタンの状態
        /// </summary>
        public enum ButtonState
        {
            /// <summary>押されている</summary>
            Pressed,
            /// <summary>離されている</summary>
            Released
        }

        /// <summary>GR-PEACH上のボタンのデジタル入力ポート</summary>
        protected readonly InterruptPort ButtonPort;

        /// <summary>前回の測定時にボタンが押されていたかどうか</summary>
        protected bool PrevPressed;

        /// <summary>ボタンが押された時のイベントハンドラー</summary>
        public event ButtonEventHandler ButtonPressed;
        /// <summary>ボタンが離された時のイベントハンドラー</summary>
        public event ButtonEventHandler ButtonReleased;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="pin">ボタンが接続されているデジタル入力ピン</param>
        public Button(Cpu.Pin pin) 
        {
            ButtonPort = new InterruptPort(pin, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
            ButtonPort.OnInterrupt += ButtonPort_OnInterrupt;
        }

        /// <summary>
        /// ボタンの押下状態が変わった時に呼び出されるイベントハンドラー
        /// </summary>
        /// <param name="data1">一つ目のデータ</param>
        /// <param name="data2">二つ目のデータ</param>
        /// <param name="time">ボタンの状態が変わった時にタイムスタンプ</param>
        /// <remarks>ボタンが押されると data2 が 0 になる、そうでない時は data2 は 0以外の値になる</remarks>
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
        /// ボタンが押されているかどうかを返す   
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
