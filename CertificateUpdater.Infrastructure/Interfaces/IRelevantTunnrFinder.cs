using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Services.Interfaces;
public interface IRelevantTunnrFinder
{
	public ICollection<int> FindRelevantTunnrs(ICollection<CatChange> catChanges);
}
