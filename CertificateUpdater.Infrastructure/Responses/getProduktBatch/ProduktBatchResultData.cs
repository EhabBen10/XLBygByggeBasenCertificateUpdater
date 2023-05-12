using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.getProduktBatch;
internal sealed record ProduktBatchResultData
{
	[JsonPropertyName("KatalogData")]
	public ICollection<KatalogData> KatalogData { get; set; } = new List<KatalogData>();
}
