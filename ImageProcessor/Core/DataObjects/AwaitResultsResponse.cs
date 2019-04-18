using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace ImageProcessor.Core.DataObjects
{
    public class AwaitResultsResponse
    {
        public Guid AwaitCallbackIdentifier { get; set; }
        public long AwaitTimePeriodMiliseconds { get; set; }
    }
}
