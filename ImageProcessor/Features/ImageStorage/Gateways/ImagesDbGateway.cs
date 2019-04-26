using System;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.DomainModels;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace ImageProcessor.Features.ImageStorage.Gateways
{
    public class ImagesDbGateway
    {
        public async Task<ImageData> GetImageById(Guid objectId, string partitionKey)
        {
            using (var client = new DocumentClient(new Uri(
                    ConfigurationManager.Repositories.ImagesProcessorCosmosDbEndpointUrl),
                ConfigurationManager.Repositories.ImagesProcessorCosmosDbPrimaryAccessKey))
            {
                var documentResponse = await client.ReadDocumentAsync<ImageData>(
                    UriFactory.CreateDocumentUri("ImageProcessor", "Images", objectId.ToString()),
                    new RequestOptions {PartitionKey = new PartitionKey(partitionKey)});

                return documentResponse.Document;
            }
        }
    }
}
