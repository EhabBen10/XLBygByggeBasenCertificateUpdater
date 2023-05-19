using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Services.Interfaces;
public interface IGetKatalogChangesService
{
	public Task<ICollection<CatChange>> GetKatalogChanges(CancellationToken cancellationToken);
}
