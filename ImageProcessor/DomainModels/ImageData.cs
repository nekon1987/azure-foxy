using System;

namespace ImageProcessor.DomainModels
{
    public class ImageData
    {
        public string Name { get; set; }
        public byte[] Bytes { get; set; }

        public override string ToString()
        {
            return $"{Name} -> {Bytes.Length} bytes";
        }
    }
}
