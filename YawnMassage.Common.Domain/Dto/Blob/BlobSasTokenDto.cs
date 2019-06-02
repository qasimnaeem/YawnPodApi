namespace YawnMassage.Common.Domain.Dto.Blob
{
    /// <summary>
    /// Defines the <see cref="BlobSasTokenDto" />
    /// </summary>
    public class BlobSasTokenDto
    {
        /// <summary>
        /// Gets or sets the SasToken
        /// </summary>
        public string SasToken { get; set; }

        /// <summary>
        /// Gets or sets the BlobUri
        /// </summary>
        public string BlobUri { get; set; }
    }
}
