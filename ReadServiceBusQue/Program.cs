using Microsoft.ServiceBus.Messaging;
using System;
using System.IO;
using System.Text;

namespace ReadServiceBusQue
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Endpoint=sb://vlabbus.servicebus.windows.net/;SharedAccessKeyName=failurelead;SharedAccessKey=RNzT1/BBEutdjiytLi/YDwwbr5iatbqR5/1cgD6Q/+0=";
            var queueName = "failurelead";

            //return string.Empty
            //var a = "hello";
            //string s = string.IsNullOrWhiteSpace(a)

            //var client = QueueClient.CreateFromConnectionString(connectionString, queueName);



            //var str = "Endpoint=sb://vlabbus.servicebus.windows.net/;SharedAccessKeyName=Failed;SharedAccessKey=HvxX2638gp8VnToMILOMxmWShlKOEsYnXhpK0PT8TYg=";
            //var queueName = "failedleaddata/$DeadLetterQueue";
            //string RespCRM = string.Empty;

            //QueueClient client1 = QueueClient.CreateFromConnectionString(str, queueName);

            //client1.OnMessage(message =>
            //{
            //    Console.WriteLine(message.ToString());
            //    //var bodyJson = new StreamReader(message.GetBody<Stream>(), Encoding.UTF8).ReadToEnd();
            //    ////log.Info(bodyJson);
            //    //dynamic JsonData = JsonConvert.DeserializeObject(bodyJson.ToString());
            //    //var LeadData = JsonData.LeadData.ToString();
            //    //var SourceInfo = JsonData.SourceData.ToString();
            //    //var TempLeadID = JsonData.LampLeadID.ToString();

            //    //RespCRM = SendCRM(LeadData, log);

            //    //log.Info("RespCRM" + RespCRM);

            //    //if (RespCRM != string.Empty)
            //    //{
            //    //    var results = SendLead(TempLeadID, "123456", SourceInfo, log);
            //    //    log.Info(results);
            //    //}
            //    //else
            //    //{
            //    //    ResendQueue(bodyJson, log);
            //    //    log.Info("Re-sendQueue" + bodyJson);
            //    //}
            //});



            QueueClient client1 = QueueClient.CreateFromConnectionString(connectionString, queueName, ReceiveMode.PeekLock);

            //while (client1.Receive() != null)
            //{

            //    var receivedMessage = client1.Receive();
            //    //Console.WriteLine(receivedMessage);
            //    var bodyJson = new StreamReader(receivedMessage.GetBody<Stream>(), Encoding.UTF8).ReadToEnd();
            //    Console.WriteLine(bodyJson);
            //    //var myMessage = JsonConvert.DeserializeObject<LeadInfo>(bodyJson);
            //    //do something with the message here
            //    //receivedMessage?.Complete();
            //}



            client1.OnMessage(message =>
            {
                //var a = message.GetBody<string>();
                var bodyJson = new StreamReader(message.GetBody<Stream>(), Encoding.UTF8).ReadToEnd();
                Console.WriteLine(bodyJson);
                //Console.WriteLine(String.Format("Message body: {0}", message.GetBody<String>()));
                Console.WriteLine(String.Format("Message id: {0}", message.MessageId));
                message.Abandon();
            });

            Console.WriteLine("Press ENTER to exit program");
            Console.ReadLine();
        }
    }

    public class LeadInfo
    {
        public string name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string area { get; set; }
        public string society { get; set; }
        public string address { get; set; }
        public string task { get; set; }
        public string leadlampid { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }


    }
}
