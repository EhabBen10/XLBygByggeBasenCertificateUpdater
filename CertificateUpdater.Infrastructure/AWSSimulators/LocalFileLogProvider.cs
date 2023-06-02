using System.Globalization;
using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.AWSSimulators;
public sealed class LocalFileLogProvider : ILogProvider
{
	public string FileName { get; set; }
	public LocalFileLogProvider(string fileName)
	{
		FileName = fileName;
	}
	public ICollection<string> GetAllLogDates()
	{
		ICollection<string> logDates = new List<string>();
		using (StreamReader sr = new(FileName))
		{
			string? text = sr.ReadToEnd();
			if (!string.IsNullOrWhiteSpace(text))
			{
				string[] lines = text.Split(Environment.NewLine);

				foreach (var line in lines)
				{
					string format = "yyyy-MM-ddTH:mm:ss.fffZ";
					string date = "/Date(" + DateTime.ParseExact(line, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal) + ")/";
					logDates.Add(date);
				}
			}
		}
		return logDates;
	}

	public string GetLastLog()
	{
		ICollection<string> logDates = new List<string>();
		using (StreamReader sr = new(FileName))
		{
			string? text = sr.ReadToEnd();
			if (!string.IsNullOrWhiteSpace(text))
			{
				string[] lines = text.Split(Environment.NewLine);

				foreach (var line in lines)
				{
					string format = "yyyy-MM-ddThh:mm:ss.fffZ";
					string unix = Convert.ToString(((DateTimeOffset)DateTime.ParseExact(line, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)).ToUnixTimeMilliseconds());
					string date = "/Date(" + unix + ")/";
					logDates.Add(date);
				}
			}
		}
		return logDates.Last();
	}
}
