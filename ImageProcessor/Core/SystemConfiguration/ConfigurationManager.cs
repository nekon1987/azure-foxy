using System;
using System.Collections.Generic;
using System.Text;
using ImageProcessor.Core.Eventing.Gateways;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace ImageProcessor.Core
{
    public static class ConfigurationManager
    {
        // https://medium.com/statuscode/getting-key-vault-secrets-in-azure-functions-37620fd20a0b

        public static class Repositories
        {
            public static string ImagesProcessorCosmosDbEndpointUrl
            {
                 get { return "https://localhost:8081"; }
            }
            public static string ImagesProcessorCosmosDbPrimaryAccessKey
            {
                get { return "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="; }
            }
        }

        public class EventGrid
        {
            public class TopicConfiguration
            {
                public EventGridTopic TopicType;
                public string TopicKey;
                public string TopicName;
            }

            public static TopicConfiguration ImageAnalysisTopic
            {
                get
                {
                    return new TopicConfiguration()
                    {
                        TopicType = EventGridTopic.ImageAnalysisTopic,
                        TopicKey = "KEY+ImageAnalysisTopic=",
                        TopicName = "localhost:60101"
                    };
                }
            }

            public static TopicConfiguration ImageStorageTopic
            {
                get
                {
                    return new TopicConfiguration()
                    {
                        TopicType = EventGridTopic.ImageStorageTopic,
                        TopicKey = "KEY+ImageStorageTopic=",
                        TopicName = "localhost:60102"
                    };
                }
            }

            public static List<TopicConfiguration> AllTopics = new List<TopicConfiguration>()
            {
                ImageAnalysisTopic, ImageStorageTopic
            };
        }

        public static class SendGrid
        {
            public static string SystemEmailAddress
            {
                get { return "noreply@foxy.com"; }
            }
        }
    }
}
