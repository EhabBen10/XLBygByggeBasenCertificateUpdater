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
		ICollection<CatChange> results = new List<CatChange>();

		foreach (var response in responses.Result.CatChangesData)
		{
			if (response is null)
			{
				throw new ArgumentNullException(nameof(responses));
			}

			CatChange result = new()
			{
				EmneId = response.EmneId,
				Tunnr = response.Tunnr,
				CreatedAt = Convert.ToDateTime(response.Created),
			};
			results.Add(result);
		}
		ICollection<CatChange> filteredObjects = results
		   .GroupBy(obj => obj.Tunnr)
				.Select(group => group.OrderByDescending(obj => obj.CreatedAt).First())
		   .ToList();
		return results;
	}
}
