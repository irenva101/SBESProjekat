using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    [ServiceContract]
    public interface ICommunication
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void SendMessage(string msg);
    }
}
