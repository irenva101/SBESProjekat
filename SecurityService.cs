using Manager;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class SecurityService : ISecurityService
    {
        
        private static Dictionary<string, int> activeUsers = new Dictionary<string, int>();
        private static Dictionary<string, X509Certificate2> revocationList = new Dictionary<string, X509Certificate2>();
        public Dictionary<string, int> GetAllActiveUsers()
        {
            return activeUsers;
        }

        public void IssueCertificate()
        {
            var principal = OperationContext.Current.ServiceSecurityContext.WindowsIdentity;
            var username = Formatter.ParseName(principal.Name);

            if (CertManager.GenerateCACertificate(username))
            {
                //GenerationCertificationSuccess EventLog
                try
                {
                    Audit.GenerationCertificationSuccess(username);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            // generate AES key
            using (AesManaged aes = new AesManaged())
            {
                File.WriteAllBytes(username + ".key", aes.Key);
                File.WriteAllBytes(username + ".IV", aes.IV);
            }

            //Logg activity


        }

        public void RevokeCertificate(X509Certificate2 cert)
        {
            var principal = OperationContext.Current.ServiceSecurityContext.WindowsIdentity;
            var username = Formatter.ParseName(principal.Name);

            revocationList.Add(username, cert);

            if (!cert.Issuer.Equals("CN=SbesCA")){
                //RevocationCertificationSuccess EventLog
                try
                {
                    Audit.RevocationCertSuccess(username);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }

        public void RegisterClient(int port)
        {
            var principal = OperationContext.Current.ServiceSecurityContext.WindowsIdentity;
            var username = Formatter.ParseName(principal.Name);

            

            if (activeUsers.ContainsKey(username))
            {
                activeUsers[username] = port;
            }
            else
            {
                activeUsers.Add(username, port);
            }
        }

        public Dictionary<string, X509Certificate2> GetRevocationList()
        {
            return revocationList;
        }
    }
}
