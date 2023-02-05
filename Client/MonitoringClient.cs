using Manager;
using ServiceContract;
using System;
using System.Security.Principal;
using System.ServiceModel;

namespace Client
{
    public class MonitoringClient : IMonitoringContract
    {
        readonly IMonitoringContract factory;

        public MonitoringClient()
        {
            var username = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.Transport;
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            binding.Security.Transport.ProtectionLevel = System.Net.Security.ProtectionLevel.EncryptAndSign;
            EndpointAddress address = new EndpointAddress(
                new Uri("net.tcp://localhost:9000/MonitoringService"),
                EndpointIdentity.CreateUpnIdentity(username));
            factory = new ChannelFactory<IMonitoringContract>(binding, address).CreateChannel();
        }

        public void SendMessageToLogs(byte[] message)
        {
            try
            {
                factory.SendMessageToLogs(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
