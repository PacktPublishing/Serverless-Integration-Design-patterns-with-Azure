using Newtonsoft.Json;
using System;

namespace AzEventGridClassLib
{
    public class Productevent
    {
        [JsonProperty(PropertyName = "productname")]
        public string Productname { get; set; }
    }
}
