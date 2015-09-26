using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using GrFamily.MainBoard;
using GrFamily.Utility;

namespace NetworkUtilityTest
{
    public class Program
    {
        public static void Main()
        {
            var peach = new Peach();

            var ipAddress = NetworkUtility.InitNetwork();
            if (ipAddress == "0.0.0.0")
            {
                Debug.Print("InitNetwork Failed");
                return;
            }

            using (var request = WebRequest.Create(new Uri("http://www.microsoft.com/ja-jp")) as HttpWebRequest)
            {
                request.Method = "GET";

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var resContentBytes = new byte[(int)response.ContentLength];
                        using (var ress = response.GetResponseStream())
                        {
                            ress.Read(resContentBytes, 0, (int)response.ContentLength);
                            var resContentChars = Encoding.UTF8.GetChars(resContentBytes);
                            var resContent = new string(resContentChars);
                            Debug.Print(resContent);
                        }

                        peach.PulseDebugLed(100, 5);
                    }
                }
            }

            Thread.Sleep(3000);
        }
    }
}
