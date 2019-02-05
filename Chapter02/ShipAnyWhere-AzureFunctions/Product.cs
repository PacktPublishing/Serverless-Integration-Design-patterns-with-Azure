

namespace ShipAnyWhere
{   
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class Product
    {
        [JsonProperty("id")]       
        public string Productid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("avialableQuantity")]
        public long AvailableQuantity { get; set; }
    }
    
}
