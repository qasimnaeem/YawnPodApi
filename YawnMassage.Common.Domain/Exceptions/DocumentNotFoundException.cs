namespace YawnMassage.Common.Domain.Exceptions
{
    /// <summary>
    /// Defines the <see cref="DocumentNotFoundException" />
    /// </summary>
    public class DocumentNotFoundException : YawnMassageException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentNotFoundException"/> class.
        /// </summary>
        public DocumentNotFoundException() : base("ERROR_RECORD_NOT_FOUND")
        {
        }
    }
}
