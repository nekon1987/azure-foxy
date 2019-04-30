using System;
using System.Linq;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.Helpers;
using ImageProcessor.Core.SystemConfiguration;
using ImageProcessor.Features.WorkflowSession.Factories;
using ImageProcessor.Features.WorkflowSession.Gateways;
using ImageProcessor.Features.WorkflowSession.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.Features.ImageStorage.Functions
{
    public static class UploadImage
    {
        private static readonly RequestHelper RequestHelper = new RequestHelper();
        private static readonly WorkflowSessionService WorkflowSessionService = new WorkflowSessionService();
        private static readonly WorkflowSessionFactory WorkflowSessionFactory = new WorkflowSessionFactory();

        [FunctionName("UploadImage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            [CosmosDB("ImageProcessor", "Images", Id = "ObjectId",
                ConnectionStringSetting = "ConnectionStrings.ImageProcessor", CreateIfNotExists = true)] IAsyncCollector<object> outputDocuments)
        {
            var partitionKey = req.Query["UserName"].First();
            var worklowSession = WorkflowSessionFactory.CreateNewWorkflowSession(partitionKey);

            var awaitableCommandResult = WorkflowSessionService.CreateAwaitableCommandResultsInScopeOfSession(worklowSession).Result;
            if (!awaitableCommandResult.WasSuccessful)
                return new BadRequestObjectResult(awaitableCommandResult.Message);

            var imageResult = RequestHelper.ExtractSingleImageFromRequest(req, worklowSession.id);
            if (!imageResult.WasSuccessful)
                return new BadRequestObjectResult(imageResult.Message);

            var storeWorkflowResponse = WorkflowSessionService.StoreSession(worklowSession).Result;
            if (!storeWorkflowResponse.WasSuccessful)
                return new BadRequestObjectResult(storeWorkflowResponse.Message);

            await outputDocuments.AddAsync(new
            {
                sessionId = worklowSession.id,
                commandId = awaitableCommandResult.Content.CommandId,
                partitionKey = partitionKey,
                name = imageResult.Content.Name,
                bytes = imageResult.Content.Bytes
            });
            
            return new OkObjectResult(awaitableCommandResult);
        }
    }
}
