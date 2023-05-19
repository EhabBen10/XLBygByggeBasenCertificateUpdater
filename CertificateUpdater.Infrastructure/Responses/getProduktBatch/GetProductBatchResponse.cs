using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.getProduktBatch;
internal sealed record getProduktBatchResponse
{
	[JsonPropertyName("Result")]
	public ICollection<ProduktBatchResultData> ResultData { get; set; } = new List<ProduktBatchResultData>();
}
