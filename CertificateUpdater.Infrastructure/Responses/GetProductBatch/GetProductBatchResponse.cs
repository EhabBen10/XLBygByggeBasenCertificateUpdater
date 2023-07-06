using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("CertificateUpdater.Services.Test")]
namespace CertificateUpdater.Services.Responses.GetProductBatch;
internal sealed record GetProductBatchResponse
{
	[JsonPropertyName("getProduktBatchResult")]
	public GetProductBatchResult Result { get; set; } = new();
}
