using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.Payment.Service.Steam.Services
{
    internal interface ISteamMarketScraperService
    {
        int                         PageSize { get; set; }
        Task<List<SteamMarketItem>> Scrape(int appId);
    }
}