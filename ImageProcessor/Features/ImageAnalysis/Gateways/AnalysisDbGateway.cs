using System;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.DomainModels;
using Microsoft.Azure.Documents.Client;

namespace ImageProcessor.Features.ImageAnalysis.Gateways
{
    public class AnalysisDbGateway
    {
        public async Task<FoxyResponse<ImageAnalysisData>> StoreAnalysisData(ImageAnalysisData analysisData)
        {
            using (var client = new DocumentClient(new Uri(
                ConfigurationManager.Repository.CosmosDbEndpointUrl),
                ConfigurationManager.Repository.CosmosDbPrimaryAccessKey))
            {
                var documentResponse = await client.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri("ImageProcessor", "ImageAnalysisResults"), analysisData);

                analysisData.id = Guid.Parse(documentResponse.Resource.Id);

                return FoxyResponse<ImageAnalysisData>.Success(analysisData);
            }
        }
    }
}
