using CertificateUpdater.Services.Interfaces;
using OfficeOpenXml;

namespace CertificateUpdater.Services.Services;
public sealed class CsvToXlsxConverter : ICsvToXlsxConverter
{
	public void ConvertToXlsx(string basePath, ICollection<string> csvFilePath)
	{

		string certificationFile = @$"{basePath}\ResultCSV\CertificationUpdates" + DateTime.Now.ToShortDateString() + ".xlsx";
		string epdFile = @$"{basePath}\ResultCSV\EPDUpdates" + DateTime.Now.ToShortDateString() + ".xlsx";
		string hazardFile = @$"{basePath}\ResultCSV\HazardUpdates" + DateTime.Now.ToShortDateString() + ".xlsx";

		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		foreach (var path in csvFilePath)
		{
			using (ExcelPackage package = new())
			{
				// Create a new worksheet
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

				// Read the CSV file
				string[] csvLines = File.ReadAllLines(path);

				// Write the CSV data to the worksheet
				int row = 1;
				foreach (string csvLine in csvLines)
				{
					string[] csvValues = csvLine.Split(';');
					for (int column = 1; column <= csvValues.Length; column++)
					{
						worksheet.Cells[row, column].Value = csvValues[column - 1];
					}
					row++;
				}

				// Save the Excel package to a file
				if (path.Contains("EPDUpdates"))
					package.SaveAs(new FileInfo(epdFile));
				else if (path.Contains("CertificationUpdates"))
					package.SaveAs(new FileInfo(certificationFile));
				else if (path.Contains("HazardUpdates"))
					package.SaveAs(new FileInfo(hazardFile));
				else
					throw new FileNotFoundException(nameof(path));
			}
			File.Delete(path);
		}
	}
}
