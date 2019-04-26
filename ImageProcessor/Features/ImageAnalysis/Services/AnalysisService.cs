using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.Core.Eventing.Gateways;
using ImageProcessor.DomainModels;
using ImageProcessor.Features.ImageAnalysis.Factories;
using ImageProcessor.Features.ImageAnalysis.Gateways;
using Microsoft.Build.Utilities;

namespace ImageProcessor.Features.ImageAnalysis.Services
{
    public class AnalysisService
    {
        private static readonly AnalysisDbGateway AnalysisDbGateway = new AnalysisDbGateway();
        private static readonly VisionApiGateway VisionApiGateway = new VisionApiGateway();
        private static readonly EventGridGateway EventPublisher = new EventGridGateway();
        private static readonly EventsFactory EventsFactory = new EventsFactory();

        public async Task<FoxyResponse<ImageAnalysisData>> StoreAnalysisData(ImageAnalysisData imageAnalysisData, Guid imageId, Guid sessionId, string partitionKey)
        { 
            try
            {
                imageAnalysisData.RelatedImageObjectId = imageId;
                imageAnalysisData.partitionKey = partitionKey;
                return await AnalysisDbGateway.StoreAnalysisData(imageAnalysisData);
            }
            catch (Exception e)
            {
                ErrorReporting.StoreExceptionDetails(e, sessionId);
                return FoxyResponse<ImageAnalysisData>.Failure("Could not store image analysis results");
            }
        }

        public async Task<FoxyResponse<ImageAnalysisData>> LoadImagaAnalysisData(Guid analysisResultId, string partitionKey)
        {
            try
            {
                var analysisResult = await AnalysisDbGateway.LoadImagaAnalysisData(analysisResultId, partitionKey);
                return FoxyResponse<ImageAnalysisData>.Success(analysisResult);
            }
            catch (Exception e)
            {
                ErrorReporting.StoreExceptionDetails(e, analysisResultId);
                return FoxyResponse<ImageAnalysisData>.Failure("There was a problem while retreiving operation status");
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

        public async Task<FoxyEmptyResponse> RaiseAnalysisCompleteEvent(
            ImageAnalysisData imageAnalysisData, Guid sessionId, Guid commandId, string partitionKey)
        {
            try
            {

                var @event = EventsFactory.CreateAnalysisCompletedEvent(sessionId, commandId, imageAnalysisData.id, partitionKey);

                await EventPublisher.PublishEvent(@event, EventGridTopic.ImageAnalysisTopic);

                return FoxyEmptyResponse.Success();
            }
            catch (Exception e)
            {
                ErrorReporting.StoreExceptionDetails(e, sessionId);
                return FoxyEmptyResponse.Failure("Could not store image analysis results");
            }
        }
    }
}
