namespace CertificateUpdater.Services.Extensions;
public static class StringExtensions
{
	public static DateTime UnixTimestampWithOffsetToDateTime(this string str)
	{
		string dateString = str;

		// Extract the timestamp and offset from the string
		int start = dateString.IndexOf('(') + 1;
		int end = dateString.IndexOf('+');
		long timestamp = long.Parse(dateString[start..end]);

		start = end + 1;
		end = dateString.IndexOf(')') - 2;
		int offsetHours = int.Parse(dateString[start..end]);

		// Calculate the DateTime
		DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
		// Adjust the DateTime with the offset
		return dateTime.AddHours(offsetHours);
	}
}
