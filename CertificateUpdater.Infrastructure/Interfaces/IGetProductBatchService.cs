using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Services.Interfaces;
public interface IGetProductBatchService
{
	public Task<ICollection<Product>> GetProducts(CancellationToken cancellationToken);

}
