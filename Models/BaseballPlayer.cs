namespace UnderdogTakeHome.Models;

public class BaseballPlayer : Player
{
    public override string NameBrief => $"{FirstName?.FirstOrDefault()}. {LastName.FirstOrDefault()}.";
}