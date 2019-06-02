using YawnMassage.Common.Domain.Constants;
using YawnMassage.Common.Domain.Contracts;
using System.Threading.Tasks;

namespace YawnMassage.Common.Services.Factories
{
    public class BlobServiceFactory : IBlobServiceFactory
    {
        private readonly IConfigurationReaderService _configurationReaderService;

        public BlobServiceFactory(IConfigurationReaderService configurationReaderService)
        {
            _configurationReaderService = configurationReaderService;
        }

        public async Task<IBlobService> CreateBlobServiceAsync(string container, string basePath)
        {
            var connectionString = await _configurationReaderService.GetValueAsync(SystemKeys.ConfigurationKeys.StoragePrimaryConnectionString);
            var blobService = new AzureBlobStorageService(connectionString, container, basePath);
            return blobService;
        }
    }
}
