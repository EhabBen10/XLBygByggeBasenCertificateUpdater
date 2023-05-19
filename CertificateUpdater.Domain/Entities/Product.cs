namespace CertificateUpdater.Domain.Entities;
public class Product
{
	public ICollection<Katalog> KatalogData { get; set; } = new List<Katalog>();
}
