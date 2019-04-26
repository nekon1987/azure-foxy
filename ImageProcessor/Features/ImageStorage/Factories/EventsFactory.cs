using System;
using ImageProcessor.Core.Eventing.Factories;
using ImageProcessor.Features.ImageStorage.Eventing;
using Microsoft.Azure.EventGrid.Models;

namespace ImageProcessor.Features.ImageStorage.Factories
{
    public class EventsFactory : BaseEventGridFactory
    {
        public EventGridEvent CreateImageStoredEvent(Guid sessionId, Guid commandId, Guid imageId, string imageName, string partitionKey)
        {
            return CreateNewGridEvent(new ImageStoredEvent()
            {
                Name = imageName, ObjectId = imageId, SessionId = sessionId, CommandId = commandId, PartitionKey = partitionKey
            });
        }
    }
}
