using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Domain.RequestBodies;
public sealed record PostChangeBatchBody
{
	public List<CertificationChange> Changes { get; set; } = new();
}
