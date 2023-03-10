using UnderdogTakeHome.Models;

public interface ICbsSportsClient
{
    Task<IEnumerable<Player>?> GetPlayersAsync(string sport);
}

public class CbsSportsClient : ICbsSportsClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CbsSportsClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<Player>?> GetPlayersAsync(string sport)
    {
        var httpClient = _httpClientFactory.CreateClient("Cbs");
        var response = await httpClient.GetFromJsonAsync<CbsPlayerResponse>(
            $"fantasy/players/list?version=3.0&SPORT={sport}&response_format=JSON");

        return response?.Body.Players;
    }
}