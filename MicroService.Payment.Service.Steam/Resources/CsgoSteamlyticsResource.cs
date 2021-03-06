﻿using MicroService.Common.Core.Web;
using Newtonsoft.Json;

namespace MicroService.Payment.Service.Steam.Resources
{
    internal class CsgoSteamlyticsResource : Parser<CsgoSteamlyticsResource>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("num_items")]
        public long NumItems { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }
        
        internal class Item
        {
            [JsonProperty("market_name")]
            public string MarketName { get; set; }

            [JsonProperty("market_hash_name")]
            public string MarketHashName { get; set; }

            [JsonProperty("icon_url")]
            public string IconUrl { get; set; }

            [JsonProperty("name_color")]
            public string NameColor { get; set; }

            [JsonProperty("quality_color")]
            public string QualityColor { get; set; }
        }
    }
}