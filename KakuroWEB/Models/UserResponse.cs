namespace KakuroWEB.Models;

public class UserResponse
{
    public IdInfo Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class IdInfo
{
    public long Timestamp { get; set; }
    public DateTime CreationTime { get; set; }
}