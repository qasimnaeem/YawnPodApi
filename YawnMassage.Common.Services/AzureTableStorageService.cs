using YawnMassage.Common.Domain.Contracts;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services
{
    public class AzureTableStorageService<T> : ITableService<T> where T : TableEntity, new()
    {
        private CloudTable table;

        public AzureTableStorageService(string connectionString, string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient();

            table = tableClient.GetTableReference(tableName);
        }

        public virtual async Task<T> GetAsync(string partitionKey, string rowKey)
        {
            var op = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var tableResult = await table.ExecuteAsync(op);

            return (T)tableResult.Result;
        }
        
        public virtual async Task InsertAsync(T entity)
        {
            var op = TableOperation.Insert(entity);
            await table.ExecuteAsync(op);
        }
        
        public virtual async Task InsertOrReplaceAsync(T entity)
        {
            var op = TableOperation.InsertOrReplace(entity);
            await table.ExecuteAsync(op);
        }

        public virtual async Task InsertOrMergeAsync(T entity)
        {
            var op = TableOperation.InsertOrMerge(entity);
            await table.ExecuteAsync(op);
        }

        public virtual async Task MergeAsync(T entity)
        {
            var op = TableOperation.Merge(entity);
            await table.ExecuteAsync(op);
        }
        
        public virtual async Task ReplaceAsync(T entity)
        {
            var op = TableOperation.Replace(entity);
            await table.ExecuteAsync(op);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            var op = TableOperation.Delete(entity);
            await table.ExecuteAsync(op);
        }
        
        public virtual async Task<IEnumerable<T>> GetPartitionQueryResultsAsync(string partitionKey, Func<TableQuery<T>, TableQuery<T>> query = null)
        {
            var list = new List<T>();
            TableContinuationToken token = null;

            var q = new TableQuery<T>().Where($"PartitionKey eq '{partitionKey}'");
            if (query != null)
                q = query.Invoke(q);

            do
            {
                var seg = await table.ExecuteQuerySegmentedAsync(q, token).ConfigureAwait(false);
                token = seg.ContinuationToken;
                list.AddRange(seg);
            } while (token != null);

            return list;
        }
    }
}
