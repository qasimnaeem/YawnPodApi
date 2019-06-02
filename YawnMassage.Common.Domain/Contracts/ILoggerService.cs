using System;
namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="ILoggerService" />
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// The LogError
        /// </summary>
        /// <param name="errorMessage">The errorMessage<see cref="string"/></param>
        void LogError(string errorMessage);

        /// <summary>
        /// The LogError
        /// </summary>
        /// <param name="ex">The ex<see cref="Exception"/></param>
        /// <param name="errorMessage">The errorMessage<see cref="string"/></param>
        void LogError(Exception ex, string errorMessage);

        /// <summary>
        /// The LogInfo
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        void LogInfo(string message);

        /// <summary>
        /// The LogTrace
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        void LogTrace(string message);

        /// <summary>
        /// The LogDebug
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        void LogDebug(string message);

        /// <summary>
        /// The LogError
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="errorMessage">The errorMessage<see cref="string"/></param>
        void LogError<T>(string errorMessage);

        /// <summary>
        /// The LogError
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ex">The ex<see cref="Exception"/></param>
        /// <param name="errorMessage">The errorMessage<see cref="string"/></param>
        void LogError<T>(Exception ex, string errorMessage);

        /// <summary>
        /// The LogInfo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message<see cref="string"/></param>
        void LogInfo<T>(string message);

        /// <summary>
        /// The LogTrace
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message<see cref="string"/></param>
        void LogTrace<T>(string message);

        /// <summary>
        /// The LogDebug
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">The message<see cref="string"/></param>
        void LogDebug<T>(string message);
    }
}
