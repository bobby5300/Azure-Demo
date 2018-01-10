 using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    [ServiceContract(Name = "IEchoContract", Namespace = "http://samples.microsoft.com/ServiceModel/Relay/")]
    public interface IEchoContract
    {
        [OperationContract]
        String Echo(string text);

        [OperationContract]
        String GetCRM(string Uname, string Password);
    }
    public interface IEchoChannel : IEchoContract, IClientChannel { }
}
