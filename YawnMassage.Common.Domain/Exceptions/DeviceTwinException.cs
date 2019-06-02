using System;
namespace YawnMassage.Common.Domain.Exceptions
{
    /// <summary>
    /// Defines the <see cref="DeviceTwinUpdateException" />
    /// </summary>
    public class DeviceTwinUpdateException : YawnMassageException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceTwinUpdateException"/> class.
        /// </summary>
        public DeviceTwinUpdateException() : base("ERROR_IOT_DEVICE_TWIN_UPDATE")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceTwinUpdateException"/> class.
        /// </summary>
        /// <param name="errorCode">The errorCode<see cref="string"/></param>
        public DeviceTwinUpdateException(string errorCode) : base(errorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceTwinUpdateException"/> class.
        /// </summary>
        /// <param name="innerException">The innerException<see cref="Exception"/></param>
        public DeviceTwinUpdateException(Exception innerException) : base("ERROR_IOT_DEVICE_TWIN_UPDATE", innerException)
        {
        }
    }
}
