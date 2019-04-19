using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.DomainModels;
using ImageProcessor.Features.ImageStorage.Gateways;
using Microsoft.Build.Utilities;

namespace ImageProcessor.Features.ImageStorage.Services
{
    public class ImagesStorageService
    {
        private static readonly ImagesDbGateway ImagesDbGateway = new ImagesDbGateway();

        public async Task<FoxyResponse<ImageData>> LoadImage(Guid imageId, Guid sessionId, string partitionKey)
        {
            try
            {
                var imageData = await ImagesDbGateway.GetImageById(imageId, partitionKey);
                return FoxyResponse<ImageData>.Success(imageData);
            }
            catch (Exception e)
            {
                ErrorReporting.StoreExceptionDetails(e, sessionId);
                return FoxyResponse<ImageData>.Failure("Unable to load image from the data store");
            }
        }
    }
}
