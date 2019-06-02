namespace YawnMassage.Platform.Domain.Contracts.AlertDispatch
{
    /// <summary>
    /// Defines the <see cref="IAlertSenderServiceFactory" />
    /// </summary>
    public interface IAlertSenderServiceFactory
    {
        /// <summary>
        /// The GetAlertSenderService
        /// </summary>
        /// <param name="channel">The channel<see cref="string"/></param>
        /// <returns>The <see cref="IAlertSenderService"/></returns>
        IAlertSenderService GetAlertSenderService(string channel);
    }
}
