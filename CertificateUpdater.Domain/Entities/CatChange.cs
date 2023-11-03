namespace CertificateUpdater.Domain.Entities;
public sealed record CatChange
{
	public int EmneId { get; set; }
	public int Tunnr { get; set; }
	public DateTime CreatedAt { get; set; }
}
