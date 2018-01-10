using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace ReadMessagesHub
{
    class Program
    {
        static void Main(string[] args)
        {
            string eventHubConnectionString = "Endpoint=sb://vlabsdemo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=JcgNM+Na9bZnSP25YZEjUibFkY4YTxeSJM+6EDO4C5w=";
            string eventHubName = "vlabtest";
            string storageAccountName = "valabdemo";
            string storageAccountKey = "JbE2XHJdeECOWXpSMTQz9e0aVcpLBJpdplu9zoo+kVIi/yon7J9aVxomHTL2jCTv1zYtjkQwku+Z4kJUYmIt4g==";
            string storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", storageAccountName, storageAccountKey);

            string eventProcessorHostName = Guid.NewGuid().ToString();

            EventProcessorHost eventProcessorHost = 
                new EventProcessorHost(
                    eventProcessorHostName, 
                    eventHubName, 
                    EventHubConsumerGroup.DefaultGroupName, 
                    eventHubConnectionString, 
                    storageConnectionString);

            Console.WriteLine("Registering EventProcessor...");

            //var epo = new EventProcessorOptions()
            //{
            //    MaxBatchSize = 100,
            //    PrefetchCount = 1,
            //    ReceiveTimeOut = TimeSpan.FromSeconds(20)
            //};

            var options = new EventProcessorOptions();
            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };
            eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>(options).Wait();

            Console.WriteLine("Receiving. Press enter key to stop worker.");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }
    }
}
