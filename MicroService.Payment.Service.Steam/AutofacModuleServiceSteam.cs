using Autofac;
using MicroService.Payment.Service.Steam.Services;

namespace MicroService.Payment.Service.Steam
{
    public class AutofacModuleServiceSteam : Module
    {
        private readonly string _steamPricingApiKey;

        public AutofacModuleServiceSteam(string steamPricingApiKey)
        {
            _steamPricingApiKey = steamPricingApiKey;
        }

        protected override void Load(ContainerBuilder builder)
        {
            #region Services

            builder.RegisterType<CsgoFastApi>().As<ISteamPricingResourceService>();
            builder.RegisterType<SteamMarketScraperService>().As<ISteamMarketScraperService>();

            builder.Register(c => new CsgoFastApiConfiguration{ApiKey = _steamPricingApiKey}).As<CsgoFastApiConfiguration>();

            #endregion

            #region HTTP

            #endregion

            #region SQL

            #endregion

            #region Mongo

            #endregion
        }
    }
}