using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessor.Features.ImageAnalysis.Eventing
{
    public class AnalysisCompletedEvent
    {
        public Guid SessionId { get; set; }
        public Guid CommandId { get; set; }
        public string PartitionKey { get; set; }
        public Guid AnalysisResultsId { get; set; }

        public override string ToString()
        {
            return $"{SessionId} -> {PartitionKey}";
        }
    }
}
