using Microsoft.AspNetCore.Mvc;
using UnderdogTakeHome.Data;
using UnderdogTakeHome.Models;

namespace UnderdogTakeHome.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly PlayerService _playerService;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(PlayerService playerService, ILogger<PlayersController> logger)
    {
        _playerService = playerService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> Get(string id)
    {
        var player = await _playerService.GetAsync(id);

        if (player is null)
        {
            return NotFound();
        }

        return player;
    }

    [HttpGet()]
    public async Task<ActionResult<List<Player>>> Get([FromQuery] PlayerSearchQuery query,
    [FromQuery] string sport = null, [FromQuery] string position = null)
    {
        IEnumerable<Player> players = await _playerService.GetAsync();
        if (players is null || !players.Any())
        {
            return NotFound();
        }

        if(query == null) {
            return players.ToList();
        }

        if (!string.IsNullOrWhiteSpace(sport))
        {
            players = players.Where(p => p.Sport == sport);
        }

        if (query.FirstLetterOfLastName != null)
        {
            players = players.Where(p => !string.IsNullOrEmpty(p.LastName) && p.LastName[0] == query.FirstLetterOfLastName);
        }

        if (query.Age != null)
        {
            players = players.Where(p => p.Age == query.Age);
        }

        if (query.MinAge != null)
        {
            players = players.Where(p => p.Age >= query.MinAge);
        }

        if (query.MaxAge != null)
        {
            players = players.Where(p => p.Age <= query.MaxAge);
        }

        if (!string.IsNullOrWhiteSpace(position))
        {
            players = players.Where(p => p.Position == position);
        }

        return players.ToList();
    }

    [HttpGet("insert/{sport}/{id}")]
    public async Task<int> Get(string sport, int id)
    {
        await _playerService.UpdateAllPlayers(sport);
        return (await _playerService.GetAsync()).Count();
    }

    public class PlayerSearchQuery
    {
        public char? FirstLetterOfLastName { get; set; }
        public int? Age { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
    }
}