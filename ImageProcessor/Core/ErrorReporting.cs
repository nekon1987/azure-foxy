using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessor.Core
{
    public static class ErrorReporting
    {
        public static void ReportErrorToClient(string error)
        {
            Console.WriteLine(error);
        }
        public static void StoreExceptionDetails(Exception exception, Guid workflowSessionId)
        {
            Console.WriteLine(exception.ToString());
        }
    }
}
