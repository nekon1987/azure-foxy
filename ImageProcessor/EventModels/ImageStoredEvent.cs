using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.EventGrid.Models;

namespace ImageProcessor.EventModels
{
    public class ImageStoredEvent
    {
        public Guid ImageId { get; set; }
        public string Name { get; set; }
    }
}
