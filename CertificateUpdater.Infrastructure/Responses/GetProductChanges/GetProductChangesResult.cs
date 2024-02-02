using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace CertificateUpdater.Services.Responses.GetProductChanges;
public sealed class GetProductChangesResult
{
	[JsonPropertyName("Result")]
	public ICollection<int> Result { get; set; } = new List<int>();
}
