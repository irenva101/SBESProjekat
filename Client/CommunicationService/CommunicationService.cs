using ServiceContract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Manager;

namespace Client.CommunicationService
{
    public class CommunicationService
    {
        public string Address { get; private set; }
        public NetTcpBinding Binding { get; private set; }
        public ServiceHost Host { get; private set; }

        public CommunicationService(String address)
        {
            var username = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            this.Address = address;

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            this.Host = new ServiceHost(typeof(CommunicationProvider));
            this.Host.AddServiceEndpoint(typeof(ICommunication), binding, address);
            this.Host.Authorization.PrincipalPermissionMode = System.ServiceModel.Description.PrincipalPermissionMode.Custom;
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
            policies.Add(new CustomAuthorizationPolicy());
            this.Host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();

            //podesavanje AuditBehaviour-a
            ServiceSecurityAuditBehavior newAudit = new ServiceSecurityAuditBehavior();
            newAudit.AuditLogLocation = AuditLogLocation.Application;
            newAudit.ServiceAuthorizationAuditLevel = AuditLevel.SuccessOrFailure;

            this.Host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            this.Host.Description.Behaviors.Add(newAudit);

            this.Host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;
            this.Host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, username);

            var behavior = this.Host.Description.Behaviors.Find<ServiceDebugBehavior>();
            behavior.IncludeExceptionDetailInFaults = true;
        }
        public void Open()
        {
            try
            {
                Host.Open();
                Console.WriteLine("Client service on " + Address);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
                throw e;
            }
        }
        public void Close()
        {
            try
            {
                Host.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] {0}", e.Message);
                Console.WriteLine("[StackTrace] {0}", e.StackTrace);
                throw e;
            }
        }
    }
}
