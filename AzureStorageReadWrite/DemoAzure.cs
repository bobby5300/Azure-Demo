using Azure.Storage.Entity;
using Azure.Storage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageReadWrite
{
    public class DemoAzure : IDisposable
    {

        private const string TableName = "MsgTbl";
        private const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=msgdmo;AccountKey=3xBsSlrwW//34ZhAcrawUVulI9YHjsASiWhgpPBV/hw2tSRdlPSd/r+Y4AK1QRIZTagTZ5tzz3MtOHZWR3cTag==;EndpointSuffix=core.windows.net";
        private static ITableStorage<TestTableEntity> tableStorage;

        public DemoAzure()
        {
            tableStorage = new TableStorage<TestTableEntity>(TableName, ConnectionString);
        }

        public static void GetRecord()
        {
            var a = tableStorage.GetEntitiesByRowKey("07ae0889-e688-46fb-844b-d560ea4deb65");
        }
        static void Main(string[] args)
        {
             GetRecord();
        }

        public void Dispose()
        {
            tableStorage.DeleteTable();
        }
    }
}
