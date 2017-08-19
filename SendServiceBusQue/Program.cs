using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendServiceBusQue
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var connectionString = "Endpoint=sb://vlabbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=RqXbXuwh+VPat9XVnI4ACdDNncT45/RKcCxtBdOoOB8=";
                var queueName = "vlabque";

                var client = QueueClient.CreateFromConnectionString(connectionString, queueName);



                int cnt = 0;
                while (true)
                {
                    cnt++;
                    var message = new BrokeredMessage("This is a test message! send from code to Queue " + cnt);

                    Console.WriteLine(String.Format("Message id: {0}", message.MessageId));
                    Console.WriteLine(String.Format("Message Cnt: {0}", cnt));

                    client.Send(message);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            //Console.WriteLine("Message successfully sent! Press ENTER to exit program");
            //Console.ReadLine();
        }
    }
}
