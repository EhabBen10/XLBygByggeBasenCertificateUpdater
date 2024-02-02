using System.Text.Json;
using CertificateUpdater.Domain.RequestBodies;
using CertificateUpdater.Domain.Shared;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Mapping;
using CertificateUpdater.Services.Responses.GetProductChanges;
using CertificateUpdater.Services.Settings;
using RestSharp;

namespace CertificateUpdater.Services.Services;
public sealed class GetProductChangesService : IGetProductChangesService
{
	ILogProvider LogProvider { get; set; }
	ICredentialProvider CredentialProvider { get; set; }
	IClient<BaseSettings> RestClient { get; set; }
	public GetProductChangesService(IClient<BaseSettings> restClient, ILogProvider logProvider, ICredentialProvider credentialProvider)
	{
		LogProvider = logProvider ?? throw (new ArgumentNullException(nameof(logProvider)));
		CredentialProvider = credentialProvider ?? throw (new ArgumentNullException(nameof(credentialProvider)));
		RestClient = restClient ?? throw (new ArgumentNullException(nameof(restClient)));

	}
	public async Task<Result<ICollection<int>>> GetProductChanges(CancellationToken cancellationToken)
	{
		RestRequest request = new("http://services.byggebasen.dk/V3/BBService.svc/getChangedProdukt");
		GetProductChangesBody body = new()
		{
			date = LogProvider.GetLastLog(),
			tunUser = new TunUser()
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
		var response = await RestClient.PostAsync<GetProductChangesResponse>(request, cancellationToken);
		return response.GetResult(GetProductChangesResponseToProductChanges.ToProductChanges);
	}
}
