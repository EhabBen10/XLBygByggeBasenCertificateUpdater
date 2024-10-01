namespace CertificateUpdater.Services.AWSSimulators;

public class LoggingToPowerAutomate
{
	private readonly string _logFilePath;
	public LoggingToPowerAutomate(string logFilePath)
	{
		_logFilePath = logFilePath;

		EnsureDirectoryExists();
		ClearLogFile();
	}

	public void Log(string message)
	{
		try
		{
			using (StreamWriter sw = new(_logFilePath, true))
			{
				sw.WriteLine(message);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error writing to log file: {ex.Message}");
		}
	}
	private void EnsureDirectoryExists()
	{
		if (!string.IsNullOrWhiteSpace(_logFilePath))
		{
			string? directory = Path.GetDirectoryName(_logFilePath);

			// Ensure directory is valid (not null)
			if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
		}

	}
	private void ClearLogFile()
	{
		try
		{
			if (File.Exists(_logFilePath))
			{
				// Overwrite the file with an empty string, clearing the content
				File.WriteAllText(_logFilePath, string.Empty);
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error clearing log file: {ex.Message}");
		}
	}
}
