using Newtonsoft.Json;
using System.Globalization;

namespace SeniorAPI.Helpers
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private const string DateFormat = "dd/MM/yyyy";

        public override DateTime ReadJson(JsonReader reader, Type objectType, DateTime existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string? dateStr = reader.Value?.ToString();
            if (!string.IsNullOrWhiteSpace(dateStr))
            return DateTime.ParseExact(dateStr, DateFormat, CultureInfo.InvariantCulture);

            return DateTime.MinValue;
        }

        public override void WriteJson(JsonWriter writer, DateTime value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString(DateFormat));
        }
    }
}