using Manager;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client.CommunicationService
{
    public class CommunicationProvider : ICommunication
    {
        public void SendMessage(string msg)
        {
            var receiver = Formatter.ParseName(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            var sender = Formatter.ParseNameFromCert(ServiceSecurityContext.Current.PrimaryIdentity.Name);

            Console.WriteLine($"Message received: {msg} sender:{sender}");

            var msg1 = $"{sender} --> {receiver}. Content: {msg}";



            var keyPath = receiver + ".key"; //nije dobro podesen path 

            Console.WriteLine(receiver);
            if (File.Exists(keyPath))
            {
                var key = File.ReadAllBytes(keyPath);
                var iv = File.ReadAllBytes(receiver + ".IV");

                var encrypted = AES.Encrypt(msg1, key, iv);

                //sladnje monitoring serveru
                //...
            }
        }
    }
}
