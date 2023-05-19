using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Services.Responses.GetKatalogChanges;

namespace CertificateUpdater.Services.Mapping;
internal static class GetKatalogChangesResponseToKatalogChanges
{
	internal static ICollection<CatChange> ToKatalogChanges(this GetKatalogChangesResponse responses)
	{
		if (responses is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}
		List<CatChange> results = new List<CatChange>();


		foreach (var response in responses.Result.CatChangesData)
		{
			if (response is null)
			{
				throw new ArgumentNullException(nameof(response));
			}

			CatChange result = new()
			{
				EmneId = response.EmneId,
				Tunnr = response.Tunnr,
			};

			results.Add(result);
		}
		return results;
	}
}

