using System.ServiceModel;

namespace ServiceContract
{
    [ServiceContract]
    public interface IMonitoringContract
    {
        [OperationContract]
        void SendMessageToLogs(byte[] message);
    }
}
