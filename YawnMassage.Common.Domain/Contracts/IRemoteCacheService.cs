using System;
using System.Threading.Tasks;
namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IRemoteCacheService" />
    /// </summary>
    public interface IRemoteCacheService : ILocalCacheService
    {
        /// <summary>
        /// The GetValueAsync
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="Task{TValue}"/></returns>
        Task<TValue> GetValueAsync<TValue>(string key);

        /// <summary>
        /// The SetValueAsync
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="value">The value<see cref="TValue"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task SetValueAsync<TValue>(string key, TValue value);

        /// <summary>
        /// The SetValueAsync
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="value">The value<see cref="TValue"/></param>
        /// <param name="expiration">The expiration<see cref="TimeSpan"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task SetValueAsync<TValue>(string key, TValue value, TimeSpan expiration);

        /// <summary>
        /// The RemoveAsync
        /// </summary>
        /// <param name="keys">The keys<see cref="string[]"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task RemoveAsync(params string[] keys);

        /// <summary>
        /// The KeyExistsAsync
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        Task<bool> KeyExistsAsync(string key);
    }
}
