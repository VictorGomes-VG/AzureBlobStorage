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

        public async Task<BlobContainerClient> GetBlobContainerClientAsync(string containerName)
        {
            var conexao = Configuration.GetConnectionString("BlobConnectionString");
            BlobServiceClient serviceClient = new BlobServiceClient(conexao);
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
