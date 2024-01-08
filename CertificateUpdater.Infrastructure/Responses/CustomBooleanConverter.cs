using System.Text.Json;
using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses;
public class CustomBooleanConverter : JsonConverter<bool>
{
	public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType == JsonTokenType.String)
		{
			if (bool.TryParse(reader.GetString(), out bool result))
			{
				return result;
			}
		}
		else if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
		{
			return reader.GetBoolean();
		}

		throw new JsonException($"Unable to convert {reader.TokenType} to boolean.");
	}

	public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
	{
		writer.WriteBooleanValue(value);
	}
}
