﻿using System.Globalization;
using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;
using CertificateUpdater.Services.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;

namespace CertificateUpdater.Services.Services;

public sealed class CSVFileCreator : ICSVFileCreator
{
	public Result<ICollection<string>> CreateCSVFiles(List<CertificationChange> changes)
	{
		ICollection<string> files = new List<string>();
		string certificationFile = @"O:\IT\EG-FIT fællesdrev\Dokumentation\Intern\Varevedligehold\Bæredygtige varer\Byg-e udtræk\ResultCSV\CertificationUpdates" + DateTime.Now.ToShortDateString() + ".csv";
		string epdFile = @"O:\IT\EG-FIT fællesdrev\Dokumentation\Intern\Varevedligehold\Bæredygtige varer\Byg-e udtræk\ResultCSV\EPDUpdates" + DateTime.Now.ToShortDateString() + ".csv";
		files.Add(certificationFile);
		files.Add(epdFile);
		foreach (var file in files)
		{
			if (File.Exists(file))
			{
				File.Delete(file);
			}
		}

		try
		{
			using var writer = new StreamWriter(certificationFile);
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
		catch (Exception)
		{
			return Result.Failure<ICollection<string>
				>(Error.NullValue);
		}
		try
		{
			using var writer = new StreamWriter(epdFile);
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
			csv.WriteField("ConversionFactor");
			csv.WriteField("CreationDate");
			csv.WriteField("EN15804ACertification");
			csv.WriteField("EPDType");
			csv.WriteField("A1");
			csv.WriteField("A1A3");
			csv.WriteField("A2");
			csv.WriteField("A3");
			csv.WriteField("A4");
			csv.WriteField("A5");
			csv.WriteField("B1");
			csv.WriteField("B1B7");
			csv.WriteField("B1C1");
			csv.WriteField("B2");
			csv.WriteField("B2B7");
			csv.WriteField("B3");
			csv.WriteField("B3B7");
			csv.WriteField("B4");
			csv.WriteField("B5");
			csv.WriteField("B6");
			csv.WriteField("B7");
			csv.WriteField("C1");
			csv.WriteField("C2");
			csv.WriteField("C3");
			csv.WriteField("C3_1");
			csv.WriteField("C3_2");
			csv.WriteField("C4");
			csv.WriteField("C4_1");
			csv.WriteField("C4_2");
			csv.WriteField("D");
			csv.WriteField("D_1");
			csv.WriteField("D_2");
			csv.WriteField("EPD_Header_ID");
			csv.WriteField("ID");
			csv.WriteField("Indicator");
			csv.WriteField("PhaseUnit");
			csv.WriteField("FunctionalUnit");
			csv.WriteField("FunctionalUnitAmount");
			csv.WriteField("ISO14025Certified");
			csv.WriteField("ISO14040Certified");
			csv.WriteField("ISO14044Certified");
			csv.WriteField("PdfAppxDate");
			csv.WriteField("PdfAppxName");
			csv.WriteField("PdfDate");
			csv.WriteField("PdfID");
			csv.WriteField("PdfName");
			csv.WriteField("ServiceLifeAmount");
			csv.WriteField("ValidFrom");
			csv.WriteField("ValidTo");
			csv.NextRecord();

			foreach (CertificationChange change in changes)
			{
				foreach (var epd in change.ePDs)
				{
					csv.WriteField(change.CompanyName);
					csv.WriteField(change.SupplierNr);
					csv.WriteField(change.DBNr);
					csv.WriteField(change.ProductGroupId);
					csv.WriteField(change.ProductText);
					csv.WriteField(epd.ConversionFactor);
					csv.WriteField(epd.CreationDate);
					csv.WriteField(epd.EN15084ACertification);
					csv.WriteField(epd.EPDType);
					csv.WriteField(epd.EPDIndicatorLines?.A1);
					csv.WriteField(epd.EPDIndicatorLines?.A1A3);
					csv.WriteField(epd.EPDIndicatorLines?.A2);
					csv.WriteField(epd.EPDIndicatorLines?.A3);
					csv.WriteField(epd.EPDIndicatorLines?.A4);
					csv.WriteField(epd.EPDIndicatorLines?.A5);
					csv.WriteField(epd.EPDIndicatorLines?.B1);
					csv.WriteField(epd.EPDIndicatorLines?.B1B7);
					csv.WriteField(epd.EPDIndicatorLines?.B1C1);
					csv.WriteField(epd.EPDIndicatorLines?.B2);
					csv.WriteField(epd.EPDIndicatorLines?.B2B7);
					csv.WriteField(epd.EPDIndicatorLines?.B3);
					csv.WriteField(epd.EPDIndicatorLines?.B3B7);
					csv.WriteField(epd.EPDIndicatorLines?.B4);
					csv.WriteField(epd.EPDIndicatorLines?.B5);
					csv.WriteField(epd.EPDIndicatorLines?.B6);
					csv.WriteField(epd.EPDIndicatorLines?.B7);
					csv.WriteField(epd.EPDIndicatorLines?.C1);
					csv.WriteField(epd.EPDIndicatorLines?.C2);
					csv.WriteField(epd.EPDIndicatorLines?.C3);
					csv.WriteField(epd.EPDIndicatorLines?.C3_1);
					csv.WriteField(epd.EPDIndicatorLines?.C3_2);
					csv.WriteField(epd.EPDIndicatorLines?.C4);
					csv.WriteField(epd.EPDIndicatorLines?.C4_1);
					csv.WriteField(epd.EPDIndicatorLines?.C4_2);
					csv.WriteField(epd.EPDIndicatorLines?.D);
					csv.WriteField(epd.EPDIndicatorLines?.D_1);
					csv.WriteField(epd.EPDIndicatorLines?.D_2);
					csv.WriteField(epd.EPDIndicatorLines?.EPDHeaderId);
					csv.WriteField(epd.EPDIndicatorLines?.Id);
					csv.WriteField(epd.EPDIndicatorLines?.Indicator);
					csv.WriteField(epd.EPDIndicatorLines?.PhaseUnit);
					csv.WriteField(epd.FunctionalUnit);
					csv.WriteField(epd.FunctionalUnitAmount);
					csv.WriteField(epd.ISO14025Certified);
					csv.WriteField(epd.ISO14040Certified);
					csv.WriteField(epd.ISO14044Certified);
					csv.WriteField(epd.PdfAppxDate);
					csv.WriteField(epd.PdfAppxName);
					csv.WriteField(epd.PdfDate);
					csv.WriteField(epd.PdfId);
					csv.WriteField(epd.PdfName);
					csv.WriteField(epd.ServiceLifeAmount);
					csv.WriteField(epd.ValidFrom);
					csv.WriteField(epd.ValidTo);
					csv.NextRecord();
				}
			}
		}
		catch (Exception)
		{
			return Result.Failure<ICollection<string>
				>(Error.NullValue);
		}
		return Result.Success(files);
	}

}
