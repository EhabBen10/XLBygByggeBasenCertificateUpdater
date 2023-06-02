using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Services.Interfaces;
public interface IGetProductBatchService
{
	public Task<ICollection<Product>> GetProductBatch(ICollection<int> tunnrs, CancellationToken cancellationToken);
}
