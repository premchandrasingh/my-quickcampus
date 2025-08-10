using System.Text.Json;
using System.Text.Json.Serialization;

namespace My.QuickCampus
{
    public class JsonDateConverter : JsonConverter<DateTime>
    {
        private readonly string _format;

        public JsonDateConverter(string format)
        {
            _format = format;
        }

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateStr = reader.GetString();
            //DateTime.ParseExact(dateStr, _format, null);
            var isSuccess = DateTime.TryParseExact(dateStr, _format, null, System.Globalization.DateTimeStyles.None, out var date);
            if (isSuccess)
                return date;

            var _formatTemp = _format + " HH:mm tt";
            isSuccess = DateTime.TryParseExact(dateStr, _formatTemp, null, System.Globalization.DateTimeStyles.None, out date);
            if (isSuccess)
                return date;

            _formatTemp = "yyyy-MM-dd HH:mm:ss.ffffff";
            isSuccess = DateTime.TryParseExact(dateStr, _formatTemp, null, System.Globalization.DateTimeStyles.None, out date);
            if (isSuccess)
                return date;

            _formatTemp = "yyyy-MM-ddTHH:mm:ssZ"; // Fallback format
            isSuccess = DateTime.TryParseExact(dateStr, _format, null, System.Globalization.DateTimeStyles.None, out date);
            if (isSuccess)
                return date;

            throw new JsonException($"JsonDateConverter - Unable to convert \"{dateStr}\" to DateTime using format \"{_format}\".");
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(dateTimeValue.ToString(_format));
        }
    }
}
