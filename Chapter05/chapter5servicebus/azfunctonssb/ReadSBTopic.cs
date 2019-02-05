using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using System;

namespace azfunctonssb
{
    public static class ReadSBTopic
    {

        [FunctionName("ReadSBTopic")]

        public static void Run([ServiceBusTrigger("sample01", "sample01-subscription", AccessRights.Manage,
        Connection = "SbConnection")]
        string mySbMsg, Int32 deliveryCount, DateTime enqueuedTimeUtc, string messageId,  TraceWriter log)

        {
            log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            log.Info($"C# deliveryCount: {deliveryCount}");
            log.Info($"C# enqueuedTimeUtc: {enqueuedTimeUtc}");
            log.Info($"C# messageId: {messageId}");
        }

    }
}

