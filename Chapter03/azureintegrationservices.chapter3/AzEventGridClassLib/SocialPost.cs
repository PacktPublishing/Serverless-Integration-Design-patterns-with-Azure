using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzEventGridClassLib
{
   public class SocialPost
    {
        [JsonProperty(PropertyName = "postedby")]
        public string PostedBy { get; set; }

        [JsonProperty(PropertyName = "postdescription")]
        public string PostDescription { get; set; }

        [JsonProperty(PropertyName = "posttype")]
        public string PostType { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }
    }
}
