using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AzEventGridClassLib;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace AzFunctionsEventAPI
{
    public static class EventGridSharedAccessSignature
    {
        private static string eventgridkey = System.Environment.GetEnvironmentVariable("eventgridkey");
        private static string eventgridendpoint = System.Environment.GetEnvironmentVariable("eventgridendpoint");
        [FunctionName("EventGridSharedAccessSignature")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            Productevent productname = await req.Content.ReadAsAsync<Productevent>();
            string name = productname.Productname;

            List<EventGridEvent> eventlist = new List<EventGridEvent>();
            for (int i = 0; i < 1; i++)
            {
                eventlist.Add(new EventGridEvent()
                {
                    Id = Guid.NewGuid().ToString(),
                    EventType = "integration.event.eventpublished",
                    EventTime = DateTime.Now,
                    Subject = "IntegrationEvent",
                    DataVersion = "1.0",
                    Data = new Productevent()
                    {
                        Productname = name

                    }

                });
            }
            string sharedaccesstoken =  SharedAccessSignature.GenerateSharedAccessSignature(eventgridendpoint, DateTime.Now.AddHours(1), eventgridkey);
            using (var reqclient = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, eventgridendpoint);
                requestMessage.Headers.Add("aeg-sas-token", sharedaccesstoken);
                var jsonString = JsonConvert.SerializeObject(eventlist);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                requestMessage.Content = content;
               HttpResponseMessage response = await reqclient.SendAsync(requestMessage);
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
