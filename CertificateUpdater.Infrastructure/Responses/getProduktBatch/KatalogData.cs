using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.getProduktBatch;
interface sealed record KatalogData
{
	[JsonPropertyName("EmneID")]
	public int EmneId { get; set; }

	[JsonPropertyName("Gyldig")]
	public bool Valid { get; set; }

	[JsonPropertyName("TUNNR")]
	public int Tunnr { get; set; }
}
