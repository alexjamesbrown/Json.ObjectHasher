namespace JsonObjectHasher.SystemTextJson.Tests.Models;

public record DeclinedEvent
{
    public int Id { get; set; }
    public string Reason { get; set; }
    public DateTimeOffset DateDeclined { get; set; }
}
