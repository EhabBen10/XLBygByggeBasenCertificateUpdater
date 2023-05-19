using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetProductBatch;

internal sealed record ResultData
{
	[JsonPropertyName("KatalogData")]
	public ICollection<KatalogData> KatalogData { get; set; } = new List<KatalogData>();

	[JsonPropertyName("LEVERANDORNAVN")]
	public string CompanyName { get; set; } = string.Empty;

	[JsonPropertyName("LEVERANDOERNR")]
	public int SupplierNr { get; set; }

	[JsonPropertyName("DBNR")]
	public int DBNr { get; set; }

	[JsonPropertyName("VARETEKST1")]
	public string ProductText1 { get; set; } = string.Empty;

	[JsonPropertyName("VARETEKST2")]
	public string ProductText2 { get; set; } = string.Empty;

}
