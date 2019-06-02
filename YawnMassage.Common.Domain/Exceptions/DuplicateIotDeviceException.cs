using System;
namespace YawnMassage.Common.Domain.Exceptions
{
    /// <summary>
    /// Defines the <see cref="DuplicateIotDeviceException" />
    /// </summary>
    public class DuplicateIotDeviceException : YawnMassageException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateIotDeviceException"/> class.
        /// </summary>
        public DuplicateIotDeviceException() : base("ERROR_IOT_DEVICE_ALREADY_EXISTS")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateIotDeviceException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/></param>
        public DuplicateIotDeviceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateIotDeviceException"/> class.
        /// </summary>
        /// <param name="innerException">The innerException<see cref="Exception"/></param>
        public DuplicateIotDeviceException(Exception innerException) : base("ERROR_IOT_DEVICE_ALREADY_EXISTS", innerException)
        {
        }
    }
}
