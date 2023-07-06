using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;

namespace CertificateUpdater.Services.Interfaces;
public interface IGetProductBatchService
{
	public Task<Result<ICollection<Product>>> GetProductBatch(ICollection<int> tunnrs, CancellationToken cancellationToken);
}
