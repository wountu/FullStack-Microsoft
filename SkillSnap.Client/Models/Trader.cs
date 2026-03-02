namespace SkillSnap.Client.Models;

public class Trader
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageLink { get; set; } = string.Empty;
}

public class TraderData
{
    public List<Trader> Traders { get; set; } 
}

public class TraderResponse
{
    public TraderData? Data { get; set; }
}