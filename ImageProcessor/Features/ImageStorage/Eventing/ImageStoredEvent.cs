using System;

namespace ImageProcessor.Features.ImageStorage.Eventing
{
    public class ImageStoredEvent
    {
        public Guid SessionId { get; set; }
        public Guid CommandId { get; set; }
        public string PartitionKey { get; set; }
        public Guid ObjectId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{SessionId} -> {Name}";
        }
    }
}
