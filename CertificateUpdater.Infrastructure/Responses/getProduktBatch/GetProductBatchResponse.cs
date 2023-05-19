using System.Text.Json.Serialization;
using CertificateUpdater.Services.Responses.GetKatalogChanges;

namespace CertificateUpdater.Services.Responses.getProduktBatch;
internal sealed record GetProductBatchResponse
{
	[JsonPropertyName("getProduktBatchResult")]
	public GetProduktBatchResult Result { get; set; } = new GetProduktBatchResult();
}
