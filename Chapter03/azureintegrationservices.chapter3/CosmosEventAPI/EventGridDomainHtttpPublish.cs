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
    public static class EventGridDomainHttpPublisher
    {
        private static string eventgridkey = System.Environment.GetEnvironmentVariable("eventgridkey");
        private static string eventgridendpoint = System.Environment.GetEnvironmentVariable("eventgridendpoint");
        private static string topicHostname = new Uri(eventgridendpoint).Host;

        [FunctionName("EventGridDomainHttpPublisher")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            SocialPost post = await req.Content.ReadAsAsync<SocialPost>();
            List<EventGridEvent> eventlist = new List<EventGridEvent>();
            for (int i = 0; i < 1; i++)
            {
                eventlist.Add(new EventGridEvent()
                {
                    Topic= "sampleintegration02",
                    Id = Guid.NewGuid().ToString(),
                    EventType = "integration.event.eventpublished",
                    EventTime = DateTime.Now,
                    Subject = "IntegrationEvent",
                    DataVersion = "1.0",
                    Data = new SocialPost()
                    {
                        PostType = post.PostType,
                        PostedBy = post.PostedBy,
                        PostDescription = post.PostDescription,
                        id = Guid.NewGuid().ToString()

                    }

                });
                eventlist.Add(new EventGridEvent()
                {
                    Topic = "sampleintegration03",
                    Id = Guid.NewGuid().ToString(),
                    EventType = "integration.event.eventpublished",
                    EventTime = DateTime.Now,
                    Subject = "IntegrationEvent",
                    DataVersion = "1.0",
                    Data = new SocialPost()
                    {
                        PostType = post.PostType,
                        PostedBy = post.PostedBy,
                        PostDescription = post.PostDescription,
                        id = Guid.NewGuid().ToString()

                    }

                });
            }
            TopicCredentials topicCredentials = new TopicCredentials(eventgridkey);
            EventGridClient client = new EventGridClient(topicCredentials);
            client.PublishEventsAsync(topicHostname, eventlist).GetAwaiter().GetResult();
            return req.CreateResponse(HttpStatusCode.OK);
        }

    }
}
