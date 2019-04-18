using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessor.DomainModels
{
    public class ImageAnalysisData
    {
        public Guid RelatedImageObjectId { get; set; }
        public Guid RelatedImageObjectPartitionId { get; set; }
        public List<string> Celebrities { get; set; }
    }
}
