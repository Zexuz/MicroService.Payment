using System.Threading.Tasks;

namespace MicroService.Payment.Service.Steam.Services
{
    public interface ISteamPricingResourceService
    {
        Task UpdatePricingForCsgoAsync();
        Task UpdatePricingForPubgAsync();
    }
}