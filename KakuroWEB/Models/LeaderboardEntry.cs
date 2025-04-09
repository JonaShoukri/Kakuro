using System.Text.Json.Serialization;

namespace KakuroWEB.Models;

public class LeaderboardEntry
{
    [JsonPropertyName("rank")]
    public int Rank { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}

public class LeaderboardModel
{
    [JsonPropertyName("list")]
    public List<LeaderboardEntry> List { get; set; } = new List<LeaderboardEntry>();
}
