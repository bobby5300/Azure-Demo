using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveMsgtoDeadQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //MoveMessages();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static void MoveMessages()
        {
            const string Queue_Name = "leaddatainfo";
            const string ConnectionString = "Endpoint=sb://vlabbus.servicebus.windows.net/;SharedAccessKeyName=Lead;SharedAccessKey=lZeBZsmIo7+EU3t5h4CghrlEuzNIjatFjn4CsTmr3IE=";

            string deadLetterQueue = QueueClient.FormatDeadLetterPath(Queue_Name);

            QueueClient clientDeadLetter = QueueClient.CreateFromConnectionString(ConnectionString, deadLetterQueue);

            //var newMsg = new BrokeredMessage("Heelo");
            //clientDeadLetter.Send(newMsg);


            QueueClient client = QueueClient.CreateFromConnectionString(ConnectionString, Queue_Name);

            //int counter = 1;
            while (true)
            {
                BrokeredMessage deadMsg = clientDeadLetter.Receive();

                if (deadMsg == null)
                    break;

                BrokeredMessage newMsg = deadMsg.Clone();
                //client.Send(newMsg);
                deadMsg.Complete();
                Console.Write("\r {0}", newMsg.ToString());
                //counter++;
            }
        }
    }
}
