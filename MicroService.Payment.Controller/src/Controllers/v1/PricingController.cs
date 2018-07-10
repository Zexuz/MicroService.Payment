using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MicroService.Payment.Service.Steam;
using MicroService.Payment.Service.Steam.Services;

namespace MicroService.Payment.Controller.Controllers.v1
{
    [Route("api/v1/[controller]")]
    public class PricingController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly ILogger<PricingController> _logger;
        private readonly IPricingService            _pricingService;

        public PricingController(ILogger<PricingController> logger, IPricingService pricingService)
        {
            _logger = logger;
            _pricingService = pricingService;
        }


        [HttpGet("steam/updatepricing/")]
        public async Task<IActionResult> Test()
        {
            await _pricingService.UpdateUpricing(PaymentMethods.SteamCsgo);
            await _pricingService.UpdateUpricing(PaymentMethods.SteamPugb);
            return Ok();
        }

        [HttpPost("steam/")]
        public async Task<IActionResult> SteamPricing([FromBody] SteamItemLookup[] items)
        {
            var res = await _pricingService.GetSteamPricingAsync(items);
            return Ok();
        }
    }
}