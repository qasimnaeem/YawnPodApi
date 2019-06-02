namespace YawnMassage.Common.Domain.Exceptions
{
    /// <summary>
    /// Defines the <see cref="DocumentDuplicateException" />
    /// </summary>
    public class DocumentDuplicateException : YawnMassageException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDuplicateException"/> class.
        /// </summary>
        public DocumentDuplicateException() : base("ERROR_DUPLICATE_RECORD")
        {
        }
    }
}
