using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessor.Core.DataObjects
{
    public class FoxyEmptyResponse
    {
        public bool WasSuccessful { get; set; }
        public string Message { get; set; }

        public static FoxyEmptyResponse Success()
        {
            return new FoxyEmptyResponse()
            {
                WasSuccessful = true
            };
        }

        public static FoxyEmptyResponse Failure(string message)
        {
            return new FoxyEmptyResponse()
            {
                Message = message
            };
        }
    }
}
