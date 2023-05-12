using RestSharp;

namespace CertificateUpdater.Services.Extensions;
internal static class RestRequestExtensions
{
	public static RestRequest AddAuthorizationHeaders(this RestRequest request) => request
			.AddHeader("Content-Type", "application/x-www-form-urlencoded")
			.AddHeader("charset", "utf-8");
}
