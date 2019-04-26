// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using ImageProcessor.Core;
using ImageProcessor.Core.Eventing.Services;
using ImageProcessor.Features.ClientFeedback.Services;
using ImageProcessor.Features.ImageAnalysis.Eventing;
using ImageProcessor.Features.ImageAnalysis.Services;
using ImageProcessor.Features.ImageStorage.Eventing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;

namespace ImageProcessor.Features.ClientFeedback.Functions
{
    public static class SendEmail
    {
        // https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-event-grid
        private static readonly EventGridService EventGridService = new EventGridService();
        private static readonly AnalysisService AnalysisService = new AnalysisService();
        private static readonly ClientFeedbackService ClientFeedbackService = new ClientFeedbackService();

        [FunctionName("SendEmail")]
        public static void Run(
            [EventGridTrigger]EventGridEvent eventGridEvent, ILogger log,
            [SendGrid(ApiKey = "SendGridApiKey")] out SendGridMessage message)
        {
            var analysisCompletedEvent = EventGridService.MapToEventType<AnalysisCompletedEvent>(eventGridEvent);
            var userEmail = analysisCompletedEvent.PartitionKey;

            var loadAnalysisResult = AnalysisService.LoadImagaAnalysisData(analysisCompletedEvent.AnalysisResultsId, analysisCompletedEvent.PartitionKey).Result;
            if(!loadAnalysisResult.WasSuccessful)
                ErrorReporting.ReportErrorToClient(loadAnalysisResult.Message);

            var createMessageResult = ClientFeedbackService.CreateAnalysisCompletedEmail(userEmail, loadAnalysisResult.Content).Result;
            if(!createMessageResult.WasSuccessful)
                ErrorReporting.ReportErrorToClient(createMessageResult.Message);

            message = createMessageResult.Content;
        }
    }
}
