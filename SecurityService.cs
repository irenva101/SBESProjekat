using Manager;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class SecurityService : ISecurityService
    {
        //public static AsymmetricKeyParameter certCA;
        private static Dictionary<string, int> activeUsers = new Dictionary<string, int>();
        public Dictionary<string, int> GetAllActiveUsers()
        {
            return activeUsers;
        }

        public void IssueCertificate()
        {
            var principal = OperationContext.Current.ServiceSecurityContext.WindowsIdentity;
            var username = Formatter.ParseName(principal.Name);

            CertManager.GenerateCACertificate(username);

            // generate AES key
            using (AesManaged aes = new AesManaged())
            {
                File.WriteAllBytes(username + ".key", aes.Key);
                File.WriteAllBytes(username + ".IV", aes.IV);
            }

            //Logg activity

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
    }
}
