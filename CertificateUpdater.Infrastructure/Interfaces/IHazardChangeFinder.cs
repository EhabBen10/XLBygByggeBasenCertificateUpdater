using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Services.Interfaces;
public interface IHazardChangeFinder
{
	public ICollection<HazardInfo> FindHazardChanges(ICollection<Product> products);

}
