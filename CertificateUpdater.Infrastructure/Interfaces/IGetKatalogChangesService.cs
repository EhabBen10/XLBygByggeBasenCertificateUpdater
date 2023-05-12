using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Infrastructure.Interfaces;
public interface IGetKatalogChangesService
{
	public ICollection<CatChange> GetKatalogChanges();
}
