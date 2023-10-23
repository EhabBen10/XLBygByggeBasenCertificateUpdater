namespace CertificateUpdater.Services.Interfaces;
public interface ICsvToXlsxConverter
{
	void ConvertToXlsx(ICollection<string> csvFilePath);
}
