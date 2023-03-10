using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UnderdogTakeHome.Models;

namespace UnderdogTakeHome.Data;

public class PlayerService
{
    private readonly IMongoCollection<Player> _playersCollection;

    private readonly ICbsSportsClient _cbsSportsClient;
    private readonly PlayerFactory _playerFactory;

    public PlayerService(
        IOptions<DatabaseSettings> playerStoreDatabaseSettings, ICbsSportsClient cbsSportsClient, 
        PlayerFactory playerFactory)
    {
        _cbsSportsClient = cbsSportsClient;
        _playerFactory = playerFactory;

        var mongoClient = new MongoClient(
            playerStoreDatabaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(
            playerStoreDatabaseSettings.Value.DatabaseName);

        _playersCollection = mongoDatabase.GetCollection<Player>(
            playerStoreDatabaseSettings.Value.PlayersCollectionName);
    }

    //Ideally, this would be called periodically from a Lambda function or a message publish
    //But for the scope of this interview, this will be called from a GET endpoint
    public async Task UpdateAllPlayers(string sport) 
    {
        if(!Enum.TryParse(sport, true, out SportType sportType)) {
            return;
        }

        var allPlayers = await _cbsSportsClient.GetPlayersAsync(sport);
        if(allPlayers is null) return;

        var avgAgesPerPosition = allPlayers.Where(p => p.Position != null)
                                .GroupBy(p => p.Position)
                                .ToDictionary(group => group.Key, group => group.Average(p => p.Age));
        foreach(var player in allPlayers)
        {
            if(player.Age > 0) {
                avgAgesPerPosition.TryGetValue(player.Position, out double averageAge);
                player.DeviationFromAverageAge = player.Age - averageAge;
            }
            player.Sport = sport;
            player.NameBrief = _playerFactory.Create(sportType).NameBrief;

            await UpsertAsync(player.Id, player);
        }
    }

    public async Task<IEnumerable<Player>> GetAsync() =>
        await _playersCollection.Find(_ => true).ToListAsync();

    public async Task<Player?> GetAsync(string id) =>
        await _playersCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task UpsertAsync(string id, Player updatedPlayer) =>
        await _playersCollection.ReplaceOneAsync(x => x.Id == id, updatedPlayer, new ReplaceOptions()
        {
            IsUpsert = true
        });

    public async Task RemoveAsync(string id) =>
        await _playersCollection.DeleteOneAsync(x => x.Id == id);
}