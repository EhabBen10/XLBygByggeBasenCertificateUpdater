using System.Globalization;
using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.Providers;
public class DateTimeProvider : IDateTimeProvider
{
	public DateTime GetNow()
	{
		return DateTime.Now;
	}

	public string GetNowShortFormat()
	{
		return DateTime.Now.ToShortDateString();
	}

	public DateTime ParseToDateTime(string dateString, string format)
	{
		return DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
	}

	public string ParseToUnix(string dateString, string format)
	{
		return Convert.ToString(((DateTimeOffset)DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)).AddDays(-1).ToUnixTimeMilliseconds());
	}
}
