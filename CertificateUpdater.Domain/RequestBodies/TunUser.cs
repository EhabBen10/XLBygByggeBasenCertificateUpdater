namespace CertificateUpdater.Domain.RequestBodies;
public sealed class TunUser
{
	public int TunUserNr { get; set; }
	public string UserName { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}
