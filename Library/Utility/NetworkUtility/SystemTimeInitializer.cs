using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.SPOT;
using Microsoft.SPOT.Time;

namespace GrFamily.Utility
{
    /// <summary>
    /// NTP�T�[�o�[���猻�ݓ������擾����N���X
    /// </summary>
    public static class SystemTimeInitializer
    {
        //private static string DefaultNtpServer = "time.nist.gov";
        private static string DefaultNtpServer = "ntp.nict.jp";
        private static int DefaultTimezoneOffset = 540;


        /// <summary>
        /// �f�t�H���g�� NTP �T�[�o�[�A�������g���āANTP�T�[�o�[����������擾���ă��[�J��������ݒ肷��
        /// </summary>
        /// <remarks>�f�t�H���g NTP �T�[�o�[�� ntp.nict.jp<br />
        /// �f�t�H���g������ 540�� (+9����)</remarks>
        public static void InitSystemTime()
        {
            InitSystemTime(DefaultNtpServer, DefaultTimezoneOffset);
        }

        /// <summary>
        /// NTP �T�[�o�[�A�������w�肵�āA�������擾���ă��[�J��������ݒ肷��
        /// </summary>
        /// <param name="ntpServer">NTP �T�[�o�[</param>
        /// <param name="timezoneOffset">���� (�P�ʁF��)</param>
        /// <remarks>���<br />
        /// http://weblogs.asp.net/mschwarz/wrong-datetime-on-net-micro-framework-devices </remarks>
        public static void InitSystemTime(string ntpServer, int timezoneOffset)
        {
            var ep = new IPEndPoint(Dns.GetHostEntry(ntpServer).AddressList[0], 123);

            var sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sock.Connect(ep);

            var ntpData = new byte[48]; // RFC 2030
            ntpData[0] = 0x1b;
            for (var i = 1; i < 48; i++)
                ntpData[i] = 0;

            sock.Send(ntpData);
            sock.Receive(ntpData);

            var offsetTransmitTime = 40;
            ulong intpart = 0;
            ulong fractpart = 0;
            for (var i = 0; i <= 3; i++)
                intpart = 256 * intpart + ntpData[offsetTransmitTime + i];

            for (var i = 4; i <= 7; i++)
                fractpart = 256 * fractpart + ntpData[offsetTransmitTime + i];

            ulong milliseconds = (intpart * 1000 + (fractpart * 1000) / 0x100000000L);

            sock.Close();

            TimeSpan timeSpan = TimeSpan.FromTicks((long)milliseconds * TimeSpan.TicksPerMillisecond);
            DateTime dateTime = new DateTime(1900, 1, 1);
            dateTime += timeSpan;

            var networkDateTime = dateTime + new TimeSpan(0, timezoneOffset, 0);

            Microsoft.SPOT.Hardware.Utility.SetLocalTime(networkDateTime);
        }
    }
}
