using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Berger.Extensions.Repository
{
    public class JsonConverter<T> : ValueConverter<T, string>
    {
        public JsonConverter() : base
        (
            e => JsonConvert.SerializeObject(e),
            e => JsonConvert.DeserializeObject<T>(e)
        )
        { }
    }
    public class StringListConverter<T> : ValueConverter<List<string>, string>
    {
        public StringListConverter() : base(e => string.Join(", ", e!), e => e.Split(',', StringSplitOptions.TrimEntries).ToList())
        { }
    }
}