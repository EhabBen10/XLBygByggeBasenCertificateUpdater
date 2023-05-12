using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.Services;
public class LogProviderDummy : ILogProvider
{
	public ICollection<DateTime> GetAllLogDates()
	{
		return new List<DateTime>(){
			DateTime.Now,
			DateTime.Now.AddHours(-20),
		};
	}

	public DateTime GetLastLog()
	{
		return DateTime.Now.AddHours(-20);
	}
}
