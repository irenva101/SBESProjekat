
using Manager;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        public static string srvCertCN = "service";
        public static X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
        static void Main(string[] args)
        { 
            var username = Formatter.ParseName(WindowsIdentity.GetCurrent().Name); //bice irenv

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            //X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Receiver"), //ovim podesavamo identitet servisa koji klijent ocekuje
                                      EndpointIdentity.CreateUpnIdentity(username));

            if(CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username) == null)
            {
                using (IssuerClient proxy = new IssuerClient(binding, address))
                {
                    proxy.IssueCertificate();
                }
                Console.WriteLine("Waiting to install certificate.");
                while (CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username) == null)
                {
                    Thread.Sleep(200);
                }

            }
            int port = 0;

            using (IssuerClient proxy = new IssuerClient(binding, address))
            {
                Random rnd = new Random(System.DateTime.Now.Millisecond);
                port = rnd.Next(12000, 20000);
                proxy.RegisterClient(port);
            }
            CommunicationService.CommunicationService communicationService = new CommunicationService.CommunicationService($"net.tcp://localhost:{port}/{username}");

            try
            {
                communicationService.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            using (IssuerClient proxy = new IssuerClient(binding, address))
            {
                Console.WriteLine("Press enter to get active user list.");
                Console.ReadLine();
                Console.WriteLine("Active users list:");
                var list = proxy.GetAllActiveUsers();
                var i = 0;
                if (list != null)
                {
                    //if (list.Count() == 1)
                    //{
                    //    Console.WriteLine("You are the only active user.");
                    //    while (list.Count() == 1)
                    //    {
                    //        Console.WriteLine("Waiting for other clients to be active.");
                    //        Thread.Sleep(200);
                    //    }
                    //}

                    var users = new List<String>();
                    foreach (var user in list.Keys)
                    {
                        Console.WriteLine($"{++i}. {user}");
                        users.Add(user);
                    }


                }

                Console.WriteLine("Select user:");
                var input = "";
                var selected = 0;
                do
                {
                    input = Console.ReadLine();
                } while (!int.TryParse(input, out selected) || selected < 1 || selected > i);

                while (true)
                {
                    Console.WriteLine("Enter message for user: ");
                    var msg = Console.ReadLine();

                    NetTcpBinding tcpBinding = new NetTcpBinding();
                    tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
                    var serverName = list.Keys.ToList()[selected - 1];
                    Console.WriteLine("Selected user: " + serverName);
                    //Console.WriteLine("Waiting for service certificate...");
                    while (CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, serverName) == null)
                    {
                        Thread.Sleep(200);
                    }
                    X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, serverName);
                    EndpointAddress address1 = new EndpointAddress(new Uri($"net.tcp://localhost:{list[serverName]}/" + serverName), new X509CertificateEndpointIdentity(srvCert));
                    using (CommunicationClient.CommunicationClient client = new CommunicationClient.CommunicationClient(tcpBinding, address1))
                    {
                        client.SendMessage(msg);
                    }
                }

            }

            Console.WriteLine("Press <enter> to continue ...");
            Console.ReadLine();

            communicationService.Close();



            #region Staro
            ////registracija na server i otvaranje host-a za client-a
            //Random rnd = new Random();
            //int temp = rnd.Next(1, 1000);
            //string currentClientName = "marko" + temp.ToString();
            //Dictionary<string, string> activeClients;
            //string clientPort = null;
            ////Console.WriteLine("currentClientName: " + currentClientName);

            //using (ServerProxy proxy = new ServerProxy(binding, address))
            //{
            //    activeClients = proxy.RegisterClient(currentClientName);

            //    ServiceHost host = new ServiceHost(typeof(ChatService));
            //    ClientExtension.OpenHost(host, activeClients[currentClientName]);

            //    while (clientPort == null && activeClients.Count() > 1)
            //    {
            //        clientPort = ClientExtension.DisplayAndPickActiveClient(activeClients, currentClientName);
            //    }
            //}

            ////slanje poruka odabranom client-u
            //if (activeClients.Count() > 1)
            //{
            //    NetTcpBinding clientBinding = new NetTcpBinding();
            //    EndpointAddress clientAddress = new EndpointAddress(new Uri($"net.tcp://localhost:{clientPort}/ChatService"));

            //    string text = "";
            //    Console.WriteLine($"Send to client on port {clientPort}:");

            //    using (ClientProxy proxy = new ClientProxy(clientBinding, clientAddress))
            //    {
            //        while (text != "stop")
            //        {
            //            text = Console.ReadLine();
            //            proxy.SendMessage(currentClientName, text);
            //        }
            //    }
            //}

            //Console.ReadLine();
            #endregion

        }
    }
}
