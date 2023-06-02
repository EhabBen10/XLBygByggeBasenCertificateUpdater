using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Domain.RequestBodies;
public sealed record PostChangeBatchBody
{
	public ICollection<CertificationChange> Changes { get; set; } = new List<CertificationChange>();
}
