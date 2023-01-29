using System;
using System.Collections.Generic;
using System.Linq;
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
        void IssueCertificate();
    }
}
