namespace CertificateUpdater.Domain.RequestBodies;
public sealed class GetKatalogChangesBody
{
	public TunUser tunuser { get; set; } = new TunUser();
	public string fromDate { get; set; } = string.Empty;
}
