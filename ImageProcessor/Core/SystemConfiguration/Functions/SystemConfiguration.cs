// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using ImageProcessor.Core;
using ImageProcessor.Core.Eventing.Services;
using ImageProcessor.DomainModels.DataFlow;
using ImageProcessor.Features.ImageAnalysis.Gateways;
using ImageProcessor.Features.ImageAnalysis.Services;
using ImageProcessor.Features.ImageStorage.Eventing;
using ImageProcessor.Features.ImageStorage.Gateways;
using ImageProcessor.Features.ImageStorage.Services;
using ImageProcessor.Features.WorkflowSession.Gateways;
using ImageProcessor.Features.WorkflowSession.Services;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.Features.ImageAnalysis.Functions
{
    public static class SystemConfiguration
    {
        // https://docs.microsoft.com/en-us/azure/azure-functions/functions-debug-event-grid-trigger-local
        // ngrok http -host-header=localhost 7071

        // or https://github.com/pmcilreavy/AzureEventGridSimulator

        private static readonly EventGridService EventGridService = new EventGridService();
        private static readonly AnalysisService AnalysisService = new AnalysisService();
        private static readonly ImagesStorageService ImagesStorageService = new ImagesStorageService();
        private static readonly WorkflowSessionService WorkflowSessionService = new WorkflowSessionService();


        [FunctionName("DetectCelebrities22")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            // todo: break into private methods for readability + maybe wrapper for this if not succesful report client pattern
            var imageStoredEvent = EventGridService.MapToEventType<ImageStoredEvent>(eventGridEvent);
            var repositoryResponse = ImagesStorageService.LoadImage(imageStoredEvent.ObjectId, imageStoredEvent.SessionId, imageStoredEvent.PartitionKey).Result;
            if(!repositoryResponse.WasSuccessful)
                ErrorReporting.ReportErrorToClient(repositoryResponse.Message);

            var analysisResponse = AnalysisService.AnalyseImage(repositoryResponse.Content, imageStoredEvent.SessionId).Result;
            if (!analysisResponse.WasSuccessful)
                ErrorReporting.ReportErrorToClient(analysisResponse.Message);

            var analysisStorageResponse = AnalysisService.StoreAnalysisData(analysisResponse.Content, imageStoredEvent.ObjectId, imageStoredEvent.SessionId, imageStoredEvent.PartitionKey).Result;
            if(!analysisStorageResponse.WasSuccessful)
                ErrorReporting.ReportErrorToClient(analysisStorageResponse.Message);

            var raiseAnalysisSompletedEvent = AnalysisService.RaiseAnalysisCompleteEvent(
                analysisStorageResponse.Content, imageStoredEvent.SessionId, imageStoredEvent.CommandId, imageStoredEvent.PartitionKey).Result;

            if (!raiseAnalysisSompletedEvent.WasSuccessful)
                ErrorReporting.ReportErrorToClient(raiseAnalysisSompletedEvent.Message);

            var workflowUpdateResult = WorkflowSessionService.StoreSessionCommandResult(
                imageStoredEvent.SessionId, imageStoredEvent.CommandId, analysisStorageResponse.Content.id,
                imageStoredEvent.PartitionKey, CommandStatus.CompletedSuccesfully).Result;

            if (!workflowUpdateResult.WasSuccessful)
                ErrorReporting.ReportErrorToClient(workflowUpdateResult.Message);
        }
    }
}
