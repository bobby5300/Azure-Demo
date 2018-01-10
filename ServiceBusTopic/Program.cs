using System;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace ServiceBusTopic
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Endpoint=sb://vlabbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=RqXbXuwh+VPat9XVnI4ACdDNncT45/RKcCxtBdOoOB8=";
            var topicName = "vlabdemotopic";

            var client = TopicClient.CreateFromConnectionString(connectionString, topicName);

            Employee employee = new Employee();
            employee.EmpId = "1001";
            employee.EmpName = "David";
            employee.DateOfHire = new DateTime(1973, 8, 5);

            string jsonData = JsonConvert.SerializeObject(employee);
            var message = new BrokeredMessage(jsonData);

            Console.WriteLine(String.Format("Message body: {0}", message.GetBody<string>()));
            Console.WriteLine(String.Format("Message id: {0}", message.MessageId));

            client.Send(message);

            Console.WriteLine("Message successfully sent! Press ENTER to exit program");
            Console.ReadLine();
        }
    }

    public class Employee
    {
        public string EmpId { get; set; }
        public string EmpName { get; set; }
        public DateTime DateOfHire { get; set; }
    }
}
