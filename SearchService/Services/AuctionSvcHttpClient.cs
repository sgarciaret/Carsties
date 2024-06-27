using MongoDB.Entities;

namespace SearchService.Services
{
    public class AuctionSvcHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<Models.Item>> GetItemsSearchDb()
        {
            var lastUpdated = await DB.Find<Models.Item, string>()
                .Sort(x => x.Descending(x => x.UpdatedAt))
                .Project(x => x.UpdatedAt.ToString())
                .ExecuteFirstAsync();

            if (string.IsNullOrEmpty(lastUpdated))
            {
                lastUpdated = DateTime.MinValue.ToString();
            }

            return await _httpClient.GetFromJsonAsync<List<Models.Item>>(_config["AuctionServiceUrl"] + "/api/auctions?date=" + DateTime.MinValue.ToString());
        }
    }
}
