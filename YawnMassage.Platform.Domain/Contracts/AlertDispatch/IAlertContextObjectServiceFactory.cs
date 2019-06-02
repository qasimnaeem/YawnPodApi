using YawnMassage.Common.Domain.Contracts;

namespace YawnMassage.Platform.Domain.Contracts.AlertDispatch
{
    /// <summary>
    /// Defines the <see cref="IAlertContextObjectServiceFactory" />
    /// </summary>
    public interface IAlertContextObjectServiceFactory
    {
        /// <summary>
        /// The GetAlertContextObjectService
        /// </summary>
        /// <param name="objectType">The objectType<see cref="string"/></param>
        /// <returns>The <see cref="IAlertContextObjectService"/></returns>
        IAlertContextObjectService GetAlertContextObjectService(string objectType);
    }
}
