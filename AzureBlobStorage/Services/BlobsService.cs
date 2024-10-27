using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

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

        public async Task<List<string>> GetAllBlobs()
        {
            var connection = Configuration.GetConnectionString("BlobConnectionString");
            BlobServiceClient serviceClient = new BlobServiceClient(connection);

            var container = Configuration["BlobContainerName"];
            BlobContainerClient containerClient = await GetBlobContainerClientAsync(container!);

            List<string> results = new List<string>();

            if (await containerClient.ExistsAsync())
            {
                BlobClient blobClient;
                BlobSasBuilder blobSasBuilder;
                await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
                {
                    blobClient = containerClient.GetBlobClient(blobItem.Name);

                    blobSasBuilder = new BlobSasBuilder()
                    {
                        BlobContainerName = containerClient.Name,
                        BlobName = blobItem.Name,
                        ExpiresOn = DateTime.UtcNow.AddMinutes(5),
                        Protocol = SasProtocol.Https
                    };
                    blobSasBuilder.SetPermissions(BlobSasPermissions.Read);

                    //results.Add(blobClient.Uri.ToString());
                    results.Add(blobClient.GenerateSasUri(blobSasBuilder).AbsolutePath);
                }
            }

            return results;  
        }
    }
}
