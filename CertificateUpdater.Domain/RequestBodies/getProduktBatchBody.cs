namespace CertificateUpdater.Domain.RequestBodies;
public sealed class getProduktBatchBody
{
	public TunUser tunUser { get; set; } = new TunUser();
	public ICollection<int> tunnr { get; set; } = new List<int>();
}
