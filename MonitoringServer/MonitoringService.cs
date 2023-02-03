using System;
using System.IO;
using System.ServiceModel;
using Manager;
using ServiceContract;

namespace MonitoringServer
{
    public class MonitoringService : IMonitoringContract
    {
        private const string Path = "C:/Users/irenv/Desktop/Novi Sbes - Copy/Server/bin/Debug/";
        private const string MessagesPath = "messages.txt";

        public void SendMessageToLogs(byte[] message)
        {
            Console.WriteLine($"Message received.");

            var principal = OperationContext.Current.ServiceSecurityContext.WindowsIdentity;
            var username = Formatter.ParseName(principal.Name);
            var key = username + ".key";
            var iv = username + ".IV";

            if (!File.Exists(Path + key))
            {
                return;
            }
            var k = File.ReadAllBytes(Path + key);
            var i = File.ReadAllBytes(Path + iv);
            var msg = AES.Decrypt(message, k, i);

            if (!File.Exists(Path + MessagesPath))
            {
                File.WriteAllText(Path + MessagesPath, msg + ".\n ");
            }
            else
            {
                File.AppendAllText(Path + MessagesPath, msg + ".\n ");
            }
        }
    }
}
