namespace CertificateUpdater.Domain.Entities;
public sealed record EPD
{
	public string CompanyName { get; set; } = string.Empty;
	public string SupplierNr { get; set; } = string.Empty;
	public int DBNr { get; set; }
	public int ProductGroupId { get; set; }
	public string ProductText { get; set; } = string.Empty;
	public decimal ConversionFactor { get; set; }
	public DateTime CreationDate { get; set; }
	public string? EN15084ACertification { get; set; }
	public int EPDType { get; set; }
	public string? FunctionalUnit { get; set; }
	public int FunctionalUnitAmount { get; set; }
	public bool ISO14025Certified { get; set; }
	public bool ISO14040Certified { get; set; }
	public bool ISO14044Certified { get; set; }
	public DateTime? PdfAppxDate { get; set; }
	public string? PdfAppxName { get; set; }
	public DateTime? PdfDate { get; set; }
	public string? PdfId { get; set; }
	public string? PdfName { get; set; }
	public int ServiceLifeAmount;
	public DateTime? ValidFrom { get; set; }
	public DateTime? ValidTo { get; set; }
	public EPDIndicatorLines? EPDIndicatorLines { get; set; }
	public bool IsDeleted { get; set; }
}
