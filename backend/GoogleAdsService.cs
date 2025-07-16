
using Google.Ads.GoogleAds;
using Google.Ads.GoogleAds.Lib;
using Google.Ads.GoogleAds.V16.Services;
using Google.Ads.GoogleAds.V16.Resources;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

public class GoogleAdsService
{
    private readonly GoogleAdsClient _client;
    private readonly IConfiguration _config;

    public GoogleAdsService(IConfiguration config)
    {
        _config = config;
        var googleConfig = new GoogleAdsConfig
        {
            DeveloperToken = config["GoogleAds:DeveloperToken"],
            ClientId = config["GoogleAds:ClientId"],
            ClientSecret = config["GoogleAds:ClientSecret"],
            RefreshToken = config["GoogleAds:RefreshToken"],
            LoginCustomerId = long.Parse(config["GoogleAds:LoginCustomerId"])
        };
        _client = new GoogleAdsClient(googleConfig);
    }

    public async Task<long> CreateDummyCampaignAsync()
    {
        var campaignService = _client.GetService(Services.V16.CampaignService);
        var customerId = _config["GoogleAds:CustomerId"];

        var campaign = new Campaign
        {
            Name = "Test Campaign " + System.Guid.NewGuid(),
            AdvertisingChannelType = AdvertisingChannelType.Search,
            Status = CampaignStatus.Paused,
            ManualCpc = new ManualCpc(),
            CampaignBudget = "customers/" + customerId + "/campaignBudgets/1234567890", // Replace with real budget resource
            NetworkSettings = new NetworkSettings
            {
                TargetGoogleSearch = true,
                TargetSearchNetwork = true,
                TargetContentNetwork = false
            }
        };

        var operation = new CampaignOperation { Create = campaign };
        var response = await campaignService.MutateCampaignsAsync(customerId, new[] { operation });

        return response.Results[0].ResourceName.Split('/')[1] is string idStr && long.TryParse(idStr, out long id) ? id : 0;
    }
}
