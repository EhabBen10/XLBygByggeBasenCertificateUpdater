using CertificateUpdater.Services.Interfaces;
using OfficeOpenXml;

namespace CertificateUpdater.Services.Services;
public sealed class CsvToXlsxConverter : ICsvToXlsxConverter
{
	public void ConvertToXlsx(string csvFilePath)
	{
		string file = @"O:\IT\EG-FIT fællesdrev\Dokumentation\Intern\Varevedligehold\Bæredygtige varer\Byg-e udtræk\ResultCSV\CertificationUpdates" + DateTime.Now.ToShortDateString() + ".xlsx";
		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
		using (ExcelPackage package = new ExcelPackage())
		{
			// Create a new worksheet
			ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

			// Read the CSV file
			string[] csvLines = File.ReadAllLines(csvFilePath);

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
			package.SaveAs(new FileInfo(file));
		}
		File.Delete(csvFilePath);
	}
}
