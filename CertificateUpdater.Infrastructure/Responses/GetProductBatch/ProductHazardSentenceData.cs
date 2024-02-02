using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetProductBatch;
internal sealed record ProductHazardSentenceData
{
	[JsonPropertyName("Ajour_date")]
	public string? AjourDate { get; set; }

	[JsonPropertyName("AjourID")]
	public int? AjourId { get; set; }

	[JsonPropertyName("Ajour_user")]
	public string? AjourUser { get; set; }

	[JsonPropertyName("Sentence")]
	public string? Sentence { get; set; }

	[JsonPropertyName("SentenceCode")]
	public string? SentenceCode { get; set; }
}
