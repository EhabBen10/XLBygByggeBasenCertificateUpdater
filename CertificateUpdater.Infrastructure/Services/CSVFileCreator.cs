﻿using System.Globalization;
using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;
using CertificateUpdater.Services.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace CertificateUpdater.Services.Services;

public sealed class CSVFileCreator : ICSVFileCreator
{
	public Result<string> CreateCSVFiles(List<CertificationChange> changes)
	{
		string file = @"O:\IT\EG-FIT fællesdrev\Dokumentation\Intern\Varevedligehold\Bæredygtige varer\Byg-e udtræk\ResultCSV\CertificationUpdates" + DateTime.Now.ToShortDateString() + ".csv";
		if (File.Exists(file))
		{
			File.Delete(file);
		}

		try
		{
			using (var writer = new StreamWriter(file))
			{
				CsvConfiguration config = new(CultureInfo.InvariantCulture)
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
				csv.WriteField("BREEAM");
				csv.WriteField("LEED");
				csv.WriteField("Cradle to Cradle");
				csv.WriteField("Der Blaue Engel");
				csv.WriteField("Certificeret træ PEFC - FSC");
				csv.WriteField("Certificeret træ PEFC");
				csv.WriteField("Indeklimamærkning");
				csv.WriteField("Astma - og Allergi Danmark");
				csv.WriteField("EPD – Miljøvaredeklaration");
				csv.WriteField("Allergy UK");
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
					csv.WriteField(Convert.ToInt32(change.hasBREEAM));
					csv.WriteField(Convert.ToInt32(change.hasLEED));
					csv.WriteField(Convert.ToInt32(change.hasC2C));
					csv.WriteField(Convert.ToInt32(change.hasDBE));
					csv.WriteField(Convert.ToInt32(change.hasFSC));
					csv.WriteField(Convert.ToInt32(change.hasPEFC));
					csv.WriteField(Convert.ToInt32(change.hasIndeKlima));
					csv.WriteField(Convert.ToInt32(change.hasAstmaOgAllergi));
					csv.WriteField(Convert.ToInt32(change.hasEPD));
					csv.WriteField(Convert.ToInt32(change.hasALUK));
					csv.NextRecord();
				}
			}

			return Result.Success(file);
		}
		catch (Exception)
		{
			return Result.Failure<string>(Error.NullValue);
		}
	}
}
