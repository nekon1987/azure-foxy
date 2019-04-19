using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.DomainModels;
using ImageProcessor.Features.ImageAnalysis.Gateways;
using Microsoft.Build.Utilities;

namespace ImageProcessor.Features.ImageAnalysis.Services
{
    public class AnalysisService
    {
        private static readonly AnalysisDbGateway AnalysisDbGateway = new AnalysisDbGateway();
        private static readonly VisionApiGateway VisionApiGateway = new VisionApiGateway();

        public async Task<FoxyResponse<ImageAnalysisData>> StoreAnalysisData(ImageAnalysisData imageAnalysisData, Guid imageId, Guid sessionId)
        {
            try
            {
                imageAnalysisData.RelatedImageObjectId = imageId;
                return await AnalysisDbGateway.StoreAnalysisData(imageAnalysisData);
            }
            catch (Exception e)
            {
                ErrorReporting.StoreExceptionDetails(e, sessionId);
                return FoxyResponse<ImageAnalysisData>.Failure("Could not store image analysis results");
            }
        }

        public async Task<FoxyResponse<ImageAnalysisData>> AnalyseImage(ImageData imageData, Guid sessionId)
        {
            try
            {
                var analysisResult = await VisionApiGateway.AnalyzeBytes(imageData.Bytes, sessionId);
                return FoxyResponse<ImageAnalysisData>.Success(analysisResult);
            }
            catch (Exception e)
            {
                ErrorReporting.StoreExceptionDetails(e, sessionId);
                return FoxyResponse<ImageAnalysisData>.Failure("There was a problem while analysing the image");
            }
        }
    }
}
