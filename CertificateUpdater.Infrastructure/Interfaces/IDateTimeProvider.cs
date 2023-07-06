namespace CertificateUpdater.Services.Interfaces;
public interface IDateTimeProvider
{
	public DateTime GetNow();
	public string GetNowShortFormat();
	public string ParseToUnix(string dateString, string format);
	public DateTime ParseToDateTime(string dateString, string format);
}
