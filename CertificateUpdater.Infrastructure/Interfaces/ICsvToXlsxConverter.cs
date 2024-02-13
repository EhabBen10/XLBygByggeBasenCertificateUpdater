namespace CertificateUpdater.Services.Interfaces;
public interface ICsvToXlsxConverter
{
	void ConvertToXlsx(string basePath, ICollection<string> csvFilePath);
}
