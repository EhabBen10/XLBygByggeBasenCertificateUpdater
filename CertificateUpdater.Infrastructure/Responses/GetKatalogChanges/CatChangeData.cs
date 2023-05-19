using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetKatalogChanges;

internal sealed record CatChangeData
{
	[JsonPropertyName("EmneId")]
	public int EmneId { get; set; }

	[JsonPropertyName("Tunnr")]
	public int Tunnr { get; set; }
}
