using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetProductBatch;
internal sealed record GetProductBatchResult
{
	[JsonPropertyName("Result")]
	public ICollection<ResultData> ResultData { get; set; } = new List<ResultData>();
}
