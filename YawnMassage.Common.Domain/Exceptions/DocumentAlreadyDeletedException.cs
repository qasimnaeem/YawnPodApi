namespace YawnMassage.Common.Domain.Exceptions
{
    /// <summary>
    /// Defines the <see cref="DocumentAlreadyDeletedException" />
    /// </summary>
    public class DocumentAlreadyDeletedException : YawnMassageException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentAlreadyDeletedException"/> class.
        /// </summary>
        public DocumentAlreadyDeletedException() : base("ERROR_RECORD_ALREADY_DELETED")
        {
        }
    }
}
