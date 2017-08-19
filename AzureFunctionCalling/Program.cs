using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks; 

namespace AzureFunctionCalling
{
    class Program
    {
        static void Main(string[] args)
        {

            //using (var client = new HttpClient())
            //{
            //    string requestUrl = $"https://vlabfunction.azurewebsites.net/api/HttpTriggerCSharp1?code=p9cKkIMe/idss5MfZGsK9T3Y98pwuVkaHlSgwToURLTXwkm7oBZatw==";
            //    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get,requestUrl);
            //    //log.Verbose(request.ToString());

            //    var res = client.SendAsync(request).Result;
            //    var responseString = res.Content.ReadAsStringAsync().Result;
                
            //}


            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("failedlead");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<LeadInfo> query = new TableQuery<LeadInfo>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "LeadData"));

            

            //var list = table.ExecuteQuery(query).ToList();

            // Print the fields for each customer.
            foreach (LeadInfo entity in table.ExecuteQuery(query))
            {
                Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                    entity.LeadData, entity.TempLeadID);
            }

            DeleteRow("63619fad-e4c7-4ac1-84a1-de1fbcef678d");




        }

        public static void DeleteRow(string Mykey)
        {
            try
            {
                // Retrieve the storage account from the connection string.
                CloudStorageAccount storageAccount1 = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

                // Create the table client.
                CloudTableClient tableClient1 = storageAccount1.CreateCloudTableClient();

                CloudTable table = tableClient1.GetTableReference("failedlead");

                TableQuery<LeadInfo> query = new TableQuery<LeadInfo>()
                                .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, Mykey));

                foreach (var item in table.ExecuteQuery(query))
                {
                    var oper = TableOperation.Delete(item);
                    table.Execute(oper);
                }
            }
            catch (Exception ex)
            {
                //LogErrorToAzure(ex);
                throw ex;
            }
        }
    }

    

    public class LeadInfo : TableEntity
    {       
        public string LeadData { get; set; }
        public string SourceData { get; set; }
        public string Status { get; set; }
        public string TempLeadID { get; set; }
        public string ProcessLeadID { get; set; }
    }
}
