using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Services.Interfaces;
public interface IPostChangesService
{
	public Task PostChangeBatch(List<CertificationChange> changes, CancellationToken cancellationToken);
}
