namespace YawnMassage.Common.Domain.Contracts.Factories
{
    /// <summary>
    /// Defines the <see cref="IGroupDataContextFactory" />
    /// </summary>
    public interface IGroupDataContextFactory
    {
        /// <summary>
        /// The CreateGroupDataContext
        /// </summary>
        /// <param name="groupId">The groupId<see cref="string"/></param>
        /// <returns>The <see cref="IGroupDataContext"/></returns>
        IGroupDataContext CreateGroupDataContext(string groupId);
    }
}
