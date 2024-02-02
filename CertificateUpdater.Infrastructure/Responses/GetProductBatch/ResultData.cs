using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetProductBatch;

internal sealed record ResultData
{
	[JsonPropertyName("KatalogData")]
	public ICollection<KatalogData> KatalogData { get; set; } = new List<KatalogData>();

	[JsonPropertyName("DGNBDocuments")]
	public ICollection<DGNBDocumentData> DGNBDocuments { get; set; } = new List<DGNBDocumentData>();

	[JsonPropertyName("EPDs")]
	public ICollection<EPDData>? EPDDatas { get; set; }

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

	[JsonPropertyName("HasHazardousGoods")]
	public bool? HasHazardousGoods { get; set; }

	[JsonPropertyName("HazardClass")]
	public string? HazardClass { get; set; }

	[JsonPropertyName("HazardMark")]
	public string? HazardMark { get; set; }

	[JsonPropertyName("Product_HazardSentences")]
	public ICollection<ProductHazardSentenceData>? ProductHazardSentencesData { get; set; }

	[JsonPropertyName("Product_HazardSymbols")]
	public ICollection<ProductHazardSymbolData>? ProductHazardSymbolsData { get; set; }

	[JsonPropertyName("Product_SafetySentences")]
	public ICollection<ProductSafetySentenceData>? ProductSafetySentencesData { get; set; }

	[JsonPropertyName("ShippingDesignation")]
	public string? ShippingDesignation { get; set; }

	[JsonPropertyName("UNCode")]
	public string? UNCode { get; set; }
}
