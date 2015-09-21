using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;

namespace Amway.OA.ETOffine.Utilities
{
    public class Utility
    {
        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            IPHostEntry IpEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            for (int i = 0; i != IpEntry.AddressList.Length; i++)
            {
                if (IpEntry.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return IpEntry.AddressList[i].ToString();
                }
            }

            return string.Empty;
        }

        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        public static bool IsConnectionNet()
        {
            int i = 0;
            if (InternetGetConnectedState(out i, 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
