using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Services.Interfaces;
public interface IGetProductsService
{
	public Task<ICollection<Product>> GetProducts(CancellationToken cancellationToken);

}
