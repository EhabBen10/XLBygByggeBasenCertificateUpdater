using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Responses.Common;

namespace CertificateUpdater.Services.Extensions;
internal static class ResponseExtension
{
	internal static string After<TResponse>(this IResponse<PaginatedResponse<TResponse>> response)
		where TResponse : class
		=> response.Data?.Paging.Next.After ?? string.Empty;
}
