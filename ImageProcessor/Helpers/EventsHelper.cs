using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor.Functions;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Rest;

namespace ImageProcessor.Helpers
{
    public class EventsHelper
    {
        private const string TopicKey = "Xj7Y7/vUGw3u6+bY3sXyB5wYPiWige9aOWwEym1Let8=";
        private const string TopicName = "egt-neu-p-images-01.northeurope-1.eventgrid.azure.net";


        public async Task<bool> PublishEvents(IList<EventGridEvent> events)
        {
            ServiceClientCredentials topicCredentials = new TopicCredentials(TopicKey);
            EventGridClient eventGrid = new EventGridClient(topicCredentials);
            var res = eventGrid.PublishEventsWithHttpMessagesAsync(TopicName, events);

            await res;
            //log.LogInformation($"Server responded with: {res.Result.Response.StatusCode}");
            return true;
        }
        public async Task<bool> PublishEvent(EventGridEvent eventData)
        {
            return await PublishEvents(new List<EventGridEvent>() {eventData});
        }

        public EventGridEvent PrepareEvent(string subject, string testData)
        {
            var ev = new EventGridEvent()
            {
                Topic = $"",
                Subject = subject,
                Id = Guid.NewGuid().ToString(),
                Data = new EventSpecificData()
                {
                    Field1 = testData,
                    Field2 = "Value2",
                    Field3 = "Value3"
                },
                EventTime = DateTime.Now,
                EventType = "Microsoft.MockPublisher.TestEvent",
                DataVersion = "1.0"
            };

            return ev;
        }
    }
}
