using Manager;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

           // this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
           // this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
           // this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

           //this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN);
            

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
