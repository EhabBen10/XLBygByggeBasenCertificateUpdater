using CertificateUpdater.Domain.Shared;

namespace CertificateUpdater.Services.Interfaces;
public interface IResponse<TResponse>
{
	TResponse? Data { get; }

	Result<TResult> GetResult<TResult>(Func<TResponse, TResult> mappingFunc);
}

