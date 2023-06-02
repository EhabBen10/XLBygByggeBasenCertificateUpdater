using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Services.Interfaces;
public interface IPostChangesService
{
	public Task PostChangeBatch(ICollection<CertificationChange> changes, CancellationToken cancellationToken);
}
