using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetKatalogChanges;
internal sealed record GetKatalogChangesResponse
{
	[JsonPropertyName("getKatalogChangesResult")]
	public GetKatalogChangesResult Result { get; set; } = new GetKatalogChangesResult();
}
