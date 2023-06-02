namespace CertificateUpdater.Domain.RequestBodies;
public sealed record GetKatalogChangesBody
{
	public TunUser tunuser { get; set; } = new TunUser();
	public string fromDate { get; set; } = string.Empty;
}
