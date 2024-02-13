using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Services.Interfaces;
public interface IEPDChangeFinder
{
	public ICollection<EPD> FindEPDChanges(ICollection<Product> products);
}
