using System;
using System.Collections.Generic;
using ImageProcessor.Core;
using ImageProcessor.Core.Helpers;
using ImageProcessor.Gateways;
using ImageProcessor.Gateways.Factories;
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
        private static readonly EventsFactory EventsFactory = new EventsFactory();
        private static readonly EventPublisher EventPublisher = new EventPublisher();

        [FunctionName("ImagesCollectionEventsPublisher")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ImageProcessor",
            collectionName: "Images",
            ConnectionStringSetting = "dbg-cstr-codb-neu-p-image-processor-01", // todo - use debug config for local and prod
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true
            )]IReadOnlyList<Document> input, ILogger log)
        {

            foreach (var document in input)
            {
                // TODO: could use mapping here
                var partitionId = document.GetPropertyValue<Guid>("partitionId");
                var imageId = document.GetPropertyValue<Guid>("id");
                var imageName= document.GetPropertyValue<string>("name");

                var @event = EventsFactory.CreateImageStoredEvent(partitionId, imageId, imageName);

                EventPublisher.PublishEvent(@event);
            }
        }
    }
}
