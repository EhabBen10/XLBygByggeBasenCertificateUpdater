using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetKatalogChanges;
internal sealed record GetKatalogChangesResult
{
	[JsonPropertyName("ChangeList")]
	public ICollection<CatChangeData> CatChangesData { get; set; } = new List<CatChangeData>();
}
