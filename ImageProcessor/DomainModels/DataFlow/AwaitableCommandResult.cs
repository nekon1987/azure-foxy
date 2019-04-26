using System;
using System.Collections.Generic;
using System.Text;

namespace ImageProcessor.DomainModels.DataFlow
{
    public enum CommandStatus
    {
        NotSet, InProgress, Retrying, CompletedSuccesfully, CompletedWithError
    }

    public class AwaitableCommandResult
    {
        public CommandStatus CommandStatus { get; set; }

        /// <summary>
        /// Describes the user session as a container for commands
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// Describes single flow of any kind which will likely have a result object that can be queried by client using this id
        /// </summary>
        public Guid CommandId { get; set; }

        /// <summary>
        /// This is an optional extension to a result status, could be an external entity of any kind
        /// </summary>
        public Guid ResultIdentifier { get; set; }

        /// <summary>
        /// This is a neat way of telling client how long it should wait before probing us for the results of some async operation
        /// we can change it in service layer depending on some logic - maybe double it each time client calls
        /// </summary>
        public long AwaitTimePeriodMiliseconds { get; set; }

        /// <summary>
        /// Generally its best to separate concerns and keep code creating classes in dedicated factories,
        /// but we will not do this for purposes of not over complicating the solution
        /// </summary>
        public static AwaitableCommandResult Create(Guid sessionId)
        {
            return new AwaitableCommandResult()
            {
                SessionId = sessionId,
                CommandId = Guid.NewGuid(),
                CommandStatus = CommandStatus.InProgress,
                AwaitTimePeriodMiliseconds = 1500
            };
        }

        public override string ToString()
        {
            return $"{SessionId} -> Status: {CommandStatus}";
        }
    }
}
