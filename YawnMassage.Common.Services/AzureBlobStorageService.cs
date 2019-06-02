using YawnMassage.Common.Domain.Contracts;
using YawnMassage.Common.Domain.Dto.Blob;
using YawnMassage.Common.Domain.Enum;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services
{
    public class AzureBlobStorageService : IBlobService
    {
        private string _basePath;
        private CloudBlobContainer _blobContainer;

        public AzureBlobStorageService(string connectionString, string blobContainerName, string basePath)
        {
            _basePath = string.IsNullOrEmpty(basePath) ? string.Empty : basePath + "/";

            var storageAccount = CloudStorageAccount.Parse(connectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();

            _blobContainer = blobClient.GetContainerReference(blobContainerName);
        }

        public async Task CreateBlobContainerAsync(bool publicAccessAllowed)
        {
            var accessType = publicAccessAllowed ? BlobContainerPublicAccessType.Blob : BlobContainerPublicAccessType.Off;
            await _blobContainer.CreateIfNotExistsAsync(accessType, null, null);
        }

        public BlobSasTokenDto GenerateSasForBlob(string blobId, TimeSpan expiresIn, BlobSasPermissions permission)
        {
            var policy = new SharedAccessBlobPolicy();
            policy.Permissions = (SharedAccessBlobPermissions)permission;
            policy.SharedAccessExpiryTime = DateTime.UtcNow.Add(expiresIn);
            
            var blob = GetBlobReference(blobId);
            var sasToken = blob.GetSharedAccessSignature(policy);

            return new BlobSasTokenDto
            {
                BlobUri = blob.Uri.ToString(),
                SasToken = sasToken
            };
        }

        public async Task<Stream> OpenWriteBlobAsync(string blobId)
        {
            var blob = GetBlobReference(blobId);
            var stream = await blob.OpenWriteAsync();
            return stream;
        }

        public async Task WriteBlobAsync(string blobId, Stream source)
        {
            var blob = GetBlobReference(blobId);
            await blob.UploadFromStreamAsync(source);
        }

        public async Task WriteBlobAsync(string blobId, string text)
        {
            var blob = GetBlobReference(blobId);
            await blob.UploadTextAsync(text);
        }

        public Task WriteBlobAsync(string blobId, Stream source, TimeSpan timeToLive)
        {
            //Azure blob storage does not support expiration yet.
            throw new NotImplementedException();
        }

        public async Task<Stream> OpenReadBlobAsync(string blobId)
        {
            var blob = GetBlobReference(blobId);
            var stream = await blob.OpenReadAsync();
            return stream;
        }

        public async Task ReadBlobAsync(string blobId, Stream target)
        {
            var blob = GetBlobReference(blobId);
            await blob.DownloadToStreamAsync(target);
        }
        
        public async Task DeleteBlobAsync(string blobId)
        {
            await GetBlobReference(blobId).DeleteIfExistsAsync();
        }

        public async Task<byte[]> ReadBlobAsync(string blobId)
        {
            var blob = GetBlobReference(blobId);

            var bytes = new byte[blob.Properties.Length];
            await blob.DownloadToByteArrayAsync(bytes, 0);

            return bytes;
        }

        public async Task<bool> BlobExistsAsync(string blobId)
        {
            var blob = GetBlobReference(blobId);
            return await blob.ExistsAsync();
        }

        private CloudBlockBlob GetBlobReference(string blobId)
        {
            return _blobContainer.GetBlockBlobReference($"{_basePath}{blobId}");
        }

    }
}
