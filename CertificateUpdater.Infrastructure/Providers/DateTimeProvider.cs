using System.Globalization;
using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.Providers;
public class DateTimeProvider : IDateTimeProvider
{
	public DateTime GetNow()
	{
		return DateTime.Now;
	}

	public DateTime GetNowMinus1Hour()
	{
		return DateTime.Now.AddHours(-1);
	}

	public string GetNowShortFormat()
	{
		return DateTime.Now.ToShortDateString();
	}
	public string ParseToUnix(string dateString, string format)
	{
		return Convert.ToString(((DateTimeOffset)DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)).ToUnixTimeMilliseconds());
	}
}
