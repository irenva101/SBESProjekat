using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    public static class ClientExtension
    {
        public static string DisplayAndPickActiveClient(Dictionary<string, string> activeClients, string currentClientName)
        {
            foreach (var c in activeClients)
                if (c.Key != currentClientName)
                    Console.WriteLine(c.Value + " - " + c.Key);
            //else
            //    Console.WriteLine("port: " + c.Value);

            Console.WriteLine("Pick client[port] for chat...");
            var response = Console.ReadLine();
            if (activeClients.ContainsValue(response))
                return response;
            else
                return null;
        }

        public static void OpenHost(ServiceHost host, string port)
        {
            NetTcpBinding binding = new NetTcpBinding();

            string address = $"net.tcp://localhost:{port}/ChatService";
            host.AddServiceEndpoint(typeof(IChatService), binding, address);

            try
            {
                host.Open();
                Console.WriteLine("ChatService is started.\nPress <enter> to stop ...");
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
        }
    }
}
