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

namespace Client.CommunicationClient
{
    public class CommunicationClient : ChannelFactory<ICommunication>, ICommunication, IDisposable
    {
        ICommunication factory;

        public CommunicationClient(NetTcpBinding binding, EndpointAddress address): base(binding, address)
        {
            var cltCert= Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            this.Credentials.ClientCertificate.Certificate =
                CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCert);

            factory = this.CreateChannel();
        }

        public void SendMessage(string msg, DateTime now)
        {
            try
            {
                
                factory.SendMessage(msg,now);
            }
            catch (Exception ex) 
            {
                //ConnectionFailed EventLog
                try
                {
                    Audit.ConnectionFailed();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine(ex.Message);
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

        
    }
}
