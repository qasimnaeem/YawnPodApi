namespace YawnMassage.Common.Domain.Enum
{
    // This enum values should be same as the Microsoft.WindowsAzure.Storage.Blob SharedAccessBlobPermissions
    public enum BlobSasPermissions
    {
        /// <summary>
        /// Defines the None
        /// </summary>
        None = 0,

        /// <summary>
        /// Defines the Read
        /// </summary>
        Read = 1,

        /// <summary>
        /// Defines the Write
        /// </summary>
        Write = 2,

        /// <summary>
        /// Defines the Delete
        /// </summary>
        Delete = 4,

        /// <summary>
        /// Defines the List
        /// </summary>
        List = 8,

        /// <summary>
        /// Defines the Add
        /// </summary>
        Add = 16,

        /// <summary>
        /// Defines the Create
        /// </summary>
        Create = 32
    }
}
