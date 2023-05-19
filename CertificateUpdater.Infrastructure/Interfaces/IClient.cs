using RestSharp;

namespace CertificateUpdater.Services.Interfaces;
public interface IClient<TSettings>
{
	Task<IResponse<TResponse>> GetAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default)
		where TResponse : class;

	Task<IResponse<TResponse>> PostAsync<TResponse>(
		RestRequest request,
		CancellationToken cancellationToken = default)
		where TResponse : class;

}
