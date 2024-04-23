using CertificateUpdater.Services.Interfaces;
using OfficeOpenXml;

namespace CertificateUpdater.Services.Services;

public sealed class CsvToXlsxConverter : ICsvToXlsxConverter
{
	public void ConvertToXlsx(string basePath, ICollection<string> csvFilePaths)
	{
		string currentDate = DateTime.Now.ToShortDateString();
		string certificationBaseFile = @$"{basePath}\ResultCSV\CertificationUpdates_{currentDate}";
		string epdBaseFile = @$"{basePath}\ResultCSV\EPDUpdates_{currentDate}";
		string hazardBaseFile = @$"{basePath}\ResultCSV\HazardUpdates_{currentDate}";

		ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

		foreach (var path in csvFilePaths)
		{
			string baseFile;
			if (path.Contains("EPDUpdates"))
				baseFile = epdBaseFile;
			else if (path.Contains("CertificationUpdates"))
				baseFile = certificationBaseFile;
			else if (path.Contains("HazardUpdates"))
				baseFile = hazardBaseFile;
			else
				throw new FileNotFoundException(nameof(path));

			using (ExcelPackage package = new ExcelPackage())
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

				string[] csvLines = File.ReadAllLines(path);

				int maxRowsPerFile = 1000000; // Excel limit is typically around 1 million rows
				int totalRowsWritten = 0;
				int fileIndex = 1;

				foreach (string csvLine in csvLines)
				{
					string[] csvValues = csvLine.Split(';');
					for (int column = 1; column <= csvValues.Length; column++)
					{
						worksheet.Cells[totalRowsWritten + 1, column].Value = csvValues[column - 1];
					}

					totalRowsWritten++;

					// If we reached the maximum rows per file, save the current package and create a new one
					if (totalRowsWritten == maxRowsPerFile)
					{
						string filename = $"{baseFile}_{fileIndex}.xlsx";
						package.SaveAs(new FileInfo(filename));

						fileIndex++;
						totalRowsWritten = 0;

						package.Workbook.Worksheets.Add($"Sheet{fileIndex}");
						worksheet = package.Workbook.Worksheets[$"Sheet{fileIndex}"];
					}
				}

				// Save the remaining data to the last file
				if (totalRowsWritten > 0)
				{
					string filename = $"{baseFile}_{fileIndex}.xlsx";
					package.SaveAs(new FileInfo(filename));
				}
				package.Dispose();

			}
			File.Delete(path);
		}
	}
}
