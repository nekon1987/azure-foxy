// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using SubscriptionValidationResponse = Microsoft.Azure.EventGrid.Models.SubscriptionValidationResponse;

namespace ImageProcessor.Functions.SendEmailFunction
{
    public static class SendEmail
    {
        // https://docs.microsoft.com/en-us/azure/event-grid/receive-events
        [FunctionName("SendEmail")]
        public static IActionResult Run([EventGridTrigger] EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function begun");

            if (eventGridEvent.Data is SubscriptionValidationEventData)
            {
                var eventData = (SubscriptionValidationEventData)eventGridEvent.Data;
                log.LogInformation($"Got SubscriptionValidation event data, validation code: {eventData.ValidationCode}, topic: {eventGridEvent.Topic}");
                // Do any additional validation (as required) and then return back the below response

                var responseData = new SubscriptionValidationResponse()
                {
                    ValidationResponse = eventData.ValidationCode
                };

                return new OkObjectResult(responseData);
            }


            return new OkObjectResult("fuck off");
        }
    }
}
