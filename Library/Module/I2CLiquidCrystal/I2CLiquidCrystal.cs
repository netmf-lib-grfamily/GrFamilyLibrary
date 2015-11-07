using System.Threading;
using Microsoft.SPOT.Hardware;
using GrFamily.Module;

namespace GrFamily.Module
{
    public class I2CLiquidCrystal : I2CDeviceEx
    {
        private bool _displayOn = true;       // ディスプレイをオンにするか
        private bool _cursorOn = false;         // カーソルを表示するかどうか
        private bool _blinkOn = false;          // カーソル位置でブリンクするか

        private readonly bool _printWithStopBit;
        private readonly bool _useExFunctionSet;

        private readonly int _commandWait = 1;            // コマンド実行後のウェイト

        public I2CLiquidCrystal(ushort i2CAddress, bool printWithStopBit = false, bool useExFunctionSet = false) : base(i2CAddress)
        {
            _printWithStopBit = printWithStopBit;
            _useExFunctionSet = useExFunctionSet;
            InitModule();
        }

        private void InitModule()
        {
            Thread.Sleep(1000);

            if (_useExFunctionSet)
            {   // 拡張機能セットを使用するモジュール
                WriteCommand(0x38);         // 標準機能セットを指定
                WriteCommand(0x39);         // 拡張機能セットを指定
                WriteCommand(0x14);         // 内部オシレーター周波数を指定
                WriteCommand(0x70);         // コントラストを指定（下位4ビット）
                WriteCommand(0x56);         // コントラストを指定 (上位4ビット)
                WriteCommand(0x6c);         // フォロワーコントロール
                WriteCommand(0x38);         // 標準機能セットに戻す
                WriteCommand(0x0c);         // 表示オン
            }
            else
            {   // 標準機能セットを使用するモジュール
                WriteCommand(0x30, 5);      // 8ビットモードにセット + 5msウェイト
                WriteCommand(0x30);         // 8ビットモードにセット
                WriteCommand(0x30);         // 8ビットモードにセット
                WriteCommand(0x38);         // 行数とフォントの設定
                WriteCommand(0x80);         // 表示オフ
            }

            WriteCommand(0x01);         // 表示クリア
            WriteCommand(0x06);         // カーソルと表示のシフト設定
            WriteCommand(0x0c);         // カーソルとブリンクの表示をオフ
            //WriteCommand(0x0f);         // カーソルとブリンクの表示をオン

            Thread.Sleep(100);
        }

        public void Print(string msg)
        {
            for (var i = 0; i < msg.Length; i++)
            {
                WriteCharactor((byte)msg[i]);
            }
        }

        /// <summary>
        /// 表示クリア
        /// </summary>
        public void Clear()
        {
            WriteCommand(0x01, 5);      // Clear Displayはウェイトが必要
        }

        /// <summary>
        /// カーソルを初期位置に戻す
        /// </summary>
        /// <remarks>表示は消さない</remarks>
        public void Home()
        {
            WriteCommand(0x02, 5);      // Return Homeはウェイトが必要
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

        public void SetCursor(int row, int col)
        {
            var addr = (byte)(((byte)row) << 6) + (byte)col;
            WriteCommand((byte)(0x80 | addr));
        }

        public void SetContrast(byte contrast)
        {
            WriteCommand(0x39);
            WriteCommand(0x14);
            WriteCommand((byte)(0x70 | contrast));
            WriteCommand(0x6c);
            WriteCommand(0x38);
        }

        private void ControlDisplay(bool displayOn, bool cursorOn, bool blinkOn)
        {
            var cmd = (byte)0x08;
            if (displayOn)
                cmd |= 0x04;
            if (cursorOn)
                cmd |= 0x02;
            if (blinkOn)
                cmd |= 0x01;

            WriteCommand(cmd, _commandWait);
        }

        public void WriteCharactor(byte data)
        {
            var reg = _printWithStopBit ? (byte) 0x40 : (byte)0x80;
            RegWrite(reg, data);
        }

        public void WriteCommand(byte cmd, int wait = 1)
        {
            var reg = (byte)0x00;
            RegWrite(reg, cmd);
            Thread.Sleep(wait);
        }
    }
}
