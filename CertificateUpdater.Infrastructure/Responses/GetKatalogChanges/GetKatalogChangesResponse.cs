using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetKatalogChanges;

internal sealed record GetKatalogChangesResponse
{
	[JsonPropertyName("ChangeList")]
	public ICollection<CatChangeData> CatChangeData { get; set; } = new List<CatChangeData>();
}
