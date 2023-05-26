using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Domain.RequestBodies;
public sealed class PostChangeBatchBody
{
	public List<CertificationChange> Changes { get; set; } = new List<CertificationChange>();
}
