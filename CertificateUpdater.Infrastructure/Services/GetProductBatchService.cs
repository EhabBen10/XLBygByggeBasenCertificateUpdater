using System.Text.Json;
using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.RequestBodies;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Mapping;
using CertificateUpdater.Services.Responses.GetProductBatch;
using CertificateUpdater.Services.Settings;
using RestSharp;

namespace CertificateUpdater.Services.Services;
public sealed class GetProductBatchService : IGetProductBatchService
{
	ICredentialProvider CredentialProvider { get; set; }
	IClient<BaseSettings> RestClient { get; set; }
	public GetProductBatchService(IClient<BaseSettings> restClient, ICredentialProvider credentialProvider)
	{
		CredentialProvider = credentialProvider ?? throw (new ArgumentNullException(nameof(credentialProvider)));
		RestClient = restClient ?? throw (new ArgumentNullException(nameof(restClient)));
	}
	public async Task<ICollection<Product>> GetProductBatch(ICollection<int> tunnrs, CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("http://servicetest.byggebasen.com/EPDTEST/BBService.svc/getProduktBatch");
		getProduktBatchBody body = new()
		{
			tunnr = tunnrs.ToList(),
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

		var response = await RestClient.PostAsync<GetProductBatchResponse>(request, cancellationToken);
		var changes = response.GetResult(getProduktBatchResponseToProducts.ToProducts);

		return changes.Value;
	}
}
