using System.Text.Json;
using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.RequestBodies;
using CertificateUpdater.Domain.Shared;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Mapping;
using CertificateUpdater.Services.Responses.GetProductBatch;
using CertificateUpdater.Services.Settings;
using Microsoft.IdentityModel.Tokens;
using RestSharp;

namespace CertificateUpdater.Services.Services;
public sealed class GetProductBatchService : IGetProductBatchService
{
	ICredentialProvider CredentialProvider { get; set; }
	IClient<BaseSettings> RestClient { get; set; }
	private TunUser _tunUser { get; set; }
	public GetProductBatchService(IClient<BaseSettings> restClient, ICredentialProvider credentialProvider)
	{
		CredentialProvider = credentialProvider ?? throw (new ArgumentNullException(nameof(credentialProvider)));
		RestClient = restClient ?? throw (new ArgumentNullException(nameof(restClient)));
		_tunUser = new TunUser()
		{
			TunUserNr = CredentialProvider.GetTunUserNr(),
			UserName = CredentialProvider.GetUserName(),
			Password = CredentialProvider.GetPassword()
		};
	}
	public async Task<Result<ICollection<Product>>> GetProductBatch(ICollection<int> tunnrs, CancellationToken cancellationToken)
	{
		IResponse<GetProductBatchResponse> response;
		Result<ICollection<Product>> allChanges = new List<Product>();
		if (tunnrs.IsNullOrEmpty())
		{
			return Result.Failure<ICollection<Product>>(Error.NullValue);
		}
		if (tunnrs.Count < 1000)
		{
			RestRequest request = new("http://services.byggebasen.dk/V3/BBService.svc/getProduktBatch");
			getProduktBatchBody body = new()
			{
				tunnr = tunnrs,
				tunUser = _tunUser
			};
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accept", "application/json");
			var json = JsonSerializer.Serialize(body);
			request.AddParameter("application/json", json, ParameterType.RequestBody);

			response = await RestClient.PostAsync<GetProductBatchResponse>(request, cancellationToken);
			return response.GetResult(getProduktBatchResponseToProducts.ToProducts);
		}
		int startNumber = 0;
		int endNumber = 999;

		while (endNumber != tunnrs.Count)
		{
			RestRequest request = new("http://services.byggebasen.dk/V3/BBService.svc/getProduktBatch");
			getProduktBatchBody body = new()
			{
				tunnr = tunnrs.Skip(startNumber).Take(endNumber - startNumber).ToList(),
				tunUser = _tunUser
			};
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Accept", "application/json");
			var json = JsonSerializer.Serialize(body);
			request.AddParameter("application/json", json, ParameterType.RequestBody);

			response = await RestClient.PostAsync<GetProductBatchResponse>(request, cancellationToken);
			var changes = response.GetResult(getProduktBatchResponseToProducts.ToProducts);
			if (changes.IsSuccess)
			{
				foreach (var item in changes.Value)
				{
					allChanges.Value.Add(item);
				}
			}
			else
			{
				return Result.Failure<ICollection<Product>>(new Error("500", "An unexpected error occured"));
			}
			startNumber = endNumber + 1;
			endNumber = tunnrs.Count - endNumber < 1000 ? tunnrs.Count : endNumber + 1000;
		}
		return allChanges;
	}
}
