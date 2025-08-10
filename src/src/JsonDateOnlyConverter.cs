using System.Text.Json;
using System.Text.Json.Serialization;

namespace My.QuickCampus
{
    public class JsonDateOnlyConverter : JsonConverter<DateOnly>
    {
        private readonly string _format;

        public JsonDateOnlyConverter(string format)
        {
            _format = format;
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var dateStr = reader.GetString();
            //DateTime.ParseExact(dateStr, _format, null);
            var isSuccess = DateOnly.TryParseExact(dateStr, _format, null, System.Globalization.DateTimeStyles.None, out var date);
            if (isSuccess)
                return date;

            var _formatTemp = "yyyy-MM-dd";
            isSuccess = DateOnly.TryParseExact(dateStr, _formatTemp, null, System.Globalization.DateTimeStyles.None, out date);
            if (isSuccess)
                return date;

            _formatTemp = "MM-dd-yyyy";
            isSuccess = DateOnly.TryParseExact(dateStr, _formatTemp, null, System.Globalization.DateTimeStyles.None, out date);
            if (isSuccess)
                return date;

            throw new JsonException($"JsonDateOnlyConverter - Unable to convert \"{dateStr}\" to DateTime using format \"{_format}\".");
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateOnly dateTimeValue,
            JsonSerializerOptions options)
        {
            writer.WriteStringValue(dateTimeValue.ToString(_format));
        }
    }
}
