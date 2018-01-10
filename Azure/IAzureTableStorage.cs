using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure
{
    public interface IAzureStorageTable
    {
        void SetPartitionKey(string Key);
        void SetRowKey(string id);
    }
}
