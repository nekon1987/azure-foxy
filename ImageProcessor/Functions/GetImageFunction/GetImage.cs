using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ImageProcessor.Functions.GetImageFunction
{
    public static class GetImage
    {
        [FunctionName("GetImage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,

            [CosmosDB(
                databaseName: "ImageProcessor",
                collectionName: "Images",
                ConnectionStringSetting = "dbg-cstr-codb-neu-p-image-processor-01",
                PartitionKey = "{Query.pid}",
                Id = "{Query.id}")]
                    dynamic imageEntity,

            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (imageEntity == null)
            {
                log.LogInformation($"ToDo item not found");
            }
            else
            {
                log.LogInformation($"Found ToDo item, Description={imageEntity.content}");
            }

            string name = req.Query["id"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? (ActionResult)new OkObjectResult($"Hello, {name}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
