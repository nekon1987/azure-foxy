using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;

namespace ImageProcessor.Functions.UploadImageFunction
{
    public static class UploadImage
    {
        private static readonly RequestHelper RequestHelper = new RequestHelper();

        [FunctionName("UploadImage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB("ImageProcessor", "Images", Id = "ObjectId", 
                    ConnectionStringSetting = "dbg-cstr-codb-neu-p-image-processor-01", CreateIfNotExists = true)]
                    IAsyncCollector<object> outputDocuments,
            ILogger log)
        {
            log.LogInformation("UploadImage function triggered by an incomming http request");
            var imageResult = RequestHelper.ExtractSingleImageFromRequest(req);

            if (imageResult.WasSuccessful)
            {
                await outputDocuments.AddAsync(new
                {
                    partitionId = Guid.NewGuid(),
                    name = imageResult.Content.Name,
                    bytes = imageResult.Content.Bytes
                });

                return new OkObjectResult(new AwaitResultsResponse()
                {
                    AwaitCallbackIdentifier = Guid.NewGuid() // how to not polute domain with this?
                });

            }
            else
            {
                return new BadRequestObjectResult($"There was a problem while storing the file: {imageResult.Message}");
            }
        }
    }
}
