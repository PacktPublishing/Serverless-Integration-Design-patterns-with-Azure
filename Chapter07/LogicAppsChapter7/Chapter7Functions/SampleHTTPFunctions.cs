using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace Chapter7Functions
{
    public static class SampleHTTPFunctions
    {
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var response = await req.Content.ReadAsStringAsync();
            Customer obj = JsonConvert.DeserializeObject<Customer>(response);
            if (obj != null)
            {
                HttpResponseMessage custresponse = new HttpResponseMessage();
                custresponse.Content = new StringContent(string.Concat("Hello", obj.name,
                    "We have send you email at", obj.emailaddress), Encoding.UTF8, "application/json");
                custresponse.StatusCode = HttpStatusCode.OK;
                return custresponse;

            }

                HttpResponseMessage failureresponse = new HttpResponseMessage();
                failureresponse.Content = new StringContent(string.Concat("Hello we have a failed invoke")
                 , Encoding.UTF8, "application/json");
                failureresponse.StatusCode = HttpStatusCode.ExpectationFailed;
            return failureresponse;
        }
    }

    public class Customer
    {

        public string name { get; set; }
        public int Id { get; set; }
        public string emailaddress { get; set; }
        public string country { get; set; }


    }
}
