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
        Dictionary<string, string> RegisterClient(string clientName);

        //[OperationContract]
        //void DisplayActiveClients();

        //[OperationContract]
        //List<string> GetActiveClients();
    }
}
