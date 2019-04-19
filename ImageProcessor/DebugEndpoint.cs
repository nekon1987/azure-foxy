using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Rest;

namespace ImageProcessor
{
    //public class Event
    //{
    //    /// <summary>
    //    /// Gets the unique identifier for the event.
    //    /// </summary>
    //    public string Id { get; }

    //    /// <summary>
    //    /// Gets the publisher defined path to the event subject.
    //    /// </summary>
    //    public string Subject { get; set; }

    //    /// <summary>
    //    /// Gets the registered event type for this event source.
    //    /// </summary>
    //    public string EventType { get; }

    //    /// <summary>
    //    /// Gets the time the event is generated based on the provider's UTC time.
    //    /// </summary>
    //    public string EventTime { get; }


    //    /// <summary>
    //    /// Constructor.
    //    /// </summary>
    //    public Event()
    //    {
    //        Id = Guid.NewGuid().ToString();
    //        EventType = "shipevent";
    //        EventTime = DateTime.UtcNow.ToString("o");
    //    }
    //}
    public class ResourceCredentials : ServiceClientCredentials
    {
        readonly string resourceKey;

        public ResourceCredentials(string resourceKey)
        {
            this.resourceKey = resourceKey;
        }

        public override void InitializeServiceClient<T>(ServiceClient<T> client)
        {
        }

        public override async Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("aeg-sas-key", this.resourceKey);
            await base.ProcessHttpRequestAsync(request, cancellationToken);
        }
    }


    public static class DebugEndpoint
    {
        private const string TopicKey = "Xj7Y7/vUGw3u6+bY3sXyB5wYPiWige9aOWwEym1Let8=";
        private const string TopicName = "egt-neu-p-images-01.northeurope-1.eventgrid.azure.net";

        [FunctionName("DebugEndpoint")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            ServiceClientTracing.IsEnabled = true;
            ServiceClientTracing.AddTracingInterceptor(new DebugTracer());

            log.LogInformation("C# HTTP trigger function processed a request.");
            ServiceClientCredentials topicCredentials = new TopicCredentials(TopicKey);
            EventGridClient eventGrid = new EventGridClient(topicCredentials);
            var res = eventGrid.PublishEventsWithHttpMessagesAsync(TopicName, GetEventsList());

            await res;

            log.LogInformation($"Server responded with: {res.Result.Response.StatusCode}");

            return (ActionResult) new OkObjectResult($"Hello");
        }

        class DebugTracer : IServiceClientTracingInterceptor
        {
            public void Information(string message)
            {
                Debug.WriteLine(message);
            }

            public void TraceError(string invocationId, Exception exception)
            {
                Debug.WriteLine("Exception in {0}: {1}", invocationId, exception);
            }

            public void ReceiveResponse(string invocationId, HttpResponseMessage response)
            {
                string requestAsString = (response == null ? string.Empty : response.AsFormattedString());
                Debug.WriteLine("invocationId: {0}\r\nresponse: {1}", invocationId, requestAsString);
            }

            public void SendRequest(string invocationId, HttpRequestMessage request)
            {
                string requestAsString = (request == null ? string.Empty : request.AsFormattedString());
                Debug.WriteLine("invocationId: {0}\r\nrequest: {1}", invocationId, requestAsString);
            }

            public void Configuration(string source, string name, string value)
            {
                Debug.WriteLine("Configuration: source={0}, name={1}, value={2}", source, name, value);
            }

            public void EnterMethod(string invocationId, object instance, string method, IDictionary<string, object> parameters)
            {
                Debug.WriteLine("invocationId: {0}\r\ninstance: {1}\r\nmethod: {2}\r\nparameters: {3}",
                    invocationId, instance, method, parameters.AsFormattedString());
            }

            public void ExitMethod(string invocationId, object returnValue)
            {
                string returnValueAsString = (returnValue == null ? string.Empty : returnValue.ToString());
                Debug.WriteLine("Exit with invocation id {0}, the return value is {1}",
                    invocationId, returnValueAsString);
            }
        }

        static IList<EventGridEvent> GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();

            for (int i = 0; i < 10; i++)
            {
                eventsList.Add(new EventGridEvent()
                {
                    Topic = $"",
                    Subject = $"Subject-{i}",
                    Id = Guid.NewGuid().ToString(),
                    EventTime = DateTime.Now,
                    EventType = "Microsoft.MockPublisher.TestEvent",
                    DataVersion = "1.0"
                });
            }

            return eventsList;
        }
    }

  
}
