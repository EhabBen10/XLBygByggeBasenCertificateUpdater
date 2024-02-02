using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace CertificateUpdater.Services.Responses.GetProductChanges;
internal sealed record GetProductChangesResponse
{
	[JsonPropertyName("getChangedProduktResult")]
	public GetProductChangesResult Result { get; set; } = new();
}
