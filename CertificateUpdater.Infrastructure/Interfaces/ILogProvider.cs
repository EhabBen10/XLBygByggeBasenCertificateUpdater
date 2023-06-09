namespace CertificateUpdater.Services.Interfaces;
public interface ILogProvider
{
	public ICollection<string> GetAllLogDates();
	public string GetLastLog();
	public void UpdateLog();
}
