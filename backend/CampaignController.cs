
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace HealthcareAdsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoogleAdsController : ControllerBase
    {
        private readonly GoogleAdsService _adsService;
        private readonly ILogger<GoogleAdsController> _logger;

        public GoogleAdsController(GoogleAdsService adsService, ILogger<GoogleAdsController> logger)
        {
            _adsService = adsService;
            _logger = logger;
        }

        [HttpPost("create-campaign")]
        public async Task<IActionResult> CreateCampaign()
        {
            var result = await _adsService.CreateDummyCampaignAsync();
            return Ok(new { message = "Campaign created", campaignId = result });
        }
    }
}
