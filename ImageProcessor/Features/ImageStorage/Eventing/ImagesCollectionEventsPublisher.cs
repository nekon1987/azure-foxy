using System;
using System.Collections.Generic;
using ImageProcessor.Core.Eventing.Gateways;
using ImageProcessor.Core.SystemConfiguration.Enums;
using ImageProcessor.Features.ImageStorage.Factories;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.Features.ImageStorage.Eventing
{
    public static class ImagesCollectionEventsPublisher
    {
        private static readonly EventsFactory EventsFactory = new EventsFactory();
        private static readonly EventGridGateway EventPublisher = new EventGridGateway();

        [FunctionName("ImagesCollectionEventsPublisher")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ImageProcessor",
            collectionName: "Images",
            ConnectionStringSetting = "ConnectionStrings.ImageProcessor", 
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true
            )]IReadOnlyList<Document> input, ILogger log)
        {

            foreach (var document in input)
            {
                // TODO: could use mapping here
                var sessionId = document.GetPropertyValue<Guid>("sessionId");
                var commandId = document.GetPropertyValue<Guid>("commandId");
                var partitionKey = document.GetPropertyValue<string>("partitionKey");
                var imageId = document.GetPropertyValue<Guid>("id");
                var imageName= document.GetPropertyValue<string>("name");

                var @event = EventsFactory.CreateImageStoredEvent(sessionId, commandId, imageId, imageName, partitionKey);

                EventPublisher.PublishEvent(@event, EventGridTopicType.ImageStorageTopic).Wait();
            }
        }
    }
}
