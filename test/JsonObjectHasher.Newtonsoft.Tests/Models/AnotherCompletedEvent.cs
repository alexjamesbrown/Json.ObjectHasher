namespace JsonObjectHasher.Newtonsoft.Tests.Models;

public record AnotherCompletedEvent
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OtherProperty { get; set; }
    public DateTimeOffset Time { get; set; }
}