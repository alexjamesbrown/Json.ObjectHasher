namespace JsonObjectHasher.Newtonsoft.Tests.Models;

public record CompletedEvent
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset TimeCompleted { get; set; }
}