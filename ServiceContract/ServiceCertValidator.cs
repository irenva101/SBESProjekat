using Manager;
using System;
using System.IdentityModel.Selectors;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace ServiceContract
{
    public class ServiceCertValidator : X509CertificateValidator
    {
        public override void Validate(X509Certificate2 certificate)
        {
            /// This will take service's certificate from storage
            //X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine,
            //Formatter.ParseName(WindowsIdentity.GetCurrent().Name)); //ZAKOMENTARISALI JER ZAKUCAVAMO CLIENTA RADI TESTIRANJA

            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage
                (StoreName.My,
                StoreLocation.LocalMachine,
                Formatter.ParseName(WindowsIdentity.GetCurrent().Name));

            if (!certificate.Issuer.Equals(srvCert.Issuer))
            {
                throw new Exception("Certificate is not from the valid issuer.");
            }
        }
    }
}
