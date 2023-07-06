using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;

namespace CertificateUpdater.Services.Interfaces;
public interface IGetKatalogChangesService
{
	public Task<Result<ICollection<CatChange>>> GetKatalogChanges(CancellationToken cancellationToken);
}
