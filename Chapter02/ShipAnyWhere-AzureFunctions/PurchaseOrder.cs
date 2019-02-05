
namespace ShipAnyWhere
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    public  class PurchaseOrder
    {
        [JsonProperty("ponumber")]     
        public string Ponumber { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sku")]
       
        public string Sku { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("messagetype")]
        public string Messagetype { get; set; }

         [JsonProperty("status")]
         public string Status { get; set; }

          [JsonProperty("shipTo")]
        public To ShipTo { get; set; }

        [JsonProperty("billTo")]
        public To BillTo { get; set; }

    }
    
    public partial class To
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("zip")]        
        public string Zip { get; set; }
    }

}
