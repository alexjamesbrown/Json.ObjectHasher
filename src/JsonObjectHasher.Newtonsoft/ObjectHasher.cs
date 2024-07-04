using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace JsonObjectHasher.Newtonsoft;

public class ObjectHasher
{
    private readonly JsonSerializerSettings _settings;

    public ObjectHasher(JsonSerializerSettings settings)
    {
        _settings = new JsonSerializerSettings(settings)
        {
            ContractResolver = new HashIgnoreContractResolver()
        };
        // var converter = new HashIgnoreJsonConverter();
        // _settings.Converters.Add(converter);
    }

    public ObjectHasher() : this(new JsonSerializerSettings())
    {
    }

    public string GenerateHash(params object[] values)
    {
        if (values == null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        var textValue = JsonConvert
            .SerializeObject(values, _settings);

        using var hasher = MD5.Create();

        var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(textValue));

        return string.Concat(hash.Select(a => a.ToString("X2")));
    }
}