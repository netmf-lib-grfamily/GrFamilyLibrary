using System.Threading;

namespace GrFamily.Module
{
    /// <summary>
    /// I2C接続の液晶キャラクターディスプレイ
    /// </summary>
    public class I2CLiquidCrystal : I2CDeviceEx
    {
        /// <summary>ディスプレイをオンにするかどうか</summary>
        private bool _displayOn = true;
        /// <summary>カーソルを表示するかどうか</summary>
        private bool _cursorOn;
        /// <summary>カーソル位置でブリンクするかどうか</summary>
        private bool _blinkOn;

        /// <summary>ストップビット草深のタイミングでキャラクター出力を行うかどうか</summary>
        private readonly bool _printWithStopBit;
        /// <summary>HD44780互換の拡張コマンドを使用するかどうか</summary>
        /// <remarks>拡張コマンドを使用するかどうかはモジュールによって決まる</remarks>
        private readonly bool _useExFunctionSet;

        /// <summary>コマンド実行後のウェイトタイム（単位：ミリ秒）</summary>
        private readonly int _commandWait = 1;

        /// <summary>
        /// コンストラクター
        /// </summary>
        /// <param name="i2CAddress">モジュールのI2Cアドレス</param>
        /// <param name="printWithStopBit">キャラクター出力にストップビットを使うかどうか</param>
        /// <param name="useExFunctionSet">拡張コマンドを使うかどうか</param>
        public I2CLiquidCrystal(ushort i2CAddress, bool printWithStopBit = false, bool useExFunctionSet = false) : base(i2CAddress)
        {
            _printWithStopBit = printWithStopBit;
            _useExFunctionSet = useExFunctionSet;
            InitModule();
        }

        /// <summary>
        /// 液晶キャラクターディスプレイのモジュールを初期化する
        /// </summary>
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

        /// <summary>
        /// 文字列を現在のカーソル位置から出力する
        /// </summary>
        /// <param name="msg">表示する文字列</param>
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

        /// <summary>
        /// ディスプレイの表示・非表示を切り替える
        /// </summary>
        /// <param name="displayOn"></param>
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
        /// <param name="blinkOn">ブリンクを行うかどうか</param>
        public void BlinkOn(bool blinkOn)
        {
            ControlDisplay(_displayOn, _cursorOn, blinkOn);
            _blinkOn = blinkOn;
        }

        /// <summary>
        /// カーソル位置を設定する
        /// </summary>
        /// <param name="row">カーソルの行位置</param>
        /// <param name="col">カーソルの列位置</param>
        public void SetCursor(int row, int col)
        {
            var addr = (byte)(((byte)row) << 6) + (byte)col;
            WriteCommand((byte)(0x80 | addr));
        }

        /// <summary>
        /// ディスプレイの表示に関する設定を変更する
        /// </summary>
        /// <param name="displayOn">ディスプレイ表示を行うかどうか</param>
        /// <param name="cursorOn">カーソル表示を行うかどうか</param>
        /// <param name="blinkOn">カーソルのブリンクを行うかどうか</param>
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

        /// <summary>
        /// カーソル位置に一文字出力する
        /// </summary>
        /// <param name="data">出力する文字</param>
        public void WriteCharactor(byte data)
        {
            var reg = _printWithStopBit ? (byte) 0x40 : (byte)0x80;
            RegWrite(reg, data);
        }

        /// <summary>
        /// 制御コマンドを送信する
        /// </summary>
        /// <param name="cmd">コマンド</param>
        /// <param name="wait">コマンド送信後のウェイトタイム</param>
        public void WriteCommand(byte cmd, int wait = 1)
        {
            var reg = (byte)0x00;
            RegWrite(reg, cmd);
            Thread.Sleep(wait);
        }
    }
}
