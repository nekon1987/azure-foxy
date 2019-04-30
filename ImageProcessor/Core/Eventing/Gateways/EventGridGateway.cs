using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ImageProcessor.Core.SystemConfiguration;
using ImageProcessor.Core.SystemConfiguration.Enums;
using ImageProcessor.Core.SystemConfiguration.Models;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

namespace ImageProcessor.Core.Eventing.Gateways
{
    public class EventGridGateway
    {
        private static readonly Dictionary<EventGridTopicConfiguration, EventGridClient> 
            EventGridClientsForGridTopics = new Dictionary<EventGridTopicConfiguration, EventGridClient>();

        static EventGridGateway()
        {
            LoadListOfAllRegisteredEventGridTopicsFromConfiguration();
        }

        public async Task<bool> PublishEvent(EventGridEvent eventData, EventGridTopicType eventGridTopicType)
        {
            return await PublishEvents(new List<EventGridEvent>() { eventData }, eventGridTopicType);
        }

        public async Task<bool> PublishEvents(IList<EventGridEvent> events, EventGridTopicType eventGridTopicType)
        {
            var eventGridClientsForGridTopic = EventGridClientsForGridTopics.SingleOrDefault(c => c.Key.TopicType == eventGridTopicType);
            
            var eventGridClient = eventGridClientsForGridTopic.Value;
            var topicConfigurtion = eventGridClientsForGridTopic.Key;

            var azureResponse = await eventGridClient.PublishEventsWithHttpMessagesAsync(topicConfigurtion.TopicName, events);
            return azureResponse.Response.StatusCode == HttpStatusCode.OK;
        }

        private static void LoadListOfAllRegisteredEventGridTopicsFromConfiguration()
        {
            foreach (var gridTopic in ConfigurationManager.EventGrid.AllTopics)
            {
                EventGridClientsForGridTopics.Add(gridTopic, new EventGridClient(new TopicCredentials(gridTopic.TopicKey)));
            }
        }
    }
}
