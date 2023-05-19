namespace CertificateUpdater.Services.Settings;
public sealed class ByggeBasenSettings : BaseSettings
{
	public int TunUserNr { get; set; }
	public string UserName { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}
