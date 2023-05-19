using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Services.Interfaces;
public interface ICertificationChangeFinder
{
	public ICollection<CertificationChange> FindCertificationChanges(ICollection<Product> products);
}
