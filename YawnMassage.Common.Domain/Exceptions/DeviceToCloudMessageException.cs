using System;
namespace YawnMassage.Common.Domain.Exceptions
{
    /// <summary>
    /// Defines the <see cref="DeviceToCloudMessageException" />
    /// </summary>
    public class DeviceToCloudMessageException : YawnMassageException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceToCloudMessageException"/> class.
        /// </summary>
        public DeviceToCloudMessageException() : base("ERROR_SENDING_DEVICE_TO_CLOUD_MESSAGE")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceToCloudMessageException"/> class.
        /// </summary>
        /// <param name="errorCode">The errorCode<see cref="string"/></param>
        public DeviceToCloudMessageException(string errorCode) : base(errorCode)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceToCloudMessageException"/> class.
        /// </summary>
        /// <param name="innerException">The innerException<see cref="Exception"/></param>
        public DeviceToCloudMessageException(Exception innerException) : base("ERROR_SENDING_DEVICE_TO_CLOUD_MESSAGE", innerException)
        {
        }
    }
}
