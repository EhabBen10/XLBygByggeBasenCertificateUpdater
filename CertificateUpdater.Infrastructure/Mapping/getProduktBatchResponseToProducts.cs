﻿using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Services.Extensions;
using CertificateUpdater.Services.Responses.GetProductBatch;
using Microsoft.IdentityModel.Tokens;

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
			if (!response.EPDDatas.IsNullOrEmpty())
			{
				ICollection<EPD> ePDs = new List<EPD>();
				foreach (var ePDData in response.EPDDatas!)
				{
					string creationDateString = ePDData.CreationDate;

					// Extract the timestamp and offset from the string
					int start = creationDateString.IndexOf('(') + 1;
					int end = creationDateString.IndexOf('+');
					long timestamp = long.Parse(creationDateString[start..end]);

					start = end + 1;
					end = creationDateString.IndexOf(')');
					int offsetMinutes = int.Parse(creationDateString[start..end]);

					// Calculate the DateTime
					DateTime creationDateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;

					// Adjust the DateTime with the offset
					creationDateTime = creationDateTime.AddMinutes(offsetMinutes);
					ePDs.Add(new()
					{
						ConversionFactor = ePDData.ConversionFactor,
						CreationDate = creationDateTime,
						EN15084ACertification = ePDData.EN15804ACertification,
						EPDType = ePDData.EPDType,
						FunctionalUnit = ePDData.FunctionalUnit,
						FunctionalUnitAmount = ePDData.FunctionalUnitAmount,
						ISO14025Certified = ePDData.ISO14025Certified,
						ISO14040Certified = ePDData.ISO14040Certified,
						ISO14044Certified = ePDData.ISO14044Certified,
						PdfAppxName = ePDData.PdfAppxName,
						ServiceLifeAmount = ePDData.ServiceLifeAmount,
						PdfAppxDate = ePDData.PdfAppxDate?.UnixTimestampWithOffsetToDateTime(),
						PdfDate = ePDData.PdfDate?.UnixTimestampWithOffsetToDateTime(),
						PdfId = ePDData.PdfID,
						PdfName = ePDData.PdfName,
						ValidFrom = ePDData.ValidFrom?.UnixTimestampWithOffsetToDateTime(),
						ValidTo = ePDData.ValidTo?.UnixTimestampWithOffsetToDateTime(),
						EPDIndicatorLines = new()
						{
							A1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A1,
							A2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A2,
							A3 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A3,
							A4 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A4,
							A5 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A5,
							A1A3 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A1A3,
							B1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B1,
							B2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B2,
							B3 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B3,
							B4 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B4,
							B5 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B5,
							B6 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B6,
							B7 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B7,
							B1B7 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B1B7,
							B1C1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B1C1,
							B2B7 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B2B7,
							B3B7 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B3B7,
							C1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C1,
							C2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C2,
							C3 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C3,
							C3_1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C3_1,
							C3_2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C3_2,
							C4 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C4,
							C4_1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C4_1,
							C4_2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C4_2,
							D = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.D,
							D_1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.D_1,
							D_2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.D_2,
							EPDHeaderId = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.EPDHeaderId,
							Id = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.Id,
							Indicator = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.Indicator,
							PhaseUnit = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.PhaseUnit,
						}
					});
				}
				foreach (var character in problematicCharacters)
				{
					foreach (var epd in ePDs)
					{
						epd.FunctionalUnit = epd.FunctionalUnit.Replace(character.ToString(), string.Empty);
						epd.PdfAppxName = epd.PdfAppxName?.Replace(character.ToString(), string.Empty);
						epd.PdfName = epd.PdfName?.Replace(character.ToString(), string.Empty);
						if (epd.EPDIndicatorLines is not null && epd.EPDIndicatorLines.Indicator is not null)
						{
							epd.EPDIndicatorLines.Indicator = epd.EPDIndicatorLines?.Indicator?.Replace(character.ToString(), string.Empty);

						}
						if (epd.EPDIndicatorLines is not null && epd.EPDIndicatorLines.PhaseUnit is not null)
						{
							epd.EPDIndicatorLines.PhaseUnit = epd.EPDIndicatorLines?.PhaseUnit?.Replace(character.ToString(), string.Empty);
						}
					}
				}
				result.EPDs = ePDs;
			}
			results.Add(result);
		}
		return results;
	}
}
