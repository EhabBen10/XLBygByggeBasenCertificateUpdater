namespace CertificateUpdater.Services.Interfaces;
public interface ILogProvider
{
	public ICollection<DateTime> GetAllLogDates();
	public DateTime GetLastLog();

}
