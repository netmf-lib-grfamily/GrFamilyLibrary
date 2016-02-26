using System.Threading;
using Microsoft.SPOT.Hardware;

namespace GrFamily.Module
{
    public class LiquidCrystal
    {
        private readonly OutputPort _pinRs;
        private readonly OutputPort _pinEnable;
        private readonly OutputPort _pinDb7;
        private readonly OutputPort _pinDb6;
        private readonly OutputPort _pinDb5;
        private readonly OutputPort _pinDb4;

        private bool _displayOn = true;       // ディスプレイをオンにするか
        private bool _cursorOn = false;         // カーソルを表示するかどうか
        private bool _blinkOn = false;          // カーソル位置でブリンクするか

        private int _commandWait = 1;            // コマンド実行後のウェイト


        public LiquidCrystal(Cpu.Pin rsPort, Cpu.Pin enablePort, Cpu.Pin db4Port, Cpu.Pin db5Port, Cpu.Pin db6Port, Cpu.Pin db7Port)
        {
            _pinRs = new OutputPort(rsPort, false);
            _pinEnable = new OutputPort(enablePort, false);
            _pinDb4 = new OutputPort(db4Port, false);
            _pinDb5 = new OutputPort(db5Port, false);
            _pinDb6 = new OutputPort(db6Port, false);
            _pinDb7 = new OutputPort(db7Port, false);
        }

        public void InitDevice(int wait = 0)
        {
            Thread.Sleep(1000);

            WriteCommand(false, false, true, true, wait + 5);       // 8ビットモードにセット + 5msウェイト
            WriteCommand(false, false, true, true, wait);           // 8ビットモードにセット
            WriteCommand(false, false, true, true, wait);           // 8ビットモードにセット (2回目)
            WriteCommand(false, false, true, false, wait);          // 4ビットモードにセット
            WriteCommand(false, false, true, false, wait);          // 行数とフォントの設定1
            WriteCommand(true, false, false, false, wait);          // 行数とフォントの設定2
            WriteCommand(false, false, false, false, wait);         // 表示をオフ1
            WriteCommand(true, false, false, false, wait);          // 表示をオフ2
            WriteCommand(false, false, false, false, wait);         // 表示データをクリア1
            WriteCommand(false, false, false, true, wait);          // 表示データをクリア2
            WriteCommand(false, false, false, false, wait);         // カーソルと表示のシフト設定1
            WriteCommand(false, true, true, false, wait);           // カーソルと表示のシフト設定2
            WriteCommand(false, false, false, false, wait);             // 表示をオン1
            WriteCommand(true, _displayOn, _cursorOn, _blinkOn, wait);  // 表示をオン2

            _commandWait = wait;

            Thread.Sleep(100);
        }

        public void Print(string msg)
        {
            for (var i = 0; i < msg.Length; i++)
            {
                Write((byte)msg[i]);
            }
        }

        /// <summary>
        /// 表示クリア
        /// </summary>
        public void Clear()
        {
            WriteCommand(false, false, false, false);
            WriteCommand(false, false, false, true, 5);     // Clear Displayはウェイトが必要
        }

        /// <summary>
        /// カーソルを初期位置に戻す
        /// </summary>
        /// <remarks>表示は消さない</remarks>
        public void Home()
        {
            WriteCommand(false, false, false, false);
            WriteCommand(false, false, true, false, 5);     // Return Homeはウェイトが必要
        }

        public void DisplayOn(bool displayOn)
        {
            ControlDisplay(displayOn, _cursorOn, _blinkOn);
            _displayOn = displayOn;
        }

        /// <summary>
        /// カーソル表示オン・オフを切り替える
        /// </summary>
        /// <param name="cursorOn"></param>
        public void CursorOn(bool cursorOn)
        {
            ControlDisplay(_displayOn, cursorOn, _blinkOn);
            _cursorOn = cursorOn;
        }

        /// <summary>
        /// カーソル位置のブリンクのオン・オフを切り替える
        /// </summary>
        /// <param name="blinkOn"></param>
        public void BlinkOn(bool blinkOn)
        {
            ControlDisplay(_displayOn, _cursorOn, blinkOn);
            _blinkOn = blinkOn;
        }

        /// <summary>
        /// カーソル位置を移動
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <remarks>row, colともに 0 始まり</remarks>
        public void SetCursor(int row, int col)
        {
            var addr = (byte)(((byte)row) << 2);
            WriteCommand(true, (addr & 0x04) != 0, (addr & 0x02) != 0, (addr & 0x01) != 0);

            addr = (byte)col;
            WriteCommand((addr & 0x08) != 0, (addr & 0x04) != 0, (addr & 0x02) != 0, (addr & 0x01) != 0);
        }

        /// <summary>
        /// 現在位置に１文字出力
        /// </summary>
        /// <param name="data">出力する文字コード</param>
        public void Write(byte data)
        {
            _pinRs.Write(true);

            _pinEnable.Write(true);
            _pinDb7.Write((data & 0x80) != 0);
            _pinDb6.Write((data & 0x40) != 0);
            _pinDb5.Write((data & 0x20) != 0);
            _pinDb4.Write((data & 0x10) != 0);
            _pinEnable.Write(false);

            _pinEnable.Write(true);
            _pinDb7.Write((data & 0x08) != 0);
            _pinDb6.Write((data & 0x04) != 0);
            _pinDb5.Write((data & 0x02) != 0);
            _pinDb4.Write((data & 0x01) != 0);
            _pinEnable.Write(false);
        }

        private void ControlDisplay(bool displayOn, bool cursorOn, bool blinkOn)
        {
            WriteCommand(false, false, false, false, _commandWait);             // 表示をオン1
            WriteCommand(true, displayOn, cursorOn, blinkOn, _commandWait);  // 表示をオン2
        }

        private void WriteCommand(bool db7, bool db6, bool db5, bool db4, int wait = 1)
        {
            _pinRs.Write(false);

            _pinEnable.Write(true);
            _pinDb7.Write(db7);
            _pinDb6.Write(db6);
            _pinDb5.Write(db5);
            _pinDb4.Write(db4);

            _pinEnable.Write(false);
            Thread.Sleep(wait);
        }
    }
}
