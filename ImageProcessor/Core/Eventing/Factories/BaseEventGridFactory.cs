using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventGrid.Models;

namespace ImageProcessor.Core.Eventing.Factories
{
    public class BaseEventGridFactory
    {
        public EventGridEvent CreateNewGridEvent<TEventDefinition>( TEventDefinition eventDefinition)
        where TEventDefinition : new()
        {
            return new EventGridEvent()
            {
                Topic = "",
                Subject = "FoxyEvents",
                DataVersion = "1.0",
                Id = Guid.NewGuid().ToString(),
                EventTime = DateTime.UtcNow,
                EventType = typeof(TEventDefinition).Name,
                Data = eventDefinition
            };
        }
    }
}
