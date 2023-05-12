using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Infrastructure.Interfaces;
using RestSharp;

namespace CertificateUpdater.Services.Services;
public sealed class GetKatalogChangesService : IGetKatalogChangesService
{
	public async Task<ICollection<CatChange>> GetKatalogChanges(CancellationToken cancellationToken)
	{
		var options = new RestClientOptions("http://servicetest.byggebasen.com/EPDTEST/BBService.svc");
		var client = new RestClient(options);
		var request = new RestRequest("GetKatalogChanges").AddStringBody();
		// The cancellation token comes from the caller. You can still make a call without it.
		var response = await client.GetAsync(request, cancellationToken);

		return null;
	}

	ICollection<CatChange> IGetKatalogChangesService.GetKatalogChanges()
	{
		throw new NotImplementedException();
	}
}
