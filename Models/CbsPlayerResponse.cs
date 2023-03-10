using UnderdogTakeHome.Models;

namespace UnderdogTakeHome.Models;

public class CbsPlayerResponse
{
    public CbsPlayerBody Body { get; set; }

    public int StatusCode { get; set; }
}

public class CbsPlayerBody {
    public List<Player> Players {get;set;}
}