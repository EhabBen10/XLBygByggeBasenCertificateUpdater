using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Infrastructure.Responses.GetKatalogChanges;

namespace CertificateUpdater.Services.Mapping;
internal static class GetKatalogChangesResponseToKatalogChanges
{
	internal static List<CatChange> ToKatalogChanges(this List<GetKatalogChangesResponse> responses)
	{
		if (responses is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}
		List<CatChange> results = new List<CatChange>();

		foreach (var response in responses)
		{
			if (response.CatChangeData is null)
			{
				throw new ArgumentNullException(nameof(response));
			}
			foreach (var change in response.CatChangeData)
			{
				CatChange catChange = new()
				{
					EmneId = change.EmneId,
					Tunnr = change.Tunnr,
				};
				results.Add(catChange);
			}
		}
		return results;
	}
}

