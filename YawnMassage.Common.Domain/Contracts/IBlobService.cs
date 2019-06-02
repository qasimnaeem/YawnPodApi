using System;
using System.IO;
using System.Threading.Tasks;
using YawnMassage.Common.Domain.Dto.Blob;
using YawnMassage.Common.Domain.Enum;

namespace YawnMassage.Common.Domain.Contracts
{
    /// <summary>
    /// Defines the <see cref="IBlobService" />
    /// </summary>
    public interface IBlobService
    {
        /// <summary>
        /// The CreateBlobContainerAsync
        /// </summary>
        /// <param name="publicAccessAllowed">The publicAccessAllowed<see cref="bool"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task CreateBlobContainerAsync(bool publicAccessAllowed);

        /// <summary>
        /// The OpenWriteBlobAsync
        /// </summary>
        /// <param name="blobId">The blobId<see cref="string"/></param>
        /// <returns>The <see cref="Task{Stream}"/></returns>
        Task<Stream> OpenWriteBlobAsync(string blobId);

        /// <summary>
        /// The WriteBlobAsync
        /// </summary>
        /// <param name="blobId">The blobId<see cref="string"/></param>
        /// <param name="source">The source<see cref="Stream"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task WriteBlobAsync(string blobId, Stream source);

        /// <summary>
        /// The WriteBlobAsync
        /// </summary>
        /// <param name="blobId">The blobId<see cref="string"/></param>
        /// <param name="text">The text<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task WriteBlobAsync(string blobId, string text);

        /// <summary>
        /// The OpenReadBlobAsync
        /// </summary>
        /// <param name="blobId">The blobId<see cref="string"/></param>
        /// <returns>The <see cref="Task{Stream}"/></returns>
        Task<Stream> OpenReadBlobAsync(string blobId);

        /// <summary>
        /// The ReadBlobAsync
        /// </summary>
        /// <param name="blobId">The blobId<see cref="string"/></param>
        /// <param name="target">The target<see cref="Stream"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task ReadBlobAsync(string blobId, Stream target);

        /// <summary>
        /// The DeleteBlobAsync
        /// </summary>
        /// <param name="blobId">The blobId<see cref="string"/></param>
        /// <returns>The <see cref="Task"/></returns>
        Task DeleteBlobAsync(string blobId);

        /// <summary>
        /// The BlobExistsAsync
        /// </summary>
        /// <param name="blobId">The blobId<see cref="string"/></param>
        /// <returns>The <see cref="Task{bool}"/></returns>
        Task<bool> BlobExistsAsync(string blobId);

        /// <summary>
        /// The GenerateSasForBlob
        /// </summary>
        /// <param name="blobId">The blobId<see cref="string"/></param>
        /// <param name="expiresIn">The expiresIn<see cref="TimeSpan"/></param>
        /// <param name="permission">The permission<see cref="BlobSasPermissions"/></param>
        /// <returns>The <see cref="BlobSasTokenDto"/></returns>
        BlobSasTokenDto GenerateSasForBlob(string blobId, TimeSpan expiresIn, BlobSasPermissions permission);
    }
}
