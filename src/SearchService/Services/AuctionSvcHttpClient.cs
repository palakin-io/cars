using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Service;

public class AuctionSvcHttpClient
{
    private readonly HttpClient _client;
    private readonly IConfiguration _config;


    public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration config)
    {
        _client = httpClient;
        _config = config;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, string>().Sort(x => x.Descending(x => x.UpdatedAt)).Project(x => x.UpdatedAt.ToString()).ExecuteFirstAsync();

        // 1. Construct the URL
        string url = _config["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated;

        // 2. Fetch the data from the API as a List<Item>
        List<Item> items = await _client.GetFromJsonAsync<List<Item>>(url);

        return items;
    }
}