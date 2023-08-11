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

		if (responses.Result is null || responses.Result.ResultData is null)
		{
			return new List<Product>();
		}

		foreach (var response in responses.Result.ResultData)
		{
			if (response is null || response.SupplierNr is null)
			{
				throw new ArgumentNullException(nameof(responses));
			}
			char[] problematicCharacters = new[]
			{
				'\t', // Tab
				'\n', // Newline
				'\r', // Carriage Return
				'\f', // Form Feed
				'\b', // Backspace
				'\a', // Alert (Bell)
				'\v', // Vertical Tab
				'"',  // Double quote
				'\'', // Single quote (Apostrophe)
				'*',  // Asterisk
				':',  // Colon
				'\\', // Backslash
				'/',  // Forward slash
				'?',  // Question mark
				'[',  // Left square bracket
				']',  // Right square bracket
			};
			Product result = new()
			{
				ProductText = response.ProductText1,
				SupplierNr = response.SupplierNr,
				DBNr = response.DBNr,
				CompanyName = response.CompanyName,
				ProductGroupId = response.ProductGroupId
			};
			foreach (var character in problematicCharacters)
			{
				result.ProductText = result.ProductText.Replace(character.ToString(), string.Empty);
				result.SupplierNr = result.SupplierNr.Replace(character.ToString(), string.Empty);
				result.CompanyName = result.CompanyName.Replace(character.ToString(), string.Empty);
			}

			foreach (var katalog in response.KatalogData)
			{
				result.KatalogData.Add(new()
				{
					isValid = katalog.isValid,
					EmneId = katalog.EmneId,
					Tunnr = katalog.Tunnr,
				});
			}
			ICollection<DGNBDocument> documents = new List<DGNBDocument>();
			foreach (var document in response.DGNBDocuments)
			{
				documents.Add(new()
				{
					IndicatorNumber = document.IndicatorNumber,
					IndicatorStep = document.IndicatorStep,
				});
			}
			foreach (var document in documents)
			{
				if (!result.dGNBDocuments.Any(x => x.IndicatorNumber == document.IndicatorNumber))
				{
					result.dGNBDocuments.Add(document);
				}
			}
			results.Add(result);
		}
		return results;
	}
}
