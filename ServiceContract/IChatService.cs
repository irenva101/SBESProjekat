using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    [ServiceContract]
    public interface IChatService
    {
        [OperationContract]
        void SendMessage(string sender, string message);
        [OperationContract]
        string GetMessages();
    }
}
