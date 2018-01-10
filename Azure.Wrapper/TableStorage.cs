using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using Azure.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System.Linq;

namespace Azure.Wrapper
{
    /// <summary>
    /// Simple helper class for Windows Azure storage tables
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TableStorage<T> : ITableStorage<T> where T : TableEntity, new()
    {

        private readonly CloudTable cloudTable;

        private readonly string TableName;

        /// <summary>
		/// Creates a new TableStorage object
		/// </summary>
		/// <param name="tableName">The name of the table to be managed</param>
		/// <param name="storageConnectionString">The connection string pointing to the storage account (this can be local or hosted in Windows Azure</param>
		public TableStorage(string tableName, string storageConnectionString)
        {
            Validate.TableName(tableName, "tableName");
            Validate.String(storageConnectionString, "storageConnectionString");

            var cloudStorageAccount = CloudStorageAccount.Parse(storageConnectionString);

            var requestOptions = new TableRequestOptions
            {
                RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(1), 3)
            };

            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            cloudTableClient.DefaultRequestOptions = requestOptions;

            TableName = tableName;
            cloudTable = cloudTableClient.GetTableReference(TableName);
            cloudTable.CreateIfNotExists();
        }

        public void CreateEntities(IEnumerable<T> entities)
        {
            Validate.Null(entities, "entities");
            var batchOperation = new TableBatchOperation();

            foreach (var entity in entities)
            {
                batchOperation.Insert(entity);
            }

            cloudTable.ExecuteBatch(batchOperation);
        }

        public void CreateEntity(T entity)
        {
            Validate.Null(entity, "entity");
            var insertOperation = TableOperation.Insert(entity);
            cloudTable.Execute(insertOperation);
        }

        public void DeleteEntitiesByPartitionKey(string partitionKey)
        {
            Validate.TablePropertyValue(partitionKey, "partitionKey");

            var query =
                new TableQuery<T>()
                    .Where(TableQuery.GenerateFilterCondition(
                        "PartitionKey",
                        QueryComparisons.Equal,
                        partitionKey));

            var results = cloudTable.ExecuteQuery(query);
            var batchOperation = new TableBatchOperation();
            var counter = 0;
            foreach (var entity in results)
            {
                batchOperation.Delete(entity);
                counter++;

                //Batch operations are limited to 100 items
                //when we reach 100, we commit and clear the operation
                if (counter == 100)
                {
                    cloudTable.ExecuteBatch(batchOperation);
                    batchOperation = new TableBatchOperation();
                    counter = 0;
                }
            }
        }

        public void DeleteEntitiesByRowKey(string rowKey)
        {
            Validate.TablePropertyValue(rowKey, "rowKey");

            var query =
                new TableQuery<T>()
                    .Where(TableQuery.GenerateFilterCondition(
                        "RowKey",
                        QueryComparisons.Equal,
                        rowKey));

            var results = cloudTable.ExecuteQuery(query);
            var batchOperation = new TableBatchOperation();
            var counter = 0;
            foreach (var entity in results)
            {
                batchOperation.Delete(entity);
                counter++;

                //Batch operations are limited to 100 items
                //when we reach 100, we commit and clear the operation
                if (counter == 100)
                {
                    cloudTable.ExecuteBatch(batchOperation);
                    batchOperation = new TableBatchOperation();
                    counter = 0;
                }
            }
        }

        public void DeleteEntity(string partitionKey, string rowKey)
        {
            Validate.TablePropertyValue(rowKey, "rowKey");
            Validate.TablePropertyValue(partitionKey, "partitionKey");

            var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var retrievedResult = cloudTable.Execute(retrieveOperation);

            var entityToDelete = retrievedResult.Result as T;
            if (entityToDelete != null)
            {
                var deleteOperation = TableOperation.Delete(entityToDelete);
                cloudTable.Execute(deleteOperation);
            }
        }

        public void DeleteTable()
        {
            cloudTable.DeleteIfExists();
        }

        public IEnumerable<T> GetEntitiesByPartitionKey(string partitionKey)
        {
            Validate.TablePropertyValue(partitionKey, "partitionKey");

            var query =
               new TableQuery<T>()
                   .Where(TableQuery.GenerateFilterCondition(
                       "PartitionKey",
                       QueryComparisons.Equal,
                       partitionKey));

            return cloudTable.ExecuteQuery(query).AsEnumerable();
        }

        public IEnumerable<T> GetEntitiesByRowKey(string rowKey)
        {
            Validate.TablePropertyValue(rowKey, "rowKey");

            var query =
               new TableQuery<T>()
                   .Where(TableQuery.GenerateFilterCondition(
                       "RowKey",
                       QueryComparisons.Equal,
                       rowKey));

            return cloudTable.ExecuteQuery(query).AsEnumerable();
        }

        public T GetEntityByPartitionKeyAndRowKey(string partitionKey, string rowKey)
        {
            var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);

            var retrievedResult = cloudTable.Execute(retrieveOperation);

            return retrievedResult.Result as T;
        }

        public void InsertOrUpdate(T entity)
        {
            Validate.Null(entity, "entity");
            var insertOrUpdateOperation = TableOperation.InsertOrMerge(entity);
            cloudTable.Execute(insertOrUpdateOperation);
        }
    }
}
