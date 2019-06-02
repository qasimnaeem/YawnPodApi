using System;
using System.Threading.Tasks;
namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="ILocalCacheService" />
    /// </summary>
    public interface ILocalCacheService
    {
        /// <summary>
        /// The GetValue
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="TValue"/></returns>
        TValue GetValue<TValue>(string key);

        /// <summary>
        /// The SetValue
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="value">The value<see cref="TValue"/></param>
        void SetValue<TValue>(string key, TValue value);

        /// <summary>
        /// The SetValue
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="value">The value<see cref="TValue"/></param>
        /// <param name="expiration">The expiration<see cref="TimeSpan"/></param>
        void SetValue<TValue>(string key, TValue value, TimeSpan expiration);

        /// <summary>
        /// The Remove
        /// </summary>
        /// <param name="keys">The keys<see cref="string[]"/></param>
        void Remove(params string[] keys);

        /// <summary>
        /// The KeyExists
        /// </summary>
        /// <param name="key">The key<see cref="string"/></param>
        /// <returns>The <see cref="bool"/></returns>
        bool KeyExists(string key);

        /// <summary>
        /// The GetOrSetValue
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="resolver">The resolver<see cref="Func{TValue}"/></param>
        /// <param name="expiration">The expiration<see cref="TimeSpan?"/></param>
        /// <returns>The <see cref="TValue"/></returns>
        TValue GetOrSetValue<TValue>(string key, Func<TValue> resolver, TimeSpan? expiration = null);

        /// <summary>
        /// The GetOrSetValueAsync
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key">The key<see cref="string"/></param>
        /// <param name="asyncResolver">The asyncResolver<see cref="Func{Task{TValue}}"/></param>
        /// <param name="expiration">The expiration<see cref="TimeSpan?"/></param>
        /// <returns>The <see cref="Task{TValue}"/></returns>
        Task<TValue> GetOrSetValueAsync<TValue>(string key, Func<Task<TValue>> asyncResolver, TimeSpan? expiration = null);
    }
}
