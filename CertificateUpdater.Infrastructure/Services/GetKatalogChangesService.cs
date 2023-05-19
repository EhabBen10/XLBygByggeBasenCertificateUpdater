using System.Text.Json;
using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.RequestBodies;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Mapping;
using CertificateUpdater.Services.Responses.GetKatalogChanges;
using CertificateUpdater.Services.Settings;
using RestSharp;

namespace CertificateUpdater.Services.Services;
public sealed class GetKatalogChangesService : IGetKatalogChangesService
{
	ILogProvider LogProvider { get; set; }
	ICredentialProvider CredentialProvider { get; set; }
	IClient<BaseSettings> RestClient { get; set; }
	public GetKatalogChangesService(IClient<BaseSettings> restClient, ILogProvider logProvider, ICredentialProvider credentialProvider)
	{
		LogProvider = logProvider ?? throw (new ArgumentNullException(nameof(logProvider)));
		CredentialProvider = credentialProvider ?? throw (new ArgumentNullException(nameof(credentialProvider)));
		RestClient = restClient ?? throw (new ArgumentNullException(nameof(restClient)));

	}
	public async Task<ICollection<CatChange>> GetKatalogChanges(CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("http://servicetest.byggebasen.com/EPDTEST/BBService.svc/GetKatalogChanges");
		var options = new RestClientOptions("http://servicetest.byggebasen.com/EPDTEST/BBService.svc/");
		GetKatalogChangesBody body = new()
		{
			fromDate = LogProvider.GetLastLog(),
			tunuser = new TunUser()
			{
				TunUserNr = CredentialProvider.GetTunUserNr(),
				UserName = CredentialProvider.GetUserName(),
				Password = CredentialProvider.GetPassword()
			}
		};
		request.AddHeader("Content-Type", "application/json");
		request.AddHeader("Accept", "application/json");
		var json = JsonSerializer.Serialize(body);
		request.AddParameter("application/json", json, ParameterType.RequestBody);

		var response = await RestClient.PostAsync<GetKatalogChangesResponse>(request, cancellationToken);
		var changes = response.GetResult(GetKatalogChangesResponseToKatalogChanges.ToKatalogChanges);

		return changes.Value;
	}
}
