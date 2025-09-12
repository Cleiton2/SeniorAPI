using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SeniorAPI.Enumerador;

namespace SeniorAPI.Helpers
{
    public class UFSiglaConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string? ufString = reader?.Value?.ToString();
                if (Enum.TryParse(ufString, true, out UF ufEnum))
                {
                    return ufEnum;
                }
            }
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is UF uf)
            {
                writer.WriteValue(uf.ToString());
            }
            else
            {
                base.WriteJson(writer, value, serializer);
            }
        }
    }
}