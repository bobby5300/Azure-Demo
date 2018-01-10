using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.IO;

namespace ReadServiceBusTopic
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Endpoint=sb://vlabbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=RqXbXuwh+VPat9XVnI4ACdDNncT45/RKcCxtBdOoOB8=";
            var topicName = "vlabdemotopic";

            var client = SubscriptionClient.CreateFromConnectionString(connectionString, topicName, "VLabSub");

            client.OnMessage(message =>
            {

                Console.WriteLine(String.Format("Message body: {0}", message.GetBody<string>()));
                Console.WriteLine(String.Format("Message id: {0}", message.MessageId));
            });

            Console.WriteLine("Press ENTER to exit program");
            Console.ReadLine();
        }
    }
}
