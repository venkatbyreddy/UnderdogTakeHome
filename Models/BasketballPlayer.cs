namespace UnderdogTakeHome.Models;

public class BasketballPlayer : Player
{
    public override string NameBrief => $"{FirstName} {LastName.FirstOrDefault()}.";
}