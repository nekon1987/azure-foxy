// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using ImageProcessor.Core;
using ImageProcessor.EventModels;
using ImageProcessor.Gateways;
using ImageProcessor.GatewaysAndRepositories;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ImageProcessor.Functions.DetectCelebritiesFunction
{
    public static class DetectCelebrities
    {
        // https://docs.microsoft.com/en-us/azure/azure-functions/functions-debug-event-grid-trigger-local
        // ngrok http -host-header=localhost 7071

        // or https://github.com/pmcilreavy/AzureEventGridSimulator

        private static readonly ImageProcessorRepository ImageProcessorRepository = new ImageProcessorRepository();
        private static VisionApiGateway VisionApiGateway = new VisionApiGateway();
        private static readonly ObjectMapping ObjectMapping = new ObjectMapping();

        [FunctionName("DetectCelebrities")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            ImageStoredEvent imageStoredEvent = new ImageStoredEvent();
            ObjectMapping.Convert(eventGridEvent.Data, imageStoredEvent);

            var repositoryResponse = ImageProcessorRepository.GetImageById(imageStoredEvent.ObjectId, imageStoredEvent.PartitionId);

            var result = VisionApiGateway.AnalyzeBytesAsync(repositoryResponse.Content.Bytes);

            result.Wait();

            result.Result.RelatedImageObjectPartitionId = imageStoredEvent.PartitionId;
            result.Result.RelatedImageObjectId = imageStoredEvent.ObjectId;

            ImageProcessorRepository.StoreAnalysisData(result.Result).Wait();
        }
    }
}
