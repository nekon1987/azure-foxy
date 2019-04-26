using System;
using System.IO;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.DomainModels.DataFlow;
using ImageProcessor.Features.ImageAnalysis.Services;
using ImageProcessor.Features.ImageStorage.Services;
using ImageProcessor.Features.WorkflowSession.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ImageProcessor.Features.WorkflowSession.Functions
{
    public static class CheckCommandResult
    {
        private static readonly AnalysisService AnalysisService = new AnalysisService();
        private static readonly WorkflowSessionService WorkflowSessionService = new WorkflowSessionService();
        [FunctionName("CheckCommandResult")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var sessionId = Guid.Parse(req.Query["sessionId"]);
            var commandId = Guid.Parse(req.Query["commandId"]);
            var userName = req.Query["userName"];

            var commandCheckResult = await WorkflowSessionService.LoadSessionCommandResult(sessionId, commandId, userName);

            if(!commandCheckResult.WasSuccessful)
                ErrorReporting.ReportErrorToClient(commandCheckResult.Message);

            if (commandCheckResult.Content.CommandStatus == CommandStatus.CompletedSuccesfully)
            {
                var loadAnalysisResults = AnalysisService.LoadImagaAnalysisData(commandCheckResult.Content.ResultIdentifier, userName).Result;
                
                if(!loadAnalysisResults.WasSuccessful)
                    return new BadRequestObjectResult(loadAnalysisResults.Message);

                return new OkObjectResult(loadAnalysisResults.Content);
            }

            return new OkObjectResult(commandCheckResult.Content);
        }
    }
}
