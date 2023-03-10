namespace UnderdogTakeHome.Models;

public class FootballPlayer : Player
{
    public override string NameBrief => $"{FirstName?.FirstOrDefault()}. {LastName}";
}