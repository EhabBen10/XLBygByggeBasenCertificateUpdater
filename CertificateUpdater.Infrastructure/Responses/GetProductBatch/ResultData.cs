using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetProductBatch;

internal sealed record ResultData
{
	[JsonPropertyName("KatalogData")]
	public ICollection<KatalogData> KatalogData { get; set; } = new List<KatalogData>();

	[JsonPropertyName("DGNBDocuments")]
	public ICollection<DGNBDocumentData> DGNBDocuments { get; set; } = new List<DGNBDocumentData>();

	[JsonPropertyName("LEVERANDORNAVN")]
	public string CompanyName { get; set; } = string.Empty;

	[JsonPropertyName("LEVERANDOERNR")]
	public string SupplierNr { get; set; } = string.Empty;

	[JsonPropertyName("DBNR")]
	public int DBNr { get; set; }

	[JsonPropertyName("VARETEKST1")]
	public string ProductText1 { get; set; } = string.Empty;

	[JsonPropertyName("VAREGRUPPEID")]
	public int ProductGroupId { get; set; }
}
