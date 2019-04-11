using System;
using System.Collections.Generic;
using System.Text;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.DomainModels;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace ImageProcessor.Core
{
    public class ImageProcessorRepository
    {
        public FoxyResponse<ImageData> GetImageById(Guid imageId)
        {
            using (var client = new DocumentClient(new Uri(
                ConfigurationManager.Repository.CosmosDbEndpointUrl),
                ConfigurationManager.Repository.CosmosDbPrimaryAccessKey))
            {
                var response = client.ReadDocumentAsync(UriFactory.CreateDocumentUri("ImageProcessor", "Images", imageId.ToString()));

                return FoxyResponse<ImageData>.Success(new ImageData()
                {

                });
            }
        }
    }
}
