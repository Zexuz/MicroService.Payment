using MicroService.Common.Core.Web;
using Newtonsoft.Json;

namespace MicroService.Payment.Service.Steam.Resources
{
    internal class SteamMarketResource:Parser<SteamMarketResource>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("start")]
        public long Start { get; set; }

        [JsonProperty("pagesize")]
        public long Pagesize { get; set; }

        [JsonProperty("total_count")]
        public long TotalCount { get; set; }

        [JsonProperty("results_html")]
        public string Results { get; set; }
    }
}