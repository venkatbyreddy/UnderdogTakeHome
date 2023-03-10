using UnderdogTakeHome.Models;

public class PlayerFactory
{
    public Player Create(SportType sport) => sport switch
    {
        SportType.BaseBall => new BaseballPlayer(),
        SportType.BasketBall => new BasketballPlayer(),
        SportType.Football => new FootballPlayer(),
        _ => throw new NotSupportedException()
    };
}