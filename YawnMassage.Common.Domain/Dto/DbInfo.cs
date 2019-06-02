namespace YawnMassage.Common.Domain.Dto
{
    /// <summary>
    /// Defines the <see cref="DbInfo" />
    /// </summary>
    public class DbInfo
    {
        /// <summary>
        /// Gets or sets the ServiceEndpoint
        /// </summary>
        public string ServiceEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the SasKey
        /// </summary>
        public string SasKey { get; set; }

        /// <summary>
        /// Gets or sets the DatabaseId
        /// </summary>
        public string DatabaseId { get; set; }

        /// <summary>
        /// Gets or sets the CollectionId
        /// </summary>
        public string CollectionId { get; set; }

        /// <summary>
        /// Gets or sets the EventsCollectionId
        /// </summary>
        public string EventsCollectionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether DisableConcurrencyCheck
        /// </summary>
        public bool DisableConcurrencyCheck { get; set; }
    }
}
