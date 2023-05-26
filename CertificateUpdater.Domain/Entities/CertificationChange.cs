namespace CertificateUpdater.Domain.Entities;

public class CertificationChange
{
	public string CompanyName { get; set; } = string.Empty;
	public int SupplierNr { get; set; }
	public int DBNr { get; set; }
	public string ProductText { get; set; } = string.Empty;
	public bool hasDGNB { get; set; }
	public bool hasSvanemærke { get; set; }
	public bool hasSvanemærkeByggeri { get; set; }
	public bool hasBREEAM { get; set; }
	public bool hasC2C { get; set; }
	public bool hasDBE { get; set; }
	public bool hasLEED { get; set; }
	public bool hasFSC { get; set; }
	public bool hasPEFC { get; set; }
	public bool hasIndeKlima { get; set; }
	public bool hasEUBlomst { get; set; }
	public bool hasAstmaOgAllergi { get; set; }
	public bool hasEPD { get; set; }
	public bool hasALUK { get; set; }
}
