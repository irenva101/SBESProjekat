using ServiceContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ChatService : IChatService
    {
        public string GetMessages()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(string sender, string message)
        {
            //TODO: write into logger
            Console.WriteLine($"[{sender}]: {message}");
            Console.WriteLine(DateTime.Now.ToString());
        }
    }
}
