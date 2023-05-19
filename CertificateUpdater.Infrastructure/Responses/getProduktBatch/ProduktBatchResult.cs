using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.getProduktBatch;
internal sealed record ProduktBatchResult
{
	[JsonPropertyName("Result")]
	public ICollection<ResultData> ResultData { get; set; } = new List<ResultData>();
}
