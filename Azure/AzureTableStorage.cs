using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure
{
    public class AzureTableStorage<T> where T : TableEntity, IAzureStorageTable, new()
    {
        CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=leaddata;AccountKey=wB7ldCPU9EI0JH3WaM3mx0q48e5uCDnkWmSrCNIIGttv+tCva07+5tAS5uiUKozcVuIQ+8gU3G5W04iU9Gi/BQ==");
            //ConfigurationManager.AppSettings["StorageConnectionString"]);

        CloudTableClient tableClient;
        CloudTable table;

        public AzureTableStorage(string tableName)
        {
            // Create the table client.
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference(tableName);
            // Create the table if it doesn't exist.
            table.CreateIfNotExists();
        }

        public T Insert(T entity)
        {
            TableOperation insertOperation = TableOperation.Insert(entity);
            table.Execute(insertOperation);
            return entity;
        }

        public List<T> ReadAll(string partitionKey)
        {
            List<T> entities = new List<T>();

            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            entities = table.ExecuteQuery(query).ToList();

            return entities;
        }

        public T Find(string partitionKey, string rowKey)
        {
            T entity = new T();

            TableQuery<T> query = new TableQuery<T>().Where(
                TableQuery.CombineFilters(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey)));

            var tableSet = table.ExecuteQuery(query).ToList();

            if (tableSet.Count >= 1)
            {
                return tableSet.First();
            }

            return null;
        }
    }
}
