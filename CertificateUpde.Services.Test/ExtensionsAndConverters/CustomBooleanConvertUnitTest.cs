using System.Text;
using System.Text.Json;
using CertificateUpdater.Services.Responses;
using Xunit;

namespace CertificateUpdater.Services.Test.ExtensionsAndConverters;

public sealed class CustomBooleanConvertUnitTest
{
	private readonly CustomBooleanConverter _uut = new CustomBooleanConverter();

	[Theory]
	[InlineData("true", true)]
	[InlineData("false", false)]
	[InlineData("\"true\"", true)] // Additional test for JsonTokenType.String
	[InlineData("\"false\"", false)] // Additional test for JsonTokenType.String
	public void CanReadValidStringValues(string jsonString, bool expectedValue)
	{
		// Arrange
		var jsonBytes = Encoding.UTF8.GetBytes(jsonString);
		var reader = new Utf8JsonReader(jsonBytes);

		// Act
		reader.Read(); // Move the reader to the start of the stream

		var result = _uut.Read(ref reader, typeof(bool), new JsonSerializerOptions());

		// Assert
		Assert.Equal(expectedValue, result);
	}


	[Theory]
	[InlineData("invalid")]
	[InlineData("123")]
	[InlineData("null")]
	public void ReadInvalidStringValuesThrowsJsonException(string stringValue)
	{
		// Act 
		var result = Record.Exception(() =>
		{
			var readerCopy = new Utf8JsonReader(Encoding.UTF8.GetBytes($"{stringValue}"));
			_uut.Read(ref readerCopy, typeof(bool), new JsonSerializerOptions());
		});

		// Assert
		Assert.IsType<System.Text.Json.JsonException>(result);
	}

	[Theory]
	[InlineData("true", true)]
	[InlineData("false", false)]
	public void CanWriteBooleanValues(string expectedStringValue, bool value)
	{
		// Arrange
		using var stream = new MemoryStream();
		using var writer = new Utf8JsonWriter(stream);
		var options = new JsonSerializerOptions();

		// Act
		_uut.Write(writer, value, options);
		writer.Flush();

		// Assert
		stream.Seek(0, SeekOrigin.Begin);
		using var reader = new StreamReader(stream);
		var resultJson = reader.ReadToEnd();
		Assert.Equal($"{expectedStringValue}", resultJson);
	}

	[Fact]
	public void CanHandleNullToken()
	{
		// Arrange
		var jsonBytes = Encoding.UTF8.GetBytes("null");
		var reader = new Utf8JsonReader(jsonBytes);

		// Act and Assert
		Assert.Throws<JsonException>(() =>
		{
			var readerCopy = new Utf8JsonReader(jsonBytes);
			_uut.Read(ref readerCopy, typeof(bool), new JsonSerializerOptions());
		});
	}
}
