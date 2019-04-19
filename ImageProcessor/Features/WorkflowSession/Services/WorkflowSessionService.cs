using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.DomainModels.DataFlow;
using ImageProcessor.Features.WorkflowSession.Factories;
using ImageProcessor.Features.WorkflowSession.Gateways;

namespace ImageProcessor.Features.WorkflowSession.Services
{
    public class WorkflowSessionService
    {
        private static readonly WorkflowsDbGateway WorkflowsDbGateway = new WorkflowsDbGateway();

        public async Task<FoxyResponse<AwaitableCommandResult>> CreateAwaitableCommandResultsInScopeOfSession(FoxyWorkflowSession workflowSession)
        {
            try
            {
                var awaitableCommandResult = AwaitableCommandResult.Create(workflowSession.id);
                workflowSession.AwaitableCommandResults.Add(awaitableCommandResult);

                return FoxyResponse<AwaitableCommandResult>.Success(awaitableCommandResult);
            }
            catch (Exception e)
            {
                ErrorReporting.StoreExceptionDetails(e, workflowSession.id);
                return FoxyResponse<AwaitableCommandResult>.Failure("There was a problem while preparing workflow session");
            }
        }

        public async Task<FoxyResponse<FoxyWorkflowSession>> StoreSession(FoxyWorkflowSession workflowSession)
        {
            try
            {
                workflowSession = await WorkflowsDbGateway.StoreWorkflowSession(workflowSession);
                return FoxyResponse<FoxyWorkflowSession>.Success(workflowSession);
            }
            catch (Exception e)
            {
                ErrorReporting.StoreExceptionDetails(e, Guid.Empty);
                return FoxyResponse<FoxyWorkflowSession>.Failure("There was a problem while storing workflow session");
            }
        }

        public async Task<FoxyResponse<AwaitableCommandResult>> LoadSessionCommandResult(Guid sessionId, Guid commandId, string partitionKey)
        {
            try
            {
                var workflowSession = await WorkflowsDbGateway.LoadFoxyWorkflowSession(sessionId, partitionKey);
                var requestedCommand = workflowSession.AwaitableCommandResults.Single(r => r.CommandId == commandId);
                return FoxyResponse<AwaitableCommandResult>.Success(requestedCommand);
            }
            catch (Exception e)
            {
                ErrorReporting.StoreExceptionDetails(e, sessionId);
                return FoxyResponse<AwaitableCommandResult>.Failure("There was a problem while retreiving operation status");
            }
        }
    

        public async Task<FoxyResponse<FoxyWorkflowSession>> StoreSessionCommandResult(
            Guid sessionId, Guid commandId, Guid resultEntityId, string partitionKey, CommandStatus status)
        {
            try
            {
                var workflowSession = await WorkflowsDbGateway.LoadFoxyWorkflowSession(sessionId, partitionKey);
                var commandResult = workflowSession.AwaitableCommandResults.SingleOrDefault(r => r.CommandId == commandId);

                if (commandResult == null)
                    throw new Exception($"Unable to find command result {commandId}");

                commandResult.CommandStatus = status;
                commandResult.ResultIdentifier = resultEntityId;

                workflowSession = await WorkflowsDbGateway.UpdateCommandResult(workflowSession);

                return FoxyResponse<FoxyWorkflowSession>.Success(workflowSession);
            }
            catch (Exception e)
            {
                ErrorReporting.StoreExceptionDetails(e, sessionId);
                return FoxyResponse<FoxyWorkflowSession>.Failure("There was a problem while updating operation status");
            }
        }
    }
}
