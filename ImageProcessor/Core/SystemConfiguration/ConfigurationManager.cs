using System;
using System.Collections.Generic;
using ImageProcessor.Core.Eventing.Gateways;
using ImageProcessor.Core.Extensions;
using ImageProcessor.Core.SystemConfiguration.Enums;
using ImageProcessor.Core.SystemConfiguration.Models;
using Microsoft.Extensions.Configuration;

namespace ImageProcessor.Core.SystemConfiguration
{
    public static class ConfigurationManager
    {
        public static void Initialize(IConfigurationRoot configurationExtension)
        {
            Repositories.Load(configurationExtension);
            SendGrid.Load(configurationExtension);
            EventGrid.Load(configurationExtension);
        }

        public static class Repositories
        {
            public static string ImagesProcessorCosmosDbEndpointUrl;
            public static string ImagesProcessorCosmosDbPrimaryAccessKey;

            public static void Load(IConfigurationRoot configurationExtension)
            {
                ImagesProcessorCosmosDbEndpointUrl  = configurationExtension.LoadValueOrThrowException("Repositories.ImagesProcessorCosmosDbEndpointUrl");
                ImagesProcessorCosmosDbPrimaryAccessKey = configurationExtension.LoadValueOrThrowException("Repositories.ImagesProcessorCosmosDbPrimaryAccessKey");
            }
        }
        public static class SendGrid
        {
            public static string SystemEmailAddress { get; set; }

            public static void Load(IConfigurationRoot configurationExtension)
            {
                SystemEmailAddress = configurationExtension.LoadValueOrThrowException("SendGrid.SystemEmailAddress");
            }
        }

        public class EventGrid
        {
            public static EventGridTopicConfiguration ImageAnalysisTopic { get; set; }
            public static EventGridTopicConfiguration ImageStorageTopic { get; set; }

            public static List<EventGridTopicConfiguration> AllTopics => new List<EventGridTopicConfiguration>() {ImageAnalysisTopic, ImageStorageTopic};

            public static void Load(IConfigurationRoot configurationExtension)
            {
                ImageAnalysisTopic = EventGridTopicConfiguration.FromConfiguration("ImageAnalysisTopic", configurationExtension);
                ImageStorageTopic = EventGridTopicConfiguration.FromConfiguration("ImageStorageTopic", configurationExtension);
            }
        }
    }
}
