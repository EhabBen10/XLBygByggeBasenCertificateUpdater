namespace CertificateUpdater.Services.Interfaces;
public interface IDateTimeProvider
{
	public DateTime GetNow();
	public DateTime GetNowMinus1Hour();
	public string GetNowShortFormat();
	public string ParseToUnix(string dateString, string format);
}
