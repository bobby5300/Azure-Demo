using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Threading;
using Newtonsoft.Json;

namespace HubDemo
{
    class Program
    {

        private const string connectionString = "Endpoint=sb://vlabsdemo.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=JcgNM+Na9bZnSP25YZEjUibFkY4YTxeSJM+6EDO4C5w=";
        private const string eventHubName = "vlabtest";
        public static void Main(string[] args)
        {
            //MainAsync(args).GetAwaiter().GetResult();
            Console.WriteLine("Press Ctrl-C to stop the sender process");
            Console.WriteLine("Press Enter to start now");
            Console.ReadLine();
            SendingRandomMessages();
        }

        static void SendingRandomMessages()
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString, eventHubName);
            //while (true)
            // {
            try
            {
                List<object> list = new List<object>();
                list.Add("HeeloHub");
                for (int i = 0; i < list.Count; i++)
                {
                    var serializedMessage = JsonConvert.SerializeObject(list[i]);

                    Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, serializedMessage);

                    //EventData data = new EventData(Encoding.UTF8.GetBytes(serializedMessage));
                    eventHubClient.Send(new EventData(Encoding.UTF8.GetBytes(serializedMessage)));


                    //tasks.Add(eventHubClient.SendAsync(data));
                }
                //var message = "Hello World Message to  Demo Valb  " + Guid.NewGuid().ToString();


            }
            catch (Exception exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0} > Exception: {1}", DateTime.Now, exception.Message);
                Console.ResetColor();
            }

            //Thread.Sleep(200);
            //}
        }

        //private static async Task MainAsync(string[] args)
        //{
        //    // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
        //    // Typically, the connection string should have the entity path in it, but for the sake of this simple scenario
        //    // we are using the connection string from the namespace.
        //    var connectionStringBuilder = new EventHubsConnectionStringBuilder(EhConnectionString)
        //    {
        //        EntityPath = EhEntityPath
        //    };

        //    eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

        //    await SendMessagesToEventHub(100);

        //    await eventHubClient.CloseAsync();

        //    Console.WriteLine("Press ENTER to exit.");
        //    Console.ReadLine();
        //}

        //// Creates an event hub client and sends 100 messages to the event hub.
        //private static async Task SendMessagesToEventHub(int numMessagesToSend)
        //{
        //    for (var i = 0; i < numMessagesToSend; i++)
        //    {
        //        try
        //        {
        //            var message = $"Message {i}";
        //            Console.WriteLine($"Sending message: {message}");
        //            await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
        //        }
        //        catch (Exception exception)
        //        {
        //            Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
        //        }

        //        await Task.Delay(10);
        //    }

        //    Console.WriteLine($"{numMessagesToSend} messages sent.");
        //}
    }
}
