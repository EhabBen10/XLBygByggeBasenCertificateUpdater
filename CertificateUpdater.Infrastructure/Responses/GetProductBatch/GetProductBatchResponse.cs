using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetProductBatch;
internal sealed record GetProductBatchResponse
{
	[JsonPropertyName("getProduktBatchResult")]
	public GetProductBatchResult Result { get; set; } = new();
}
