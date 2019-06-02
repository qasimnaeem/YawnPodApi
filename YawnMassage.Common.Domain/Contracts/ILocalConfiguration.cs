namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="ILocalConfiguration" />
    /// </summary>
    public interface ILocalConfiguration
    {
        /// <summary>
        /// Gets the SystemDataContextConnectionString
        /// </summary>
        string SystemDataContextConnectionString { get; }

        /// <summary>
        /// Gets the TenantCredentialsString
        /// </summary>
        string TenantCredentialsString { get; }
    }
}
