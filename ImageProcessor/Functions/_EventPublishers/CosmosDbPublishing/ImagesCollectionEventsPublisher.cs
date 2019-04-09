using System;
using System.Collections.Generic;
using ImageProcessor.Helpers;
using Microsoft.Azure.Documents;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;

namespace ImageProcessor.Functions._EventPublishers.CosmosDbPublishing
{
    public class SomeImageMetadata
    {
        public string ValueAbc { get; set; }
    }

    public static class ImagesCollectionEventsPublisher
    {
        private static readonly EventsHelper _eventsHelper = new EventsHelper();

        [FunctionName("ImagesCollectionEventsPublisher")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ImageProcessor",
            collectionName: "Images",
            ConnectionStringSetting = "cstr-codb-neu-p-image-processor-01",
            LeaseCollectionName = "leases")]IReadOnlyList<Document> input, ILogger log)
        {
            // we should have simple event publisher based on rules depending on data

            var testEvent = _eventsHelper.PrepareEvent("testSubject", $"this is data created at {DateTime.Now}");
             _eventsHelper.PublishEvent(testEvent);

            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
