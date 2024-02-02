namespace CertificateUpdater.Domain.RequestBodies;
public sealed record GetProductChangesBody
{
	public TunUser tunUser { get; set; } = new TunUser();
	public string date { get; set; } = string.Empty;
}
