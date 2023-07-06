namespace CertificateUpdater.Domain.Entities;
public sealed record Product
{
	public ICollection<Katalog> KatalogData { get; set; } = new List<Katalog>();
	public ICollection<DGNBDocument> dGNBDocuments { get; set; } = new List<DGNBDocument>();
	public string CompanyName { get; set; } = string.Empty;
	public string SupplierNr { get; set; } = string.Empty;
	public int DBNr { get; set; }
	public string ProductText { get; set; } = string.Empty;
}
