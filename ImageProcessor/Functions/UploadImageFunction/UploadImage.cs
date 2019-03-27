using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ImageProcessor.Functions.UploadImageFunction
{
    public static class UploadImage
    {
        [FunctionName("UploadImage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB("ImageProcessor", "Images", Id = "ImageId", ConnectionStringSetting = "cstr-codb-neu-p-image-processor-01")] IAsyncCollector<dynamic> outputDocuments,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string name = req.Query["name"];


            var outputDocument = new
            {
                ImageId = Guid.NewGuid(),
                name = name,
                content = "3333",
                outputDocument = "3333333"
            };

            await outputDocuments.AddAsync(outputDocument);


            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
