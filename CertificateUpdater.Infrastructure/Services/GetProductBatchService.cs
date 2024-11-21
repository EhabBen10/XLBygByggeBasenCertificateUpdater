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
			return Result.Failure<ICollection<Product>>(new Error("Null value", "No new updates"));
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

		int batchSize = 999;
		int totalProducts = tunnrs.Count;
		int totalBatches = (totalProducts + batchSize - 1) / batchSize;
		List<Task<Result<ICollection<Product>>>> tasks = new List<Task<Result<ICollection<Product>>>>();
		List<int> failedTunnrs = new List<int>();
		Console.WriteLine($"Getting {totalProducts} products in {totalBatches} batches.");
		int completedBatches = 0;
		int maxConcurrency = 3;
		SemaphoreSlim semaphore = new SemaphoreSlim(maxConcurrency); // Semaphore to limit concurrency

		for (int start = 0; start < totalProducts; start += batchSize)
		{
			var batch = tunnrs.Skip(start).Take(batchSize).ToList();

			semaphore.WaitAsync(cancellationToken);
			var batchTask = SendBatchRequest(batch, cancellationToken, allChanges, failedTunnrs);
			await batchTask.ContinueWith(t =>
				   {
					   semaphore.Release();
					   lock (tasks)
					   {
						   if (t.IsFaulted)
						   {
							   // Log the exception here
							   Console.WriteLine($"Batch failed with exception: {t.Exception?.Message}");
							   failedTunnrs.AddRange(batch);
						   }
						   else
						   {
							   completedBatches++;
							   double progress = (double)completedBatches / totalBatches * 100;
							   Console.SetCursorPosition(0, 3);
							   Console.Write($"Progress: {completedBatches} / {totalBatches} batches completed ({progress:F2}%)");
						   }
					   }
				   },TaskContinuationOptions.OnlyOnRanToCompletion);
			tasks.Add(batchTask);
		}
		// Wait for all tasks to complete
		await Task.WhenAll(tasks);

		if (failedTunnrs.Any())
		{
			Console.WriteLine("Retrying failed batches...");

			// Create new tasks to retry failed batches
			List<Task<Result<ICollection<Product>>>> retryTasks = new List<Task<Result<ICollection<Product>>>>();
			retryTasks.Add(SendBatchRequest(failedTunnrs, cancellationToken, allChanges, null));

			// Wait for retry attempts to complete
			await Task.WhenAll(retryTasks);

			// Check if any retries failed
			if (failedTunnrs.Any())
			{
				return Result.Failure<ICollection<Product>>(new Error("500", "Some batches failed even after retrying"));
			}
		}


		Console.SetCursorPosition(0, 3);
		Console.CursorVisible = true;
		return allChanges;
	}

	private async Task<Result<ICollection<Product>>> SendBatchRequest(
	ICollection<int> batch,
	CancellationToken cancellationToken,
	Result<ICollection<Product>> allChanges,
	List<int> failedTunnrs)
	{

		Console.SetCursorPosition(1, 3);
		RestRequest request = new("http://services.byggebasen.dk/V3/BBService.svc/getProduktBatch");
		getProduktBatchBody body = new()
		{
			tunnr = batch,
			tunUser = _tunUser
		};
		request.AddHeader("Content-Type", "application/json");
		request.AddHeader("Accept", "application/json");
		var json = JsonSerializer.Serialize(body);
		request.AddParameter("application/json", json, ParameterType.RequestBody);

		IResponse<GetProductBatchResponse> response;
		int maxRetries = 3;
		int retryCount = 0;

		do
		{
			try
			{
				// Send the request
				response = await RestClient.PostAsync<GetProductBatchResponse>(request, cancellationToken);
				var result = response.GetResult(getProduktBatchResponseToProducts.ToProducts);
				if (result.IsSuccess)
				{
					foreach (var product in result.Value)
					{
						allChanges.Value.Add(product);  // Add to the collection of products
					}
					return Result.Success(result.Value);
				}
			}
			catch (Exception)
			{
				retryCount++;
				if (retryCount >= maxRetries && failedTunnrs != null)
				{
					failedTunnrs.AddRange(batch);  // Add batch to failed list if retries exhausted
					return Result.Failure<ICollection<Product>>(new Error("500", "Maximum retries exceeded for batch"));
				}
			}
		} while (retryCount < maxRetries);

		return Result.Failure<ICollection<Product>>(new Error("500", "Batch failed"));
	}
}
