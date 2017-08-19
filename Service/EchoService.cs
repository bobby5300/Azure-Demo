using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    [ServiceBehavior(Name = "EchoService", Namespace = "http://samples.microsoft.com/ServiceModel/Relay/")]
    class EchoService : IEchoContract
    {
        
        public string Echo(string text)
        {
            Console.WriteLine("Echoing: {0}", text);
            return text;
        }

        public string GetCRM(string USERNAME, string PASSWORD)
        {
            return "CRM";
        }
    }
}
