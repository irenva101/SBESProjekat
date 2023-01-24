
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            /// Define the expected service certificate. It is required to establish cmmunication using certificates.
            string srvCertCN = "service"; //ovaj sertifikat treba da ocekuje

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/SecurityService"), //ovim podesavamo identitet servisa koji klijent ocekuje
                                      new X509CertificateEndpointIdentity(srvCert));


            //registracija na server i otvaranje host-a za client-a
            Random rnd = new Random();
            int temp = rnd.Next(1, 1000);
            string currentClientName = "marko" + temp.ToString();
            Dictionary<string, string> activeClients;
            string clientPort = null;
            //Console.WriteLine("currentClientName: " + currentClientName);

            using (ServerProxy proxy = new ServerProxy(binding, address))
            {
                activeClients = proxy.RegisterClient(currentClientName);

                ServiceHost host = new ServiceHost(typeof(ChatService));
                ClientExtension.OpenHost(host, activeClients[currentClientName]);

                while (clientPort == null && activeClients.Count() > 1)
                {
                    clientPort = ClientExtension.DisplayAndPickActiveClient(activeClients, currentClientName);
                }
            }

            //slanje poruka odabranom client-u
            if (activeClients.Count() > 1)
            {
                NetTcpBinding clientBinding = new NetTcpBinding();
                EndpointAddress clientAddress = new EndpointAddress(new Uri($"net.tcp://localhost:{clientPort}/ChatService"));

                string text = "";
                Console.WriteLine($"Send to client on port {clientPort}:");

                using (ClientProxy proxy = new ClientProxy(clientBinding, clientAddress))
                {
                    while (text != "stop")
                    {
                        text = Console.ReadLine();
                        proxy.SendMessage(currentClientName, text);
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
