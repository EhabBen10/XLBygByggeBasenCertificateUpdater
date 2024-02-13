namespace CertificateUpdater.Domain.Entities;
public sealed record HazardInfo
{
	public string CompanyName { get; set; } = string.Empty;
	public string SupplierNr { get; set; } = string.Empty;
	public int DBNr { get; set; }
	public string ProductText { get; set; } = string.Empty;
	public int ProductGroupId { get; set; }
	public string? HazardClass { get; set; }
	public string? HazardMark { get; set; }
	public bool? HasHazardousGoods { get; set; }
	public string? UNCode { get; set; }
	public string? ShippingDesignation { get; set; }
	public List<ProductHazardSentence> ProductHazardSentences { get; set; } = new List<ProductHazardSentence>();
	public List<ProductSafetySentence> ProductSafetySentences { get; set; } = new List<ProductSafetySentence>();
	public List<ProductHazardSymbol> ProductHazardSymbols { get; set; } = new List<ProductHazardSymbol>();
}

