namespace CertificateUpdater.Domain.RequestBodies;
public sealed record getProduktBatchBody
{
	public TunUser tunUser { get; set; } = new TunUser();
	public ICollection<int> tunnr { get; set; } = new List<int>();
}
