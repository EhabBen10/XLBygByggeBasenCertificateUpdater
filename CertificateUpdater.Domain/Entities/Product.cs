namespace CertificateUpdater.Domain.Entities;
public class Product
{
	public ICollection<Katalog> KatalogData { get; set; } = new List<Katalog>();
	public string CompanyName { get; set; } = string.Empty;
	public int SupplierNr { get; set; }
	public int DBNr { get; set; }
	public string ProductText { get; set; } = string.Empty;
}
