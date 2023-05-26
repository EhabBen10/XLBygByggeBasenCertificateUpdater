using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.Common;
internal sealed class Next
{
	[JsonPropertyName("after")]
	public string After { get; set; } = string.Empty;
}
