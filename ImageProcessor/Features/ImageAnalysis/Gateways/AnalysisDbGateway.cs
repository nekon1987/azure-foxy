using System;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.Core.SystemConfiguration;
using ImageProcessor.DomainModels;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace ImageProcessor.Features.ImageAnalysis.Gateways
{
    public class AnalysisDbGateway
    {
        public async Task<ImageAnalysisData> StoreAnalysisData(ImageAnalysisData analysisData)
        {
            using (var client = new DocumentClient(new Uri(
                ConfigurationManager.Repositories.ImagesProcessorCosmosDbEndpointUrl),
                ConfigurationManager.Repositories.ImagesProcessorCosmosDbPrimaryAccessKey))
            {
                var documentResponse = await client.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri("ImageProcessor", "ImageAnalysisResults"), analysisData);

                analysisData.id = Guid.Parse(documentResponse.Resource.Id);

                return analysisData;
            }
        }

        public async Task<ImageAnalysisData> LoadImagaAnalysisData(Guid analysisResultId, string partitionKey)
        {
            using (var client = new DocumentClient(new Uri(
                    ConfigurationManager.Repositories.ImagesProcessorCosmosDbEndpointUrl),
                ConfigurationManager.Repositories.ImagesProcessorCosmosDbPrimaryAccessKey))
            {

                var documentResponse = await client.ReadDocumentAsync<ImageAnalysisData>(
                    UriFactory.CreateDocumentUri("ImageProcessor", "ImageAnalysisResults", analysisResultId.ToString()),
                    new RequestOptions { PartitionKey = new PartitionKey(partitionKey) });

                return documentResponse.Document;
            }
        }
    }
}
