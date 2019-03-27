using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ImageProcessor.Functions.DetectCelebritiesFunction
{
    public static class DetectCelebrities
    {
        [FunctionName("DetectCelebrities")]
        public static void Run([CosmosDBTrigger(
            databaseName: "ImageProcessor",
            collectionName: "Images",
            ConnectionStringSetting = "cstr-codb-neu-p-image-processor-01",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true
            )]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }
        }
    }
}
