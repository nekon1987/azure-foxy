using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessor.Core.DataObjects
{
    public class FoxyResponse<TContent>
    {
        public bool WasSuccessful { get; set; }
        public string Message { get; set; }
        public TContent Content { get; set; }

        public static FoxyResponse<TContent> Failure(string message)
        {
            return new FoxyResponse<TContent>()
            {
                Message = message
            };
        }
        public static FoxyResponse<TContent> Success(TContent content)
        {
            return new FoxyResponse<TContent>()
            {
                WasSuccessful = true,
                Content = content
            };
        }
    }
}
