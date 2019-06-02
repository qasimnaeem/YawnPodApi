using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="ITableService{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITableService<T> where T : TableEntity, new()
    {
        /// <summary>
        /// The GetAsync
        /// </summary>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="rowKey">The rowKey<see cref="string"/></param>
        /// <returns>The <see cref="Task{T}"/></returns>
        Task<T> GetAsync(string partitionKey, string rowKey);

        /// <summary>
        /// The InsertAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task InsertAsync(T entity);

        /// <summary>
        /// The InsertOrReplaceAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task InsertOrReplaceAsync(T entity);

        /// <summary>
        /// The InsertOrMergeAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task InsertOrMergeAsync(T entity);

        /// <summary>
        /// The ReplaceAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task ReplaceAsync(T entity);

        /// <summary>
        /// The MergeAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task MergeAsync(T entity);

        /// <summary>
        /// The DeleteAsync
        /// </summary>
        /// <param name="entity">The entity<see cref="T"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteAsync(T entity);

        /// <summary>
        /// The GetPartitionQueryResultsAsync
        /// </summary>
        /// <param name="partitionKey">The partitionKey<see cref="string"/></param>
        /// <param name="query">The query<see cref="Func{TableQuery{T}, TableQuery{T}}"/></param>
        /// <returns>The <see cref="Task{IEnumerable{T}}"/></returns>
        Task<IEnumerable<T>> GetPartitionQueryResultsAsync(string partitionKey, Func<TableQuery<T>, TableQuery<T>> query = null);
    }
}
