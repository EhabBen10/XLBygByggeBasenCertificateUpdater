using System.Globalization;
using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Enum;
using CertificateUpdater.Domain.Shared;
using CertificateUpdater.Services.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace CertificateUpdater.Services.Services.CSVFileCreators;
public class HazardChangeCSVCreator : IHazardChangeCSVCreator
{
	public Result<string> CreateHazardCSVFile(string basePath, List<HazardInfo> hazardInfos)
	{
		string hazardFile = @$"{basePath}\ResultCSV\HazardUpdates" + ".csv";
		if (File.Exists(hazardFile))
		{
			File.Delete(hazardFile);
		}
		string sourceFolderPath = @$"{basePath}\ResultCSV\";
		string hazardFolderPath = Path.Combine(@$"{basePath}\ResultCSV\", "HazardUpdatesFolder");
		if (!Directory.Exists(hazardFolderPath))
		{
			Directory.CreateDirectory(hazardFolderPath);
		}
		string[] sourcefiles = Directory.GetFiles(sourceFolderPath);

		foreach (string filePath in sourcefiles)
		{
			string destinationPath = "";
			// Check if the file name contains the specified string
			if (Path.GetFileName(filePath).Contains("HazardUpdates"))
			{
				// Build the destination path
				destinationPath = Path.Combine(hazardFolderPath, Path.GetFileName(filePath));
			}
			// Move the file to the destination folder
			if (!string.IsNullOrEmpty(destinationPath))
			{
				if (File.Exists(destinationPath))
				{
					File.Delete(destinationPath);
				}
				File.Move(filePath, destinationPath);
			}
			Console.WriteLine($"Moved: {filePath} to {destinationPath}");
		}

		try
		{
			using var writer = new StreamWriter(hazardFile);
			CsvConfiguration config = new(CultureInfo.CurrentCulture)
			{
				Delimiter = ";",
			};
			using var csv = new CsvWriter(writer, config);
			csv.WriteField("Fare record type");
			csv.WriteField("DB nr.");
			csv.WriteField("Varegruppe id");
			csv.WriteField("Firmanavn");
			csv.WriteField("Leverandørnr");
			csv.WriteField("Varetekst 1");
			csv.WriteField("Fareklasse");
			csv.WriteField("Faremærke");
			csv.WriteField("UN-kode");
			csv.WriteField("Leverings designation");
			csv.WriteField("Is deleted");
			csv.WriteField("Faresymbol Url");
			csv.WriteField("Faresymbol beskrivelse");
			csv.WriteField("Faresymbol navn");
			csv.WriteField("Faresætning");
			csv.WriteField("Faresætning kode");
			csv.WriteField("Sikkerhedssætning");
			csv.WriteField("Sikkerhedssætning kode");


			csv.NextRecord();

			foreach (HazardInfo hazardInfo in hazardInfos)
			{
				csv.WriteField(HazardRecordEnum.ProductInfo);
				csv.WriteField(hazardInfo.DBNr);
				csv.WriteField(hazardInfo.ProductGroupId);
				csv.WriteField(hazardInfo.CompanyName);
				csv.WriteField(hazardInfo.SupplierNr);
				csv.WriteField(hazardInfo.ProductText);
				csv.WriteField(hazardInfo.HazardClass);
				csv.WriteField(hazardInfo.HazardMark);
				csv.WriteField(hazardInfo?.UNCode ?? "");
				csv.WriteField(hazardInfo?.ShippingDesignation ?? "");
				csv.WriteField(Convert.ToInt32(hazardInfo!.IsDeleted));
				csv.NextRecord();

				var productHazardSentences = hazardInfo?.ProductHazardSentences ?? Enumerable.Empty<ProductHazardSentence>();
				foreach (var item in productHazardSentences)
				{
					csv.WriteField(HazardRecordEnum.HazardSentence);
					csv.WriteField(hazardInfo!.DBNr);
					csv.WriteField(hazardInfo.ProductGroupId);
					csv.WriteField(hazardInfo.CompanyName);
					csv.WriteField(hazardInfo.SupplierNr);
					csv.WriteField(hazardInfo.ProductText);
					csv.WriteField(hazardInfo.HazardClass);
					csv.WriteField(hazardInfo.HazardMark);
					csv.WriteField(hazardInfo?.UNCode ?? "");
					csv.WriteField(hazardInfo?.ShippingDesignation ?? "");
					csv.WriteField("");
					csv.WriteField("");
					csv.WriteField("");
					csv.WriteField(item.Sentence);
					csv.WriteField(item.SentenceCode);
					csv.NextRecord();
				}
				var productSafetySentences = hazardInfo?.ProductSafetySentences ?? Enumerable.Empty<ProductSafetySentence>();
				foreach (var item in productSafetySentences)
				{
					csv.WriteField(HazardRecordEnum.SafetySentence);
					csv.WriteField(hazardInfo!.DBNr);
					csv.WriteField(hazardInfo.ProductGroupId);
					csv.WriteField(hazardInfo.CompanyName);
					csv.WriteField(hazardInfo.SupplierNr);
					csv.WriteField(hazardInfo.ProductText);
					csv.WriteField(hazardInfo.HazardClass);
					csv.WriteField(hazardInfo.HazardMark);
					csv.WriteField(hazardInfo?.UNCode ?? "");
					csv.WriteField(hazardInfo?.ShippingDesignation ?? "");
					csv.WriteField("");
					csv.WriteField("");
					csv.WriteField("");
					csv.WriteField("");
					csv.WriteField("");
					csv.WriteField(item.Sentence);
					csv.WriteField(item.SentenceCode);
					csv.NextRecord();
				}
				var productHazardSymbols = hazardInfo?.ProductHazardSymbols ?? Enumerable.Empty<ProductHazardSymbol>();
				foreach (var item in productHazardSymbols)
				{
					csv.WriteField(HazardRecordEnum.HazardSymbol);
					csv.WriteField(hazardInfo!.DBNr);
					csv.WriteField(hazardInfo.ProductGroupId);
					csv.WriteField(hazardInfo.CompanyName);
					csv.WriteField(hazardInfo.SupplierNr);
					csv.WriteField(hazardInfo.ProductText);
					csv.WriteField(hazardInfo.HazardClass);
					csv.WriteField(hazardInfo.HazardMark);
					csv.WriteField(hazardInfo?.UNCode ?? "");
					csv.WriteField(hazardInfo?.ShippingDesignation ?? "");
					csv.WriteField(item.SymImgUrl);
					csv.WriteField(item.SymDesc);
					csv.WriteField(item.SymName);
					csv.NextRecord();
				}
			};

		}
		catch (Exception)
		{
			return Result.Failure<string>(Error.NullValue);
		}
		return Result.Success(hazardFile);
	}
}
