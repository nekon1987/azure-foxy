using System;
using ImageProcessor.Core.Extensions;
using ImageProcessor.Core.SystemConfiguration.Enums;
using Microsoft.Extensions.Configuration;

namespace ImageProcessor.Core.SystemConfiguration.Models
{
    public class EventGridTopicConfiguration
    {
        public EventGridTopicType TopicType;
        public string TopicKey;
        public string TopicName;

        public static EventGridTopicConfiguration FromConfiguration(string eventGridTopicName, IConfigurationRoot configurationExtension)
        {
            try
            {
                var topicType = Enum.Parse<EventGridTopicType>(eventGridTopicName);
                var topicNameFromConfig = configurationExtension.LoadValueOrThrowException($"EventGrid.{topicType}.Name");
                var topicKeyFromConfig = configurationExtension.LoadValueOrThrowException($"EventGrid.{topicType}.Key");

                return new EventGridTopicConfiguration()
                {
                    TopicType = topicType,
                    TopicKey = topicKeyFromConfig,
                    TopicName = topicNameFromConfig
                };
            }
            catch (Exception e)
            {
                throw new Exception($"Unable to load event grid topic [{eventGridTopicName}] settings from configuration.", e);
            }
        
        }
    }
}
