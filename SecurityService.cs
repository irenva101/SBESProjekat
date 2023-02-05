﻿using Manager;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

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

        public Dictionary<string, X509Certificate2> GetRevocationList()
        {
            return revocationList;
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

        public void RevokeCertificate(X509Certificate2 cert)
        {
            var principal = OperationContext.Current.ServiceSecurityContext.WindowsIdentity;
            var username = Formatter.ParseName(principal.Name);

            revocationList.Add(username, cert);

            if (!cert.Issuer.Equals("CN=SbesCA"))
            {
                //GenerationCertificationSuccess EventLog
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
    }
}
