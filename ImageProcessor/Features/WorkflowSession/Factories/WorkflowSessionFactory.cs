using System;
using System.Collections.Generic;
using ImageProcessor.DomainModels.DataFlow;

namespace ImageProcessor.Features.WorkflowSession.Factories
{
    public class WorkflowSessionFactory
    {
        public FoxyWorkflowSession CreateNewWorkflowSession(string partitionKey)
        {
            return new FoxyWorkflowSession()
            {
                partitionKey = partitionKey,
                AwaitableCommandResults = new List<AwaitableCommandResult>(),
                id = Guid.NewGuid()
            };
        }
    }
}
