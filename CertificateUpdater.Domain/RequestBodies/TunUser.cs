namespace CertificateUpdater.Domain.RequestBodies;
public sealed record TunUser
{
	public int TunUserNr { get; set; }
	public string UserName { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
}
