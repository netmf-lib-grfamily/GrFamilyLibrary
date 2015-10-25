# GrFamilyLibrary
.NET Micro Framework Class Library for GR Family Boards

このソリューションは、GRファミリーの .NET Micro Framework用ライブラリです。
（GR-PEACH, GR-SAKURA, センサーボード、接続可能な各種センサーデバイス）

・Peach ・・・ GR-PEACH クラス
・Sakura ・・・ GR-SAKURA クラス
・SensorBoard ・・・ PinKit のセンサーボード（温度、3軸加速度、ブロック端子台）
・NetworkUtility ・・・ 有線LAN の初期化処理
・LiquidCrystal ・・・ キャラクターディスプレイクラス（HD44780 互換）
・AnalogSensor ・・・ アナログ入力の汎用センサークラス


対象の .NET Micro Framework のバージョンは、4.3 QFE2 です。

ライブラリ（Libraryフォルダー）、およびライブラリを利用するアプリケーション（TestAppフォルダー）とも、Visual Studio 2015 でビルドできます。

ライブラリのバイナリファイルのみ必要な場合は、DLLsフォルダー内のZIPファイルを解凍することで利用可能です。
ライブラリの利用例は、TestAppフォルダー内の各プロジェクトを参照してください。
