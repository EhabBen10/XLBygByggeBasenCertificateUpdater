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
		if (dateString.Contains("T") && !dateString.Contains(":"))
		{
			string firstpart = dateString.Substring(0, 18);
			string secoundpart = dateString.Substring(18);

			// Replace periods in the time part with colons (only for hours, minutes, seconds)
			firstpart = firstpart.Replace('.', ':');

			// Combine the date and adjusted time part
			dateString = firstpart + secoundpart;
		}
		return DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
	}

	public string ParseToUnix(string dateString, string format)
	{
		if (dateString.Contains("T") && !dateString.Contains(":"))
		{
			string firstpart = dateString.Substring(0, 18);
			string secoundpart = dateString.Substring(18);

			// Replace periods in the time part with colons (only for hours, minutes, seconds)
			firstpart = firstpart.Replace('.', ':');

			// Combine the date and adjusted time part
			dateString = firstpart + secoundpart;
		}
		return Convert.ToString(((DateTimeOffset)DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)).AddDays(-1).ToUnixTimeMilliseconds());
	}
}
