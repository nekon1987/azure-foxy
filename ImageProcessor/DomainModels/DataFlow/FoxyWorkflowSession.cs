using System;
using System.Collections.Generic;

namespace ImageProcessor.DomainModels.DataFlow
{
    public class FoxyWorkflowSession
    {
        // TODO: Do something so we can have it upper case in code
        public Guid id { get; set; }
        public string partitionKey { get; set; }
        public List<AwaitableCommandResult> AwaitableCommandResults { get; set; }

        public override string ToString()
        {
            return $"{id} -> Number of commands: {AwaitableCommandResults?.Count ?? 0}";
        }
    }
}
