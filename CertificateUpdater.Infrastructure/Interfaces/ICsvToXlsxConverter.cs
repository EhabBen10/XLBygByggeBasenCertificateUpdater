namespace CertificateUpdater.Services.Interfaces;
public interface ICsvToXlsxConverter
{
	void ConvertToXlsx(string csvFilePath);
}
