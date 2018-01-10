using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Azure.Storage.Interface
{
    public interface ITableStorage<T> where T : TableEntity, new()
    {
        void CreateEntity(T entity);
        void CreateEntities(IEnumerable<T> entities);
        void InsertOrUpdate(T entity);
        void DeleteEntitiesByPartitionKey(string partitionKey);
        void DeleteEntitiesByRowKey(string rowKey);
        void DeleteEntity(string partitionKey, string rowKey);
        void DeleteTable();
        IEnumerable<T> GetEntitiesByPartitionKey(string partitionKey);
        IEnumerable<T> GetEntitiesByRowKey(string rowKey);
        T GetEntityByPartitionKeyAndRowKey(string partitionKey, string rowKey);

    }
}
