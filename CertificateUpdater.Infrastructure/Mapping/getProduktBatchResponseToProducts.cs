using CertificateUpdater.Domain.Entities;
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
			if (response is null || string.IsNullOrEmpty(response.SupplierNr))
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
				';',  // Colon
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
				ProductGroupId = response.ProductGroupId,
				IsDeleted = !string.IsNullOrWhiteSpace(response.Slet_dato)
			};
			if (!response.HazardMark.IsNullOrEmpty())
			{
				result.HazardInfo = new()
				{
					HasHazardousGoods = response.HasHazardousGoods,
					HazardMark = response.HazardMark,
					HazardClass = response.HazardClass,
					ShippingDesignation = response.ShippingDesignation,
					UNCode = response.UNCode,
					CompanyName = response.CompanyName,
					DBNr = response.DBNr,
					ProductGroupId = response.ProductGroupId,
					ProductText = response.ProductText1,
					SupplierNr = response.SupplierNr,
					IsDeleted = result.IsDeleted,
				};

				foreach (var item in response.ProductHazardSentencesData)
				{
					result.HazardInfo.ProductHazardSentences.Add(new()
					{
						AjourDate = item.AjourDate,
						AjourId = item.AjourId,
						AjourUser = item.AjourUser,
						Sentence = item.Sentence,
						SentenceCode = item.SentenceCode
					});
				}


				foreach (var item in response.ProductSafetySentencesData)
				{
					result.HazardInfo.ProductSafetySentences.Add(new()
					{
						AjourDate = item.AjourDate,
						AjourId = item.AjourId,
						AjourUser = item.AjourUser,
						Sentence = item.Sentence,
						SentenceCode = item.SentenceCode
					});

				}

				foreach (var item in response.ProductHazardSymbolsData)
				{
					result.HazardInfo.ProductHazardSymbols.Add(new()
					{
						SymDesc = item.SymDesc,
						SymImgUrl = item.SymImgUrl,
						SymName = item.SymName
					});
				}

			}
			foreach (var character in problematicCharacters)
			{
				result.ProductText = result.ProductText.Replace(character.ToString(), string.Empty);
				result.SupplierNr = result.SupplierNr.Replace(character.ToString(), string.Empty);
				result.CompanyName = result.CompanyName.Replace(character.ToString(), string.Empty);
			}

			foreach (var katalog in response!.KatalogData)
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
				if (!result.DGNBDocuments.Any(x => x.IndicatorNumber == document.IndicatorNumber))
				{
					result.DGNBDocuments.Add(document);
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
						IsDeleted = !string.IsNullOrWhiteSpace(response.Slet_dato),
						CompanyName = response.CompanyName,
						DBNr = response.DBNr,
						ProductGroupId = response.ProductGroupId,
						ProductText = response.ProductText1,
						SupplierNr = response.SupplierNr,
						ConversionFactor = ePDData.ConversionFactor ?? 0,
						CreationDate = creationDateTime,
						EN15084ACertification = ePDData.EN15804ACertification?.ToString(),
						EPDType = ePDData.EPDType ?? 0,
						FunctionalUnit = ePDData.FunctionalUnit,
						FunctionalUnitAmount = ePDData.FunctionalUnitAmount ?? 0,
						ISO14025Certified = ePDData.ISO14025Certified ?? false,
						ISO14040Certified = ePDData.ISO14040Certified ?? false,
						ISO14044Certified = ePDData.ISO14044Certified ?? false,
						PdfAppxName = ePDData.PdfAppxName,
						ServiceLifeAmount = ePDData.ServiceLifeAmount ?? 0,
						PdfAppxDate = ePDData.PdfAppxDate?.UnixTimestampWithOffsetToDateTime() ?? new DateTime(1, 1, 1),
						PdfDate = ePDData.PdfDate?.UnixTimestampWithOffsetToDateTime() ?? new DateTime(1, 1, 1),
						PdfId = ePDData.PdfID,
						PdfName = ePDData.PdfName,
						ValidFrom = ePDData.ValidFrom?.UnixTimestampWithOffsetToDateTime() ?? new DateTime(1, 1, 1),
						ValidTo = ePDData.ValidTo?.UnixTimestampWithOffsetToDateTime() ?? new DateTime(1, 1, 1),
						EPDIndicatorLines = new()
						{
							A1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A1 ?? 0,
							A2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A2 ?? 0,
							A3 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A3 ?? 0,
							A4 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A4 ?? 0,
							A5 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A5 ?? 0,
							A1A3 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.A1A3 ?? 0,
							B1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B1 ?? 0,
							B2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B2 ?? 0,
							B3 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B3 ?? 0,
							B4 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B4 ?? 0,
							B5 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B5 ?? 0,
							B6 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B6 ?? 0,
							B7 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B7 ?? 0,
							B1B7 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B1B7 ?? 0,
							B1C1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B1C1 ?? 0,
							B2B7 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B2B7 ?? 0,
							B3B7 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.B3B7 ?? 0,
							C1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C1 ?? 0,
							C2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C2 ?? 0,
							C3 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C3 ?? 0,
							C3_1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C3_1 ?? 0,
							C3_2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C3_2 ?? 0,
							C4 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C4 ?? 0,
							C4_1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C4_1 ?? 0,
							C4_2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.C4_2 ?? 0,
							D = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.D ?? 0,
							D_1 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.D_1 ?? 0,
							D_2 = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.D_2 ?? 0,
							EPDHeaderId = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.EPDHeaderId ?? 0,
							Id = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.Id ?? 0,
							Indicator = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.Indicator,
							PhaseUnit = ePDData.EPDIndicatorLinesData?.FirstOrDefault()?.PhaseUnit,
						}
					});
				}
				foreach (var character in problematicCharacters)
				{
					foreach (var epd in ePDs)
					{
						epd.FunctionalUnit = epd.FunctionalUnit?.Replace(character.ToString(), string.Empty);
						epd.PdfAppxName = epd.PdfAppxName?.Replace(character.ToString(), string.Empty);
						epd.PdfName = epd.PdfName?.Replace(character.ToString(), string.Empty);
						if (epd.EPDIndicatorLines is not null && !string.IsNullOrEmpty(epd.EPDIndicatorLines.Indicator))
						{
							epd.EPDIndicatorLines.Indicator = epd.EPDIndicatorLines?.Indicator?.Replace(character.ToString(), string.Empty);
						}
						if (epd.EPDIndicatorLines is not null && !string.IsNullOrEmpty(epd.EPDIndicatorLines.PhaseUnit))
						{
							epd.EPDIndicatorLines.PhaseUnit = epd.EPDIndicatorLines?.PhaseUnit?.Replace(character.ToString(), string.Empty);
						}
					}
				}
				result.EPDs = ePDs;
			}
			results.Add(result);
		}
		results.ToList().RemoveAll(x => x.DBNr.ToString().Length != 7);
		return results;
	}
}
