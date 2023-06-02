namespace CertificateUpdater.Domain.Entities;
public sealed record Katalog
{
	public int EmneId { get; set; }
	public bool isValid { get; set; }
	public int Tunnr { get; set; }
}
