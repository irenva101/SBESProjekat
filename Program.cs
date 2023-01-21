using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        //Certificates
        //TODO: Create Certificates (with or without private key)
        //TODO: Store Certificates on local folder for later distribution
        //TODO: Rewoke Certificates if compromised
        //TODO: Must contain RewocationList of revocated certificates
        //TODO: Log above in CMS Windows event log

        //Service
        //TODO: Provides list of connected clients with the information needed for clients to establish a connection with each other
        //TODO: Generates password and distributes with certificate
        //TODO: Log connections with clients and ended connections in Application Windows event log
        static void Main(string[] args)
        {
            string srvCertCN = "service"; //OVDE KAZEMO KOJI SERTIFIKAT KORISTIMO

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            string address = "net.tcp://localhost:9999/SecurityService";
            ServiceHost host = new ServiceHost(typeof(SecurityService));
            host.AddServiceEndpoint(typeof(ISecurityService), binding, address);

            ///Custom validation mode enables creation of a custom validator - CustomCertificateValidator
			host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom; //validacija je po chain of thrust principu
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);

            try
            {
                host.Open();
                Console.WriteLine("WCFService is started.\nPress <enter> to stop ...");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
            }
            finally
            {
                host.Close();
            }
        }
    }
}
