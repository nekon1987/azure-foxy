using System;
using System.Threading.Tasks;
using ImageProcessor.Core;
using ImageProcessor.Core.DataObjects;
using ImageProcessor.Core.SystemConfiguration;
using ImageProcessor.DomainModels;
using ImageProcessor.DomainModels.DataFlow;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace ImageProcessor.Features.WorkflowSession.Gateways
{
    public class WorkflowsDbGateway
    {
        public async Task<FoxyWorkflowSession> LoadFoxyWorkflowSession(Guid workflowSessionId, string partitionKey)
        {
            using (var client = new DocumentClient(new Uri(
                    ConfigurationManager.Repositories.ImagesProcessorCosmosDbEndpointUrl),
                ConfigurationManager.Repositories.ImagesProcessorCosmosDbPrimaryAccessKey))
            {

                var documentResponse = await client.ReadDocumentAsync<FoxyWorkflowSession>(
                    UriFactory.CreateDocumentUri("ImageProcessor", "WorkflowSessions", workflowSessionId.ToString()),
                    new RequestOptions {PartitionKey = new PartitionKey(partitionKey)});

                return documentResponse.Document;
            }
        }

        public async Task<FoxyWorkflowSession> StoreWorkflowSession(FoxyWorkflowSession workflowSession)
        {
            using (var client = new DocumentClient(new Uri(
                ConfigurationManager.Repositories.ImagesProcessorCosmosDbEndpointUrl),
                ConfigurationManager.Repositories.ImagesProcessorCosmosDbPrimaryAccessKey))
            {
                var documentResponse = await client.CreateDocumentAsync(UriFactory
                    .CreateDocumentCollectionUri("ImageProcessor", "WorkflowSessions"), workflowSession);

                workflowSession.id = Guid.Parse(documentResponse.Resource.Id);

                return workflowSession;
            }
        }

        public async Task<FoxyWorkflowSession> UpdateCommandResult(FoxyWorkflowSession workflowSession)
        {
            using (var client = new DocumentClient(new Uri(
             ConfigurationManager.Repositories.ImagesProcessorCosmosDbEndpointUrl),
             ConfigurationManager.Repositories.ImagesProcessorCosmosDbPrimaryAccessKey))
            {
                var documentResponse = await client.UpsertDocumentAsync(UriFactory
                    .CreateDocumentCollectionUri("ImageProcessor", "WorkflowSessions"), workflowSession);

                return workflowSession;
            }
        }
    }
}
