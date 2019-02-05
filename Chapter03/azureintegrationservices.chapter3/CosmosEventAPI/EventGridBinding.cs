// This is the default URL for triggering event grid function in the local environment.
// http://localhost:7071/admin/extensions/EventGridExtensionConfig?functionName={functionname} 

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;

namespace AzFunctionsEventAPI
{
    public static class EventGridBinding
    {
        //private static string storageConnection = System.Environment.GetEnvironmentVariable("blobstorageconnection");
        private static readonly HttpClient client = new HttpClient();

        [FunctionName("EventGridBinding")]
        public static async System.Threading.Tasks.Task RunAsync([EventGridTrigger]JObject eventGridEvent, TraceWriter log)
        {
            log.Info(eventGridEvent.ToString(Formatting.Indented));
            if (eventGridEvent != null && eventGridEvent.Count > 0)
            {
                var jsonString = JsonConvert.SerializeObject(eventGridEvent);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://requestbin.fullcontact.com/1obunfk1", content);
            }

        }
    }
}
