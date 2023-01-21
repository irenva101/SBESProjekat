using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SecurityService : ISecurityService
    {
        //key: clientName - value: port
        public static Dictionary<string, string> activeClients = new Dictionary<string, string>();
        private static int nextFreePort = 9999;

        public Dictionary<string, string> RegisterClient(string clientName)
        {
            nextFreePort = nextFreePort - 1;

            activeClients.Add(clientName, nextFreePort.ToString());

            return activeClients;
        }
    }
}
