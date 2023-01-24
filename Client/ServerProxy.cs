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
    public class ServerProxy: ChannelFactory<ISecurityService>, IDisposable
    {
		ISecurityService factory;

		public ClientData ClientData;

		public ServerProxy(NetTcpBinding binding, EndpointAddress address)
			: base(binding, address)
		{
			/// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
			//string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name); //irenv je client, al mogu zakucati da je neki drugi
			string cltCertCN = "milena"; //zakucavamo radi testiranja
			this.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
			this.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
			this.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

			/// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
			this.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, cltCertCN); //klijentski sertifikat uzima

			factory = this.CreateChannel();
		}

		public Dictionary<string, string> RegisterClient(string clientName)
		{
			try
			{
				//string cltCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
				//string cltCertCN = "milena";
				//activeClients.Add(cltCertCN);
				var serverResponse = factory.RegisterClient(clientName);

				return serverResponse;
			}
			catch (Exception e)
			{
				Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
				return null;
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
