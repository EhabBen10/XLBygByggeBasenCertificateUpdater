using CertificateUpdater.Services.Responses.GetProductChanges;

namespace CertificateUpdater.Services.Mapping;
internal static class GetProductChangesResponseToProductChanges
{
	internal static ICollection<int> ToProductChanges(this GetProductChangesResponse responses)
	{
		if (responses is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}
		ICollection<int> results = new List<int>();

		foreach (var response in responses.Result.Result)
		{
			if (response == default)
			{
				throw new ArgumentNullException(nameof(responses));
			}
			int result = response;
			results.Add(result);
		}

		return results;
	}
}
