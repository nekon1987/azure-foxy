#r "Newtonsoft.Json"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public static async Task<IActionResult> Run(HttpRequest req,  IAsyncCollector<object> outputDocuments, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    try{
        // https://fap-neu-p-image-processor-01.azurewebsites.net/api/UploadImage?code=1ybGWpIehDuDABZJCQwgFkxdesyBUkZQZziaGfCPXctzK6h1XEjUOw==
        // testing sc integration
        string name = req.Query["name"];

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        name = name ?? data?.name;

        //var requestContentString = await req.Content.ReadAsStringAsync();
        //var requestContent = JObject.Parse(requestContent);

        var outputDocument = new {
            name = name,
            content = "12s3" ,
            outputDocument = "asd"
        }; 

        await outputDocuments.AddAsync(outputDocument);
    }
    catch(Exception ex)
    {
         log.LogInformation(ex.ToString());
    }

    return name != null
        ? (ActionResult)new OkObjectResult(outputDocument)
        : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
}
