namespace CertificateUpdater.Domain.Entities;

public sealed record CertificationChange
{
	public string CompanyName { get; set; } = string.Empty;
	public string SupplierNr { get; set; } = string.Empty;
	public int DBNr { get; set; }
	public int ProductGroupId { get; set; }
	public string ProductText { get; set; } = string.Empty;
	public List<string> DGNBQualityStep { get; set; } = new();
	public bool hasSvanemærke { get; set; }
	public bool hasSvanemærkeByggeri { get; set; }
	public bool hasBREEAM { get; set; }
	public bool hasC2C { get; set; }
	public bool hasDBE { get; set; }
	public bool hasLEED { get; set; }
	public bool hasFSC { get; set; }
	public bool hasPEFC { get; set; }
	public bool hasIndeKlima { get; set; }
	public bool hasAstmaOgAllergi { get; set; }
	public bool hasEPD { get; set; }
	public bool hasALUK { get; set; }
}
