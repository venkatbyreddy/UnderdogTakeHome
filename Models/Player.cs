using System.Text.Json.Serialization;

namespace UnderdogTakeHome.Models;

public class Player
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }

    [JsonPropertyName("position")]
    public string Position { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("average_position_age_diff")]
    public double DeviationFromAverageAge { get; set; }

    [JsonPropertyName("sport")]
    public string Sport { get; set; }

    [JsonPropertyName("name_brief")]
    public virtual string NameBrief { get; set; }
}