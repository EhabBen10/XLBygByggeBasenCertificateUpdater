using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetProductBatch;

internal sealed record KatalogData
{
	[JsonPropertyName("EmneID")]
	public int EmneId { get; set; }

	[JsonPropertyName("Gyldig")]
	public bool isValid { get; set; }

	[JsonPropertyName("TUNNR")]
	public int Tunnr { get; set; }
}
