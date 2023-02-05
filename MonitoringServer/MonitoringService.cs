using Manager;
using ServiceContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringServer
{
    public class MonitoringService : IMonitoringContract
    {
        private string messages_path = "messages.txt";
        public void SendMessageToLogs(byte[] message)
        {
            Console.WriteLine($"Message received.");
            var principal = OperationContext.Current.ServiceSecurityContext.WindowsIdentity;
            var username = Formatter.ParseName(principal.Name);
            string key = username + ".key";
            string iv = username + ".IV";
            string path = "C:/Users/irenv/Desktop/Novi Sbes - Copy/Server/bin/Debug/";

            if (File.Exists(path + key))
            {
                var k = File.ReadAllBytes(path + key);
                var i = File.ReadAllBytes(path + iv);
                var msg = AES.Decrypt(message, k, i);

                if (!File.Exists(path + messages_path))
                {
                    File.WriteAllText(path+messages_path, msg + ".\n ");

                }
                else
                {
                    File.AppendAllText(path+messages_path, msg + ".\n ");
                }
            }


        }
    }
}
