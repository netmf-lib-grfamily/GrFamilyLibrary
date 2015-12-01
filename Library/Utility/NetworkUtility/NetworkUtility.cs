using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;

namespace GrFamily.Utility
{
    /// <summary>
    /// NIC�̏������N���X
    /// </summary>
    public static class NetworkUtility
    {
        /// <summary>
        /// NIC������������
        /// </summary>
        /// <param name="initialWait">�������J�n�O�̑҂����ԁi�P�ʁF�~���b�A�f�t�H���g�l=1000ms�j</param>
        /// <returns>IP�A�h���X</returns>
        public static string InitNetwork(int initialWait = 1000)
        {
            Thread.Sleep(initialWait);

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                    continue;
                if (!ni.IsDhcpEnabled)
                {
                    ni.EnableDhcp();
                    Thread.Sleep(1000);
                }

                var count = 0;
                var ipAddr = ni.IPAddress;
                while (ipAddr == "0.0.0.0" && count++ < 5)
                {
                    ni.RenewDhcpLease();
                    Thread.Sleep(1000);
                    ipAddr = ni.IPAddress;
                    Debug.Print("Retring[" + count + "] - Check IP Address : " + ipAddr);
                }
                if (ipAddr == "0.0.0.0")
                    return ipAddr;

                if (!ni.IsDynamicDnsEnabled)
                {
                    ni.EnableDynamicDns();
                    Thread.Sleep(1000);
                }

                return ipAddr;
            }

            return "0.0.0.0";
        }
    }
}
