using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ChatService
    {
        private static readonly List<string> messages = new List<string>();
        public string GetMessages()
        {
            return string.Join("\n", messages);
        }

        public void SendMessage(string sender, string message)
        {
            messages.Add($"{sender}: {message}");
        }
    }
}
