using System.Net.Sockets;
using System.Net;

namespace mixer_api.Utilities
{
    public static class NetworkUtilities
    {
        public static IEnumerable<string> GetLocalIpAddresses()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    yield return ip.ToString();
                }
            }
        }
    }
}
