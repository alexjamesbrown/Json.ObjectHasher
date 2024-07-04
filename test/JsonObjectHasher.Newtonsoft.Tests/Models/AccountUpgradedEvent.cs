namespace JsonObjectHasher.Newtonsoft.Tests.Models;

public record AccountUpgradedEvent
{
    public int Id { get; set; }
    public string UpgradeLevel { get; set; }
    public DateTimeOffset DateOfUpgrade { get; set; }
    public Account Account { get; set; }
}