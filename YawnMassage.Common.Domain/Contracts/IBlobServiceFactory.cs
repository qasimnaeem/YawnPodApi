using System.Threading.Tasks;
namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IBlobServiceFactory" />
    /// </summary>
    public interface IBlobServiceFactory
    {
        /// <summary>
        /// The CreateBlobServiceAsync
        /// </summary>
        /// <param name="container">The container<see cref="string"/></param>
        /// <param name="basePath">The basePath<see cref="string"/></param>
        /// <returns>The <see cref="Task{IBlobService}"/></returns>
        Task<IBlobService> CreateBlobServiceAsync(string container, string basePath);
    }
}
