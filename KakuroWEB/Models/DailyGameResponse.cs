namespace KakuroWEB.Models;

public class DailyGameResponse
{
    public string Id { get; set; }
    public List<int> Row { get; set; }
    public List<int> Column { get; set; }
}