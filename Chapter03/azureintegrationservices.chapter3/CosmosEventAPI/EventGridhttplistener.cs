using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzFunctionsEventAPI
{
    public static class EventGridhttplistener
    {
        private static readonly HttpClient client = new HttpClient();

        [FunctionName("EventGridhttplistener")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string requestContent = await req.Content.ReadAsStringAsync();

            EventGridSubscriber eventGridSubscriber = new EventGridSubscriber();
            EventGridEvent[] eventGridEvents = eventGridSubscriber.DeserializeEventGridEvents(requestContent);

            foreach (EventGridEvent azevents in eventGridEvents)
            {
                if (azevents.Data is SubscriptionValidationEventData)
                {
                    var EventData = (SubscriptionValidationEventData)azevents.Data;
                    var responseData = new SubscriptionValidationResponse()
                    {
                        ValidationResponse = EventData.ValidationCode
                    };
                    return req.CreateResponse(HttpStatusCode.OK, responseData);
                }
                else
                {
                    var jsonString = JsonConvert.SerializeObject(azevents);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://webhook.site/9d2ae6bf-cd79-4896-b44c-0e60a9e2061a", content);

                }
            }
            return req.CreateResponse(HttpStatusCode.OK);
        }
    }

}

