using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessor.Core
{
    public static class ConfigurationManager
    {
        public static class Repository
        {
            public static string CosmosDbEndpointUrl
            {
                get { return "https://localhost:8081"; }
            }
            public static string CosmosDbPrimaryAccessKey
            {
                get { return "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="; }
            }
        }


        public class EventGrid
        {
            // private const string TopicKey = "Xj7Y7/vUGw3u6+bY3sXyB5wYPiWige9aOWwEym1Let8=";
            // private const string TopicName = "egt-neu-p-images-01.northeurope-1.eventgrid.azure.net";

            public static string TopicKey
            {
                get { return "TheLocal+DevelopmentKey="; }
            }

            public static string TopicName
            {
                get { return "localhost:60101"; }
            }
        }

    }
}
