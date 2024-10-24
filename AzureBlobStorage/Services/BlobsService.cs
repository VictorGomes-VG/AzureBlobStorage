using Azure.Storage.Blobs;

namespace AzureBlobStorage.Services
{
    public class BlobsService
    {
        public IConfiguration Configuration { get; set; }
        public BlobsService(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Get Container
        /// </summary>
        /// <param name="containerName"></param>
        /// <returns> containerClient </returns>
        public async Task<BlobContainerClient> GetBlobContainerClientAsync(string containerName)
        {
            var connection = Configuration.GetConnectionString("BlobConnectionString");
            BlobServiceClient serviceClient = new BlobServiceClient(connection);
            BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            return containerClient;
        }

        public async Task UploadBlobAsync(string blob)
        {
            var container = Configuration["BlobContainerName"];
            BlobContainerClient containerClient = await GetBlobContainerClientAsync(container!);
            BlobClient blobClient = containerClient.GetBlobClient(blob);

            await blobClient.UploadAsync($"D:/azure/imagens/{blob}");
        }
    }
}
