using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroService.Payment.Service.Steam.Helpers;

namespace MicroService.Payment.Service.Steam.Services
{
    public class PricingService : IPricingService
    {
        private readonly ISteamPricingResourceService _steamPricingResourceService;
        private readonly IMongoPricingRepository      _pricingRepository;

        public PricingService(ISteamPricingResourceService steamPricingResourceService, IMongoPricingRepository pricingRepository)
        {
            _steamPricingResourceService = steamPricingResourceService;
            _pricingRepository = pricingRepository;
        }

        public async Task UpdateUpricing(PaymentMethods paymentMethods)
        {
            switch (paymentMethods)
            {
                case PaymentMethods.SteamCsgo:
                    await _steamPricingResourceService.UpdatePricingForCsgoAsync();
                    break;
                case PaymentMethods.SteamPugb:
                    await _steamPricingResourceService.UpdatePricingForPubgAsync();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(paymentMethods), paymentMethods, null);
            }
        }

        public async Task<List<SteamItemPrice>> GetSteamPricingAsync(SteamItemLookup[] itemsToLookup)
        {
            var gameGroups = itemsToLookup.GroupBy(lookup => lookup.AppId);

            var itemsToReturn = new List<SteamItemPrice>();
            foreach (var gameGroup in gameGroups)
            {
                var items = await _pricingRepository.GetAllAsync(SteamPaymentMethodToAppId.ToEnum(gameGroup.Key));

                foreach (var steamItemLookup in itemsToLookup)
                {
                    var result = items.Items.FirstOrDefault(price => price.MarketHashName == steamItemLookup.MarketHashName);
                    if(result == null)
                        throw new Exception();
                    
                    itemsToReturn.Add(result);
                }
            }

            return itemsToReturn;
        }
    }
}