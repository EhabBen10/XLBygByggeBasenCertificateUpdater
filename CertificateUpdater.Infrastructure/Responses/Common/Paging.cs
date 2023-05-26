using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.Common;
internal sealed class Paging
{
	[JsonPropertyName("next")]
	public Next Next { get; set; } = new();
}
