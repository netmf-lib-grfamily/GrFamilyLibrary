using System;
using System.Net;
using System.Net.Sockets;

namespace GrFamily.Utility
{
    /// <summary>
    /// NTPサーバーから現在日時を取得するクラス
    /// </summary>
    public static class SystemTimeInitializer
    {
        //private static string DefaultNtpServer = "time.nist.gov";
        private static string DefaultNtpServer = "ntp.nict.jp";
        private static int DefaultTimezoneOffset = 540;


        /// <summary>
        /// デフォルトの NTP サーバー、時差を使って、NTPサーバーから日時を取得してローカル日時を設定する
        /// </summary>
        /// <remarks>デフォルト NTP サーバーは ntp.nict.jp<br />
        /// デフォルト時差は 540分 (+9時間)</remarks>
        public static void InitSystemTime()
        {
            InitSystemTime(DefaultNtpServer, DefaultTimezoneOffset);
        }

        /// <summary>
        /// NTP サーバー、時差を指定して、日時を取得してローカル日時を設定する
        /// </summary>
        /// <param name="ntpServer">NTP サーバー</param>
        /// <param name="timezoneOffset">時差 (単位：分)</param>
        /// <remarks>情報源<br />
        /// http://stackoverflow.com/questions/1193955/how-to-query-an-ntp-server-using-c<br />
        /// http://weblogs.asp.net/mschwarz/wrong-datetime-on-net-micro-framework-devices</remarks>
        public static void InitSystemTime(string ntpServer, int timezoneOffset)
        {
            var ep = new IPEndPoint(Dns.GetHostEntry(ntpServer).AddressList[0], 123);

            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.Connect(ep);

            var ntpData = new byte[48];
            ntpData[0] = 0x1b;
            for (var i = 1; i < 48; i++)
                ntpData[i] = 0;

            sock.Send(ntpData);
            sock.Receive(ntpData);
            sock.Close();

            const int offset = 40;
            ulong intPart = 0;
            for (var i = 0; i <= 3; i++)
                intPart = 256 * intPart + ntpData[offset + i];

            ulong fractPart = 0;
            for (var i = 4; i <= 7; i++)
                fractPart = 256 * fractPart + ntpData[offset + i];

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            var dateTime = new DateTime(1900, 1, 1).AddMilliseconds(milliseconds);
            var networkDateTime = dateTime + new TimeSpan(0, timezoneOffset, 0);

            Microsoft.SPOT.Hardware.Utility.SetLocalTime(networkDateTime);
        }
    }
}
