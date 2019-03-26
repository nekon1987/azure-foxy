using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace imageprocessor
{
    public static class UploadImage
    {
        [FunctionName("UploadImage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            IAsyncCollector<object> outputDocuments,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string name = req.Query["name"];

            try
            {
                // https://fap-neu-p-image-processor-01.azurewebsites.net/api/UploadImage?code=1ybGWpIehDuDABZJCQwgFkxdesyBUkZQZziaGfCPXctzK6h1XEjUOw==
                // testing sc integration
                

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(requestBody);
                name = name ?? data?.name;

                //var requestContentString = await req.Content.ReadAsStringAsync();
                //var requestContent = JObject.Parse(requestContent);

                var outputDocument = new
                {
                    name = name,
                    content = "12s3",
                    outputDocument = "asd"
                };

                await outputDocuments.AddAsync(outputDocument);
            }
            catch (Exception ex)
            {
                log.LogInformation(ex.ToString());
            }

            return name != null
                ? (ActionResult)new OkObjectResult("All done")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}
