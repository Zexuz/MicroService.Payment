using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.Payment.Service.Steam.Services
{
    public interface IPricingService
    {
        Task UpdateUpricing(PaymentMethods paymentMethods);
        Task<List<SteamItemPrice>> GetSteamPricingAsync(SteamItemLookup[] itemsToLookup);
    }
    
    public class SteamItemLookup
    {
        public string MarketHashName { get; set; }
        public int    AppId          { get; set; }
    }

    public enum PaymentMethods
    {
        SteamCsgo,
        SteamPugb 
    }

    public enum SteamGames
    {
        Csgo,
        Pubg,
    }
}