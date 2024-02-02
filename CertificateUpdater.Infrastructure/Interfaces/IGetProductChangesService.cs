using CertificateUpdater.Domain.Shared;

namespace CertificateUpdater.Services.Interfaces;
public interface IGetProductChangesService
{
	public Task<Result<ICollection<int>>> GetProductChanges(CancellationToken cancellationToken);
}
