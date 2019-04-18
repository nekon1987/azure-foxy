using System;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.DomainModels;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace ImageProcessor.Gateways
{
    public class ImageProcessorRepository
    {
        private static readonly ObjectMapping ObjectMapping = new ObjectMapping();

        public FoxyResponse<ImageData> GetImageById(Guid objectId, Guid partitionId)
        {
            using (var client = new DocumentClient(new Uri(
                ConfigurationManager.Repository.CosmosDbEndpointUrl),
                ConfigurationManager.Repository.CosmosDbPrimaryAccessKey))
            {
                try
                {
                    var documentResponse = client.ReadDocumentAsync<ImageData>(
                        UriFactory.CreateDocumentUri("ImageProcessor", "Images", objectId.ToString()),
                        new RequestOptions { PartitionKey = new PartitionKey(partitionId.ToString()) });

                    documentResponse.Wait();

                    return FoxyResponse<ImageData>.Success(documentResponse.Result.Document);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public async Task<FoxyResponse<string>> StoreAnalysisData(ImageAnalysisData analysisData)
        {
            using (var client = new DocumentClient(new Uri(
                ConfigurationManager.Repository.CosmosDbEndpointUrl),
                ConfigurationManager.Repository.CosmosDbPrimaryAccessKey))
            {
                var documentResponse = await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri("ImageProcessor", "ImageAnalysisResults"), analysisData);
                return FoxyResponse<string>.Success(documentResponse.Resource.Id);

            }
        }
    }
}
