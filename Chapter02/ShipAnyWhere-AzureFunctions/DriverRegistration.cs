using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;

namespace ShipAnyWhere
{
    public static class DriverRegistration
    {
        [FunctionName("DriverRegistration")]
        public static async Task<List<string>> Run(
            [OrchestrationTrigger] DurableOrchestrationContextBase context)
        {
            var outputs = new List<string>();

            outputs.Add(await context.CallActivityAsync<string>("BackgroundVerification", "name"));
            outputs.Add(await context.CallActivityAsync<string>("RegisterDriver", "Seattle"));
            outputs.Add(await context.CallActivityAsync<string>("BookTraining", "London"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("BackgroundVerification")]
        public static string BackgroundVerification([ActivityTrigger] DurableActivityContextBase context)
        {
            string name = context.GetInput<string>();
            return $"success";
        }

        [FunctionName("RegisterDriver")]
        public static string RegisterDriver([ActivityTrigger] DurableActivityContextBase context)
        {
            string name = context.GetInput<string>();
            return $"Id001";
        }

        [FunctionName("BookTraining")]
        public static string BookTraining([ActivityTrigger] DurableActivityContextBase context)
        {
            string name = context.GetInput<string>();
            return $"trained";
        }

    }
}
