using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace CertificateUpdater.Services.Responses.GetKatalogChanges;
internal sealed record GetKatalogChangesResponse
{
	[JsonPropertyName("getKatalogChangesResult")]
	public GetKatalogChangesResult Result { get; set; } = new();
}
