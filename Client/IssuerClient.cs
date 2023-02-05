using ServiceContract;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Client
{
    public class IssuerClient : ChannelFactory<ISecurityService>, ISecurityService, IDisposable
    {
        ISecurityService factory;

        public IssuerClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address) => factory = this.CreateChannel();

        public void IssueCertificate()
        {
            try
            {
                factory.IssueCertificate();
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
            }
        }

        public Dictionary<string, int> GetAllActiveUsers()
        {
            try
            {
                return factory.GetAllActiveUsers();
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
            }
            return null;
        }

        public void RegisterClient(int port)
        {
            try
            {
                factory.RegisterClient(port);
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
            }
        }

        public void Dispose()
        {
            if (factory != null)
            {
                factory = null;
            }
            this.Close();
        }

        public Dictionary<string, X509Certificate2> GetRevocationList()
        {
            try
            {
                return factory.GetRevocationList();
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
            }
            return null;
        }

        public void RevokeCertificate(X509Certificate2 cert)
        {
            try
            {
                factory.RevokeCertificate(cert);
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
            }
        }
    }
}
