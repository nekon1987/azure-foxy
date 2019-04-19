using System;
using ImageProcessor.Features.ImageStorage.Eventing;
using Microsoft.Azure.EventGrid.Models;

namespace ImageProcessor.Features.ImageStorage.Factories
{
    public class EventsFactory
    {
        public EventGridEvent CreateNewGridEvent<TEventDefinition>(TEventDefinition eventDefinition) 
            where TEventDefinition : new()
        {
            return new EventGridEvent()
            {
                Topic = $"",
                Subject = "FoxyEvents",
                DataVersion = "1.0",
                Id = Guid.NewGuid().ToString(),
                EventTime = DateTime.UtcNow,
                EventType = typeof(TEventDefinition).Name,
                Data = eventDefinition
            };
        }

        public EventGridEvent CreateImageStoredEvent(Guid sessionId, Guid commandId, Guid imageId, string imageName, string partitionKey)
        {
            return CreateNewGridEvent(new ImageStoredEvent()
            {
                Name = imageName, ObjectId = imageId, SessionId = sessionId, CommandId = commandId, PartitionKey = partitionKey
            });
        }
    }
}
