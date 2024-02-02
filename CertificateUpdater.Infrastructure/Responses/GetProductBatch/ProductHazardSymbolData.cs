using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetProductBatch;
internal sealed record ProductHazardSymbolData
{
	[JsonPropertyName("SymDesc")]
	public string? SymDesc { get; set; }

	[JsonPropertyName("SymImgUrl")]
	public string? SymImgUrl { get; set; }

	[JsonPropertyName("SymName")]
	public string? SymName { get; set; }
}
