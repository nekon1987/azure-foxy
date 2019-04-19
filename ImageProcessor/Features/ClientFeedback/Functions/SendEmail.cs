// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.Features.ClientFeedback.Functions
{
    public static class SendEmail
    {

        // https://docs.microsoft.com/en-us/azure/azure-functions/functions-bindings-event-grid

            // todo: react to event

        [FunctionName("SendEmail")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation($"C# HTTP trigger function begun");

            var eventData = (SubscriptionValidationEventData)eventGridEvent.Data;


            //if (eventGridEvent.Data is SubscriptionValidationEventData)
            //{
            //    var eventData = (SubscriptionValidationEventData)eventGridEvent.Data;
            //    log.LogInformation($"Got SubscriptionValidation event data, validation code: {eventData.ValidationCode}, topic: {eventGridEvent.Topic}");
            //    // Do any additional validation (as required) and then return back the below response

            //    var responseData = new SubscriptionValidationResponse()
            //    {
            //        ValidationResponse = eventData.ValidationCode
            //    };

            //    return new OkObjectResult(responseData);
            //}


            // return new OkObjectResult("fuck off");
        }
    }
}
