using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;

namespace CertificateUpdater.Services.Interfaces;
public interface IPostChangesService
{
	public Task<Result> PostChangeBatch(ICollection<CertificationChange> changes, CancellationToken cancellationToken);
}
