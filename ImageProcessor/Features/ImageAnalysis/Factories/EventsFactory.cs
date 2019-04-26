using System;
using ImageProcessor.Core.Eventing.Factories;
using ImageProcessor.Features.ImageAnalysis.Eventing;
using Microsoft.Azure.EventGrid.Models;

namespace ImageProcessor.Features.ImageAnalysis.Factories
{
    public class EventsFactory : BaseEventGridFactory
    {
        public EventGridEvent CreateAnalysisCompletedEvent(Guid sessionId, Guid commandId, Guid analysisResultsId, string partitionKey)
        {
            return CreateNewGridEvent(new AnalysisCompletedEvent()
            {
                AnalysisResultsId = analysisResultsId, SessionId = sessionId, CommandId = commandId, PartitionKey = partitionKey
            });
        }
    }
}
