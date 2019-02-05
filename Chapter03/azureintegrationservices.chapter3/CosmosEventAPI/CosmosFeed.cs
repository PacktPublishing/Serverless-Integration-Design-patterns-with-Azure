using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace AzFunctionsEventAPI
{
    public static class CosmosFeed
    {
        private static readonly HttpClient client = new HttpClient();

        [FunctionName("CosmosFeed")]
        public static async System.Threading.Tasks.Task RunAsync([CosmosDBTrigger(
            databaseName: "socialwikigrpah",
            collectionName: "sample01",
            ConnectionStringSetting = "CosmosDBConnection",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "leases")]IReadOnlyList<Document> documents, TraceWriter log)
        {
            if (documents != null && documents.Count > 0)
            {
                log.Verbose("Documents modified " + documents.Count);
                log.Verbose("First document Id " + documents[0].Id);

                var jsonString = JsonConvert.SerializeObject(documents);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://webhook.site/9d2ae6bf-cd79-4896-b44c-0e60a9e2061a", content);


            }
        }
    }
}
