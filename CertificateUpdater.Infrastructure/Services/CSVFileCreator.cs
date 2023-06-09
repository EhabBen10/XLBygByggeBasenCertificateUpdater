using System.Text;
using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;
using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.Services;
public sealed class CSVFileCreator : ICSVFileCreator
{
	public Result CreateCSVFiles(List<CertificationChange> changes)
	{
		string file = @"C:\Users\AME\OneDrive - XL-BYG a.m.b.a\Documents\Byggebasen\ResultCSV\CertificationUpdates" + DateTime.Now.ToShortDateString() + ".csv";
		string separator = ";";
		StringBuilder output = new();
		string[] headings = { "Firmanavn",  "Leverandørnr", "DB nr.", "Varetekst 1", "DGNB", "Svanemærke",  "Svanemærket byggeri", "BREEAM",
			"LEED", "Cradle to Cradle", "Der Blaue Engel", "Certificeret træ PEFC - FSC", "Certificeret træ PEFC", "Indeklimamærkning",
			"Svanemærke / EU - Blomsten", "Astma - og Allergi Danmark",   "EPD – Miljøvaredeklaration", "Allergy UK" };
		output.AppendLine("sep=;");
		output.AppendLine(string.Join(separator, headings));

		foreach (CertificationChange change in changes)
		{
			string[] newLine = { change.CompanyName, change.SupplierNr, Convert.ToString(Convert.ToInt32(change.DBNr)),change.ProductText, Convert.ToString(Convert.ToInt32(change.hasDGNB)), Convert.ToString(Convert.ToInt32(change.hasSvanemærke)),
				Convert.ToString(Convert.ToInt32(change.hasSvanemærkeByggeri)), Convert.ToString(Convert.ToInt32(change.hasBREEAM)), Convert.ToString(Convert.ToInt32(change.hasLEED)),Convert.ToString(Convert.ToInt32(change.hasC2C)), Convert.ToString(Convert.ToInt32(change.hasDBE)),
				Convert.ToString(Convert.ToInt32(change.hasFSC)), Convert.ToString(Convert.ToInt32(change.hasPEFC)),Convert.ToString(Convert.ToInt32(change.hasIndeKlima)),Convert.ToString(Convert.ToInt32(change.hasEUBlomst)),Convert.ToString(Convert.ToInt32(change.hasAstmaOgAllergi)),
				Convert.ToString(Convert.ToInt32(change.hasEPD)), Convert.ToString(Convert.ToInt32(change.hasALUK))};
			output.AppendLine(string.Join(separator, newLine));
		}

		try
		{
			using StreamWriter writer = new(file, true, Encoding.UTF8);
			writer.Write(output.ToString());
			return Result.Success();
		}
		catch (Exception)
		{
			return Result.Failure(Error.NullValue);
		}
	}
}
