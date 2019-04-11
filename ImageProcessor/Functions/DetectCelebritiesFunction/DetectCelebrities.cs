// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using ImageProcessor.Core;
using ImageProcessor.EventModels;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace ImageProcessor.Functions.DetectCelebritiesFunction
{
    public static class DetectCelebrities
    {
        // https://docs.microsoft.com/en-us/azure/azure-functions/functions-debug-event-grid-trigger-local
        // ngrok http -host-header=localhost 7071

        // or https://github.com/pmcilreavy/AzureEventGridSimulator


        // Replace <Subscription Key> with your valid subscription key.
        const string subscriptionKey = "54ac46816459495f94b032d6e48f5dd3";

        // You must use the same Azure region in your REST API method as you used to
        // get your subscription keys. For example, if you got your subscription keys
        // from the West US region, replace "westcentralus" in the URL
        // below with "westus".
        //
        // Free trial subscription keys are generated in the "westus" region.
        // If you use a free trial subscription key, you shouldn't need to change
        // this region.
        const string uriBase = "https://northeurope.api.cognitive.microsoft.com/vision/v2.0/analyze";

        private static readonly ImageProcessorRepository ImageProcessorRepository = new ImageProcessorRepository();
        private static readonly ObjectMapping ObjectMapping = new ObjectMapping();

        [FunctionName("DetectCelebrities")]
        public static void Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            ImageStoredEvent imageStoredEvent = new ImageStoredEvent();

            ObjectMapping.Convert(eventGridEvent.Data, imageStoredEvent);

            ImageProcessorRepository.GetImageById(imageStoredEvent.ImageId);

            log.LogInformation(eventGridEvent.Data.ToString());
        }


        /// <summary>
        /// Gets the analysis of the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file to analyze.</param>
        static async Task MakeAnalysisRequest(byte[] imageBytes)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters. A third optional parameter is "details".
                // The Analyze Image method returns information about the following
                // visual features:
                // Categories:  categorizes image content according to a
                //              taxonomy defined in documentation.
                // Description: describes the image content with a complete
                //              sentence in supported languages.
                // Color:       determines the accent color, dominant color, 
                //              and whether an image is black & white.
                string requestParameters =
                    "visualFeatures=Categories,Description,Color";

                // Assemble the URI for the REST API method.
                string uri = uriBase + "?" + requestParameters;

                HttpResponseMessage response;
                
                // Add the byte array as an octet stream to the request body.
                using (ByteArrayContent content = new ByteArrayContent(imageBytes))
                {
                    // This example uses the "application/octet-stream" content type.
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Asynchronously call the REST API method.
                    response = await client.PostAsync(uri, content);
                }

                // Asynchronously get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                Console.WriteLine("\nResponse:\n\n{0}\n",
                    JToken.Parse(contentString).ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }
    }
}
