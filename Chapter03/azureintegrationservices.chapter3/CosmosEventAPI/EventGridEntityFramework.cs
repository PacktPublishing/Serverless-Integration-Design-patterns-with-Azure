// This is the default URL for triggering event grid function in the local environment.
// http://localhost:7071/admin/extensions/EventGridExtensionConfig?functionName={functionname} 

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data.Entity;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

namespace AzFunctionsEventAPI
{
    public static class EventGridEntityFramework
    {
        [FunctionName("EventGridEntityFramework")]
        public static void Run([EventGridTrigger]JObject eventGridEvent, TraceWriter log)
        {
            log.Info(eventGridEvent.ToString(Formatting.Indented));

            if (eventGridEvent != null)
            {
                dynamic eventGridEvents = eventGridEvent.ToString(Newtonsoft.Json.Formatting.None);

                foreach (var azevent in eventGridEvents)
                {
                    using (var dbcontext = new DbContext(System.Environment.GetEnvironmentVariable("DBconneciton")))
                    {
                        dbcontext.Database.Connection.Open();
                        dbcontext.Database.ExecuteSqlCommandAsync(string.Format
                            ("Insert into dbo.eventgridevents(eventid,eventsubject ,eventdata)values ({0},{1},{2})",
                            azevent.Id, azevent.Subject, azevent.Data));

                        dbcontext.Database.Connection.Close();

                    }
                }
            }
            else
            {
                log.Info("Event payload is empty");
            }
        }
    }
}



