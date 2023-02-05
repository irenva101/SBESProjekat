using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
        Dictionary<string, X509Certificate2> GetRevocationList();

        [OperationContract]
        void IssueCertificate();
        [OperationContract]
         void RevokeCertificate(X509Certificate2 cert);
    }
}
