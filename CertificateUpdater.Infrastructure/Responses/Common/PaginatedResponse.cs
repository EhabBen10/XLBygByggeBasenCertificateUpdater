using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.Common;
internal sealed class PaginatedResponse<TResponse>
{
	[JsonPropertyName("results")]
	public IEnumerable<TResponse> Results { get; set; } = Enumerable.Empty<TResponse>();

	[JsonPropertyName("paging")]
	public Paging Paging { get; set; } = new();
}
