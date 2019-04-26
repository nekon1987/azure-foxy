using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Rest;
using TopicConfiguration = ImageProcessor.Core.ConfigurationManager.EventGrid.TopicConfiguration;

namespace ImageProcessor.Core.Eventing.Gateways
{
    public enum EventGridTopic
    {
        ImageStorageTopic, ImageAnalysisTopic
    }

    

    public class EventGridGateway
    {
        private static readonly Dictionary<TopicConfiguration, EventGridClient> EventGridClientsForGridTopics = new Dictionary<TopicConfiguration, EventGridClient>();

        static EventGridGateway()
        {
            foreach (var gridTopic in ConfigurationManager.EventGrid.AllTopics)
            {
                EventGridClientsForGridTopics.Add(gridTopic, new EventGridClient(new TopicCredentials(gridTopic.TopicKey)));
            }
        }

        public async Task<bool> PublishEvents(IList<EventGridEvent> events, EventGridTopic eventGridTopic)
        {
            var eventGridClientsForGridTopic = EventGridClientsForGridTopics.Single(c => c.Key.TopicType == eventGridTopic);

            var eventGridClient = eventGridClientsForGridTopic.Value;
            var topicConfigurtion = eventGridClientsForGridTopic.Key;

            var azureResponse = await eventGridClient.PublishEventsWithHttpMessagesAsync(topicConfigurtion.TopicName, events);
            return azureResponse.Response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> PublishEvent(EventGridEvent eventData, EventGridTopic eventGridTopic)
        {
            return await PublishEvents(new List<EventGridEvent>() { eventData }, eventGridTopic);
        }
    }
}
