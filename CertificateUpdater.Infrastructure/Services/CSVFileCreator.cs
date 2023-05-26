using System.Text;
using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.Services;
public sealed class CSVFileCreator : ICSVFileCreator
{
	public void CreateCSVFiles(List<CertificationChange> changes)
	{
		string file = @"C:\Users\AME\OneDrive - XL-BYG a.m.b.a\Documents\Byggebasen\ResultCSV\MyTest.csv";
		string separator = ",";
		StringBuilder output = new StringBuilder();
		string[] headings = { "Firmanavn",  "Leverandørnr",    "DB nr.", "Varetekst 1", "DGNB", "Svanemærke",  "Svanemærket byggeri", "BREEAM",
			"LEED", "Cradle to Cradle", "Der Blaue Engel", "Certificeret træ PEFC - FSC", "Certificeret træ PEFC", "Indeklimamærkning",
			"Svanemærke / EU - Blomsten", "Astma - og Allergi Danmark",   "EPD – Miljøvaredeklaration", "Allergy UK" };
		output.AppendLine(string.Join(separator, headings));

		foreach (CertificationChange change in changes)
		{
			string[] newLine = { change.CompanyName, Convert.ToString(change.SupplierNr), Convert.ToString(change.DBNr),change.ProductText, Convert.ToString(change.hasDGNB), Convert.ToString(change.hasSvanemærke),
				Convert.ToString(change.hasSvanemærkeByggeri), Convert.ToString(change.hasBREEAM), Convert.ToString(change.hasLEED),Convert.ToString(change.hasC2C), Convert.ToString(change.hasDBE),
				Convert.ToString(change.hasFSC), Convert.ToString(change.hasPEFC),Convert.ToString(change.hasIndeKlima),Convert.ToString(change.hasEUBlomst),Convert.ToString(change.hasAstmaOgAllergi),
				Convert.ToString(change.hasEPD), Convert.ToString(change.hasALUK)};
			output.AppendLine(string.Join(separator, newLine));
		}

		try
		{
			File.AppendAllText(file, output.ToString());
		}
		catch (Exception)
		{
			Console.WriteLine("Data could not be written to the CSV file.");
			return;
		}
	}
}
