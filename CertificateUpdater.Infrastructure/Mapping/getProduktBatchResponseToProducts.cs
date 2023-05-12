using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Services.Responses.getProduktBatch;

namespace CertificateUpdater.Services.Mapping;
internal static class getProduktBatchResponseToProducts
{
	internal static List<Product> ToProducts(this List<getProduktBatchResponse> responses)
	{
		if (responses is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}
		List<Product> results = new List<Product>();

		foreach (var response in responses)
		{
			if (response.ResultData is null)
			{
				throw new ArgumentNullException(nameof(response));
			}
			Product product = new();
			foreach (var change in response.ResultData)
			{
				Katalog newKatalog = new Katalog();
				foreach (var katalog in change.KatalogData)
				{
					newKatalog.isValid = katalog.Valid;
					newKatalog.Tunnr = katalog.Tunnr;
					newKatalog.EmneId = katalog.EmneId;
				};
				product.KatalogData.Add(newKatalog);
			}
			results.Add(product);
		}
		return results;
	}
}
