using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.AWSSimulators;
public sealed class LocalFileLogProvider : ILogProvider
{
	public string FileName { get; set; }
	public IDateTimeProvider DateTimeProvider { get; set; }
	public LocalFileLogProvider(string fileName, IDateTimeProvider dateTimeProvider)
	{
		FileName = fileName;
		DateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
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
					string currentLine = line.Replace(",", "");
					string format = "yyyy-MM-ddTHH:mm:ss.fffZ";
					string unix = DateTimeProvider.ParseToUnix(currentLine, format);
					string date = "/Date(" + unix + ")/";
					logDates.Add(date);
				}
			}
		}
		return logDates;
	}

	public string GetLastLog()
	{
		ICollection<string> logDates = new List<string>();
		using StreamReader sr = new(FileName);
		string? text = sr.ReadToEnd();
		if (!string.IsNullOrWhiteSpace(text))
		{
			string[] lines = text.Split(Environment.NewLine);

			foreach (var line in lines)
			{
				string currentLine = line.Replace(",", "");
				string format = "yyyy-MM-ddTHH:mm:ss.fffZ";
				string unix = DateTimeProvider.ParseToUnix(currentLine, format);
				string date = "/Date(" + unix + ")/";
				logDates.Add(date);
			}
		}
		return logDates.Last();
	}

	private DateTime GetLastLogDateTime()
	{
		ICollection<DateTime> logDates = new List<DateTime>();
		using StreamReader sr = new(FileName);
		string? text = sr.ReadToEnd();
		if (!string.IsNullOrWhiteSpace(text))
		{
			string[] lines = text.Split(Environment.NewLine);

			foreach (var line in lines)
			{
				string currentLine = line.Replace(",", "");
				string format = "yyyy-MM-ddTHH:mm:ss.fffZ";
				DateTime date = DateTimeProvider.ParseToDateTime(currentLine, format);
				logDates.Add(date);
			}
		}
		return logDates.Last();
	}
	public void UpdateLog()
	{
		DateTime lastRun = GetLastLogDateTime();
		DateTime newestRun = DateTimeProvider.GetNow();
		string format = "yyyy-MM-ddTHH:mm:ss.fffZ";
		using StreamWriter sw = new(FileName, true);
		if (lastRun.Date != newestRun.Date)
		{
			sw.Write(",\r\n" + newestRun.ToString(format));
		}
	}
}
