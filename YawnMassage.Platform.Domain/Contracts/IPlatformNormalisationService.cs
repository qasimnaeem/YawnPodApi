using System.Collections.Generic;
using System.Threading.Tasks;
using YawnMassage.Platform.Domain.Dto.DeviceKeyNormalisation;
using YawnMassage.Platform.Domain.Dto.DeviceNormalisation;
namespace YawnMassage.Platform.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IPlatformNormalisationService" />
    /// </summary>
    public interface IPlatformNormalisationService
    {
        /// <summary>
        /// The NormaliseLocalisationAsync
        /// </summary>
        /// <returns>The <see cref="Task{List{LocalisationDto}}"/></returns>
        Task<List<LocalisationDto>> NormaliseLocalisationAsync();

        /// <summary>
        /// The NormaliseLookupAsync
        /// </summary>
        /// <returns>The <see cref="Task{List{LookupDto}}"/></returns>
        Task<List<LookupDto>> NormaliseLookupAsync();
    }
}
