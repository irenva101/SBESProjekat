using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace ServiceContract
{
    [ServiceContract]
    public interface ISecurityService
    {
        [OperationContract]
        void RegisterClient(int port);

        [OperationContract]
        Dictionary<string, int> GetAllActiveUsers();

        [OperationContract]
        void IssueCertificate();

        [OperationContract]
        Dictionary<string, X509Certificate2> GetRevocationList();

        [OperationContract]
        void RevokeCertificate(X509Certificate2 cert);
    }
}
