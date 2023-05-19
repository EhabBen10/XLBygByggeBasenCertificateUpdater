using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Settings;
using Microsoft.Extensions.Options;
using RestSharp;

namespace CertificateUpdater.Services.RestSharp;
public sealed class RestSharpClient<TSettings> : IClient<TSettings> where TSettings : BaseSettings
{
	private readonly RestClient _client;

	public RestSharpClient(
		IOptions<TSettings> options)
		=> _client = string.IsNullOrWhiteSpace(options.Value.BaseUrl)
		? new RestClient()
		: new RestClient(options.Value.BaseUrl);

	public async Task<IResponse<TResponse>> GetAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default)
		where TResponse : class
		=> new RestSharpResponse<TResponse>(request, await _client.ExecuteGetAsync<TResponse>(request, cancellationToken));

	public async Task<IResponse<TResponse>> PostAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default)
		where TResponse : class
		=> new RestSharpResponse<TResponse>(request, await _client.ExecutePostAsync<TResponse>(request, cancellationToken));
}

