using System.Globalization;
using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;
using CertificateUpdater.Services.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace CertificateUpdater.Services.Services.CSVFileCreators;

public sealed class CertificationChangeCSVCreator : ICertificationChangeCSVCreator
{
	public Result<string> CreateCertificationChangeCSVFile(string basePath, List<CertificationChange> changes)
	{
		string certificationFile = @$"{basePath}\ResultCSV\CertificationUpdates" + DateTime.Now.ToShortDateString() + ".csv";

		if (File.Exists(certificationFile))
		{
			File.Delete(certificationFile);
		}
		string sourceFolderPath = @$"{basePath}\ResultCSV\";
		string certFolderPath = Path.Combine(@$"{basePath}\ResultCSV\", "CertificationUpdatesFolder");

		// Create the destination folder if it doesn't exist
		if (!Directory.Exists(certFolderPath))
		{
			Directory.CreateDirectory(certFolderPath);
		}

		// Get all files in the source folder
		string[] sourcefiles = Directory.GetFiles(sourceFolderPath);

		foreach (string filePath in sourcefiles)
		{
			string destinationPath = "";
			// Check if the file name contains the specified string
			if (Path.GetFileName(filePath).Contains("CertificationUpdates"))
			{
				// Build the destination path
				destinationPath = Path.Combine(certFolderPath, Path.GetFileName(filePath));
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
			using var writer = new StreamWriter(certificationFile);
			CsvConfiguration config = new(CultureInfo.CurrentCulture)
			{
				Delimiter = ";",
			};
			using var csv = new CsvWriter(writer, config);
			csv.WriteField("Firmanavn");
			csv.WriteField("Leverandørnr");
			csv.WriteField("DB nr.");
			csv.WriteField("Varegruppe id");
			csv.WriteField("Varetekst 1");
			csv.WriteField("DGNB Kvalitetstrin");
			csv.WriteField("Svanemærke");
			csv.WriteField("Svanemærket byggeri");
			csv.WriteField("EUTR (EU Timber Regulation)");
			csv.WriteField("BREEAM");
			csv.WriteField("LEED");
			csv.WriteField("SITAC");
			csv.WriteField("NEMKO");
			csv.WriteField("M1");
			csv.WriteField("Global Compact");
			csv.WriteField("GEV EMICODE");
			csv.WriteField("Cradle to Cradle");
			csv.WriteField("Der Blaue Engel");
			csv.WriteField("Certificeret træ PEFC - FSC");
			csv.WriteField("Certificeret træ PEFC");
			csv.WriteField("Indeklimamærkning");
			csv.WriteField("Astma - og Allergi Danmark");
			csv.WriteField("EPD – Miljøvaredeklaration");
			csv.WriteField("Allergy UK");
			csv.WriteField("EU Blomsten");
			csv.WriteField("Energimærkning");
			csv.WriteField("Is deleted");
			csv.NextRecord();

			foreach (CertificationChange change in changes)
			{
				csv.WriteField(change.CompanyName);
				csv.WriteField(change.SupplierNr);
				csv.WriteField(change.DBNr);
				csv.WriteField(change.ProductGroupId);
				csv.WriteField(change.ProductText);
				string? currentField = "";
				foreach (var item in change.DGNBQualityStep.ToList())
				{
					if (item == change.DGNBQualityStep.ToList().FirstOrDefault())
					{
						currentField = change.DGNBQualityStep.ToList().FirstOrDefault();
					}
					else
					{
						currentField += "," + item;
					}
				};
				csv.WriteField("\"" + currentField + "\"");
				csv.WriteField(Convert.ToInt32(change.hasSvanemærke));
				csv.WriteField(Convert.ToInt32(change.hasSvanemærkeByggeri));
				csv.WriteField(Convert.ToInt32(change.hasEUTR));
				csv.WriteField(Convert.ToInt32(change.hasBREEAM));
				csv.WriteField(Convert.ToInt32(change.hasLEED));
				csv.WriteField(Convert.ToInt32(change.hasSITAC));
				csv.WriteField(Convert.ToInt32(change.hasNEMKO));
				csv.WriteField(Convert.ToInt32(change.hasM1));
				csv.WriteField(Convert.ToInt32(change.hasGlobalCompact));
				csv.WriteField(Convert.ToInt32(change.hasGEV_EMICODE));
				csv.WriteField(Convert.ToInt32(change.hasC2C));
				csv.WriteField(Convert.ToInt32(change.hasDBE));
				csv.WriteField(Convert.ToInt32(change.hasFSC));
				csv.WriteField(Convert.ToInt32(change.hasPEFC));
				csv.WriteField(Convert.ToInt32(change.hasIndeKlima));
				csv.WriteField(Convert.ToInt32(change.hasAstmaOgAllergi));
				csv.WriteField(Convert.ToInt32(change.hasEPD));
				csv.WriteField(Convert.ToInt32(change.hasALUK));
				csv.WriteField(Convert.ToInt32(change.hasEUBlomsten));
				csv.WriteField(Convert.ToInt32(change.hasEnergiMærkning));
				csv.WriteField(Convert.ToInt32(change.isDeleted));
				csv.NextRecord();
			}
		}
		catch (Exception)
		{
			return Result.Failure<string>(Error.NullValue);
		}
		return Result.Success(certificationFile);
	}
}
