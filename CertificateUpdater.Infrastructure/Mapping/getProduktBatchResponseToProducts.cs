using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Services.Responses.GetProductBatch;

namespace CertificateUpdater.Services.Mapping;
internal static class getProduktBatchResponseToProducts
{
	internal static ICollection<Product> ToProducts(this GetProductBatchResponse responses)
	{
		if (responses is null)
		{
			throw new ArgumentNullException(nameof(responses));
		}
		ICollection<Product> results = new List<Product>();

		foreach (var response in responses.Result.ResultData)
		{
			if (response is null || response.SupplierNr is null)
			{
				throw new ArgumentNullException(nameof(response));
			}
			Product result = new()
			{
				ProductText = response.ProductText1,
				SupplierNr = response.SupplierNr,
				DBNr = response.DBNr,
				CompanyName = response.CompanyName
			};

			foreach (var katalog in response.KatalogData)
			{
				result.KatalogData.Add(new()
				{
					isValid = katalog.isValid,
					EmneId = katalog.EmneId,
					Tunnr = katalog.Tunnr,
				});
			}
			results.Add(result);
		}
		return results;
	}
}
