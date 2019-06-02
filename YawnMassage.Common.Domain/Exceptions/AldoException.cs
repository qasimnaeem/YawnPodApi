using System;
namespace YawnMassage.Common.Domain.Exceptions
{
    /// <summary>
    /// Defines the <see cref="YawnMassageException" />
    /// </summary>
    public class YawnMassageException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YawnMassageException"/> class.
        /// </summary>
        public YawnMassageException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YawnMassageException"/> class.
        /// </summary>
        /// <param name="errorCode">The errorCode<see cref="string"/></param>
        public YawnMassageException(string errorCode) : base(errorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YawnMassageException"/> class.
        /// </summary>
        /// <param name="errorCode">The errorCode<see cref="string"/></param>
        /// <param name="innerException">The innerException<see cref="Exception"/></param>
        public YawnMassageException(string errorCode, Exception innerException) : base(errorCode, innerException)
        {
        }
    }
}
