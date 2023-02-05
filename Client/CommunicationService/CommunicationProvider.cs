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
        public void SendMessage(string msg, DateTime date)
        {
            var receiver = Formatter.ParseName(System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            var sender = Formatter.ParseNameFromCert(ServiceSecurityContext.Current.PrimaryIdentity.Name); 
            sender = sender.Remove(sender.Length - 1, 1); 

            Console.WriteLine($"Message received: {msg}<-----------sender:{sender}");

            //ConnectionSuccess EventLog
            try
            {
                Audit.ConnectionSuccess(sender, receiver);
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }


            var msg1 = $"At {date.Day}.{date.Month}.{date.Year}. {date.Hour}:{date.Minute}--------sender:{sender}--------receiver:{receiver}--------message:{msg}";

           

            var keyPath = receiver + ".key"; 
            string path = "C:/Users/irenv/Desktop/Novi Sbes - Copy/Server/bin/Debug/";


            Console.WriteLine(receiver);
            if (File.Exists(path+keyPath)){
                var key = File.ReadAllBytes(path + keyPath);
                var iv = File.ReadAllBytes(path+receiver + ".IV");

                var encrypted = AES.Encrypt(msg1, key, iv);

                var monitorClient = new MonitoringClient();
                monitorClient.SendMessageToLogs(encrypted);
            }
        }

        
    }
}
