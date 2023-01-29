using Manager;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class IssuerClient : ChannelFactory<ISecurityService>, ISecurityService, IDisposable
    {
        ISecurityService factory;
        public IssuerClient(NetTcpBinding binding, EndpointAddress address) : base(binding, address)
        {
            var cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            factory = this.CreateChannel();
        }

        public void IssueCertificate()
        {
            try
            {
                factory.IssueCertificate(); //odavde treba da udje u SecurityServicezna
            }
            catch(Exception e)
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
            catch(Exception e)
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
            if(factory != null)
            {
                factory = null;
            }
            this.Close();
        }

   
    }
}
