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
		IResponse<GetProductBatchResponse> response;
		ICollection<Product> allChanges = new List<Product>();

		if (tunnrs.Count < 1000)
		{
			RestRequest request = new("http://services.byggebasen.dk/V3/BBService.svc/getProduktBatch");
			getProduktBatchBody body = new()
			{
				tunnr = tunnrs,
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

			response = await RestClient.PostAsync<GetProductBatchResponse>(request, cancellationToken);
			var changes = response.GetResult(getProduktBatchResponseToProducts.ToProducts);
			return changes.Value;
		}
		int startNumber = 0;
		int endNumber = 999;

		while (endNumber != tunnrs.Count)
		{
			RestRequest request = new("http://services.byggebasen.dk/V3/BBService.svc/getProduktBatch");
			getProduktBatchBody body = new()
			{
				tunnr = tunnrs.Skip(startNumber).Take(endNumber - startNumber).ToList(),
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

			response = await RestClient.PostAsync<GetProductBatchResponse>(request, cancellationToken);
			var changes = response.GetResult(getProduktBatchResponseToProducts.ToProducts);
			foreach (var item in changes.Value)
			{
				allChanges.Add(item);
			}
			startNumber = endNumber + 1;
			endNumber = tunnrs.Count - endNumber < 1000 ? tunnrs.Count : endNumber + 1000;
		}
		return allChanges;
	}
}
