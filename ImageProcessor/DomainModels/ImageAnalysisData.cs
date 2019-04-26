using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessor.DomainModels
{
    public class ImageAnalysisData
    {
        public Guid RelatedImageObjectId { get; set; }
        public Guid id { get; set; }
        public string partitionKey { get; set; }
        public List<string> Celebrities { get; set; }

        public override string ToString()
        {
            return $"{id} -> Celebrities found: {Celebrities?.Count ?? 0}";
        }
    }
}
