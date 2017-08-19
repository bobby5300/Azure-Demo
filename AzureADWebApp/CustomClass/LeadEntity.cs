using Azure;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureADWebApp.CustomClass
{
    public class LeadEntity : TableEntity, IAzureStorageTable
    {   

        public LeadEntity(string key, string id)
        {
            this.PartitionKey = key;
            this.RowKey = id;
            this.Timestamp = DateTime.Now;
        }

        public LeadEntity() { }
        public string LeadData { get; set; }
        public string SourceData { get; set; }
        public int Status{ get; set; }

        public void SetPartitionKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException();
            }
            this.PartitionKey = key;
        }

        public void SetRowKey(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException();
            }
            this.RowKey = id;
        }

    }
    
}