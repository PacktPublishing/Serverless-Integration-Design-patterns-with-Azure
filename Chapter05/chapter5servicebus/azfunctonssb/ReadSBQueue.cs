using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using System;

namespace azfunctonssb
{
    public static class ReadSBQueue
    {

        [FunctionName("ReadSBQueue")]
        public static void Run([ServiceBusTrigger("sample01", AccessRights.Manage, Connection = "SbConnection")]
        string myQueueItem, Int32 deliveryCount,DateTime enqueuedTimeUtc,string messageId, TraceWriter log)
        {
            log.Info($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            log.Info($"C# deliveryCount: {deliveryCount}");
            log.Info($"C# enqueuedTimeUtc: {enqueuedTimeUtc}");
            log.Info($"C# messageId: {messageId}");

        }
    }
}
