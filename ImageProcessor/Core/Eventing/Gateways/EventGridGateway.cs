using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Rest;

namespace ImageProcessor.Core.Eventing.Gateways
{
    public class EventGridGateway
    {
        private static readonly ServiceClientCredentials TopicCredentials = new TopicCredentials(ConfigurationManager.EventGrid.TopicKey);
        private static readonly EventGridClient EventGrid = new EventGridClient(TopicCredentials);

        public async Task<bool> PublishEvents(IList<EventGridEvent> events)
        {
            var azureResponse = await EventGrid.PublishEventsWithHttpMessagesAsync(ConfigurationManager.EventGrid.TopicName, events);
            return azureResponse.Response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> PublishEvent(EventGridEvent eventData)
        {
            return await PublishEvents(new List<EventGridEvent>() { eventData });
        }
    }
}
