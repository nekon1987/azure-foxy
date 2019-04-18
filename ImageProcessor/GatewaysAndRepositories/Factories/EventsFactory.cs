using System;
using ImageProcessor.EventModels;
using Microsoft.Azure.EventGrid.Models;

namespace ImageProcessor.Gateways.Factories
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

        public EventGridEvent CreateImageStoredEvent(Guid partitionId, Guid imageId, string imageName)
        {
            return CreateNewGridEvent(new ImageStoredEvent()
            {
                Name = imageName, ObjectId = imageId, PartitionId = partitionId
            });
        }
    }
}
