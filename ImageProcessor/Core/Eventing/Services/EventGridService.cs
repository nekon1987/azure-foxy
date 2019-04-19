using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventGrid.Models;

namespace ImageProcessor.Core.Eventing.Services
{
    public class EventGridService
    {
        private static readonly ObjectMapping ObjectMapping = new ObjectMapping();
            
        public TEvent MapToEventType<TEvent>(EventGridEvent eventGridEvent)
         where TEvent : new()
        {
            TEvent mappedEvent = new TEvent();
            ObjectMapping.Convert(eventGridEvent.Data, mappedEvent);
            return mappedEvent;
        }
    }
}
