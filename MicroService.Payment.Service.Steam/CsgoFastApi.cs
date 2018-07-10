using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MicroService.Common.Core.Web;
using MicroService.Payment.Service.Steam.Helpers;
using MicroService.Payment.Service.Steam.Resources;
using MicroService.Payment.Service.Steam.Services;

namespace MicroService.Payment.Service.Steam
{
    public class CsgoFastApiConfiguration
    {
        public string ApiKey { get; set; }
    }

    internal class CsgoFastApi : ISteamPricingResourceService
    {
        private readonly ISteamMarketScraperService _steamMarketScraperService;
        private readonly IHttpRequestParser         _httpRequestParser;
        private readonly IMongoPricingRepository    _pricingRepository;
        private readonly CsgoFastApiConfiguration   _configuration;
        private readonly ILogger<CsgoFastApi>       _logger;

        public CsgoFastApi
        (
            CsgoFastApiConfiguration configuration,
            ISteamMarketScraperService steamMarketScraperService,
            IHttpRequestParser httpRequestParser,
            IMongoPricingRepository pricingRepository,
            ILogger<CsgoFastApi> logger
        )
        {
            _configuration = configuration;
            _steamMarketScraperService = steamMarketScraperService;
            _httpRequestParser = httpRequestParser;
            _pricingRepository = pricingRepository;
            _logger = logger;
        }

        public async Task UpdatePricingForCsgoAsync()
        {
            await UpdatePriceing("https://api.csgofast.com/price/all", SteamPaymentMethodToAppId.ToInt(SteamGames.Csgo));
            await GetNameAndImagesForCsgo();
        }

        public async Task UpdatePricingForPubgAsync()
        {
            await UpdatePriceing("https://api.dapubg.com/price/all", SteamPaymentMethodToAppId.ToInt(SteamGames.Pubg));
            await GetNameAndImagesForPubg();
        }

        private async Task UpdatePriceing(string url, int appId)
        {
            _logger.Info($"Starting price update for {SteamPaymentMethodToAppId.ToEnum(appId).ToString()}");
            var jObject = await _httpRequestParser.ExecuteAsJObject(new RequestMessage(HttpMethod.Get, url));

            var steamPrices = new SteamPricing
            {
                Id = SteamPaymentMethodToAppId.ToEnum(appId),
                Items = new List<SteamItemPrice>(),
                LastUpdated = DateTimeOffset.Now
            };

            using (var cureser = jObject.GetEnumerator())
            {
                while (cureser.MoveNext())
                {
                    var name = cureser.Current.Key.ToString();
                    var price = decimal.Parse(cureser.Current.Value.ToString());

                    steamPrices.Items.Add(new SteamItemPrice
                    {
                        Value = Math.Round(price, 2),
                        MarketHashName = name,
                        ContextId = 2,
                        LastUpdated = DateTimeOffset.Now,
                        BackgroundColor = null,
                        IconUrl = null,
                        NameColor = null,
                    });
                }

                await _pricingRepository.InsertOrUpdateAsync(steamPrices);
                _logger.Info($"Pricing for {SteamPaymentMethodToAppId.ToEnum(appId).ToString()} done");
            }
        }

        private async Task GetNameAndImagesForCsgo()
        {
            _logger.Info($"Starting image fetchin for CSGO");
            var request = new RequestMessage(HttpMethod.Get, $"http://api.csgo.steamlytics.xyz/v1/items?key={_configuration.ApiKey}");
            var imageResponse = await _httpRequestParser.ExecuteRawAsync(request);

            var response = CsgoSteamlyticsResource.FromJson(imageResponse);
            var listCount = response.NumItems;
            var list = response.Items;
            var regex = new Regex("\\/\\/steamcommunity-a.akamaihd.net\\/economy\\/image\\/(.+)");

            var csgoPricing = await _pricingRepository.GetAllAsync(SteamGames.Csgo);

            var regExService = new RegexService();
            for (var index = 0; index < listCount; index++)
            {
                var item = list[index];
                var name = item.MarketHashName;
                if (string.IsNullOrEmpty(item.IconUrl)) continue;

                string regExMatch;
                try
                {
                    regExMatch = regExService.GetFirstGroupMatch(regex, item.IconUrl);
                }
                catch (Exception e)
                {
                    e.Data.Add("name", name);
                    throw;
                }

                var imgUrl = regExMatch;
                foreach (var csgoItem in csgoPricing.Items)
                {
                    if (csgoItem.MarketHashName != name) continue;
                    csgoItem.IconUrl = imgUrl;
                    break;
                }
            }

            await _pricingRepository.InsertOrUpdateAsync(csgoPricing);
            _logger.Info($"Images for CSGO done");

        }

        private async Task GetNameAndImagesForPubg()
        {
            _logger.Info($"Starting image fetchin for PUBG");

            var nameAndImages = await _steamMarketScraperService.Scrape(SteamPaymentMethodToAppId.ToInt(SteamGames.Pubg));

            var regex = new Regex("/economy\\/image\\/([^/]*)");

            var pubgPricing = await _pricingRepository.GetAllAsync(SteamGames.Pubg);

            var regExService = new RegexService();
            for (var index = 0; index < nameAndImages.Count; index++)
            {
                var item = nameAndImages[index];
                var name = item.Name;
                if (string.IsNullOrEmpty(item.ImageSrc)) continue;

                string regExMatch;
                try
                {
                    regExMatch = regExService.GetFirstGroupMatch(regex, item.ImageSrc);
                }
                catch (Exception e)
                {
                    e.Data.Add("name", name);
                    throw;
                }

                var imgUrl = regExMatch;

                foreach (var pubgItem in pubgPricing.Items)
                {
                    if (pubgItem.MarketHashName != name) continue;
                    pubgItem.IconUrl = imgUrl;
                    break;
                }
            }

            await _pricingRepository.InsertOrUpdateAsync(pubgPricing);
            _logger.Info($"Images for PUBG done");
        }
    }
}