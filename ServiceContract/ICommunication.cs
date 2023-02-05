using System;
using System.ServiceModel;

namespace ServiceContract
{
    [ServiceContract]
    public interface ICommunication
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void SendMessage(string msg, DateTime date);
    }
}
