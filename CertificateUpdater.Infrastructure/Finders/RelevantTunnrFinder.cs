using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Enum;
using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.Finders;
public class RelevantTunnrFinder : IRelevantTunnrFinder
{
	public ICollection<int> FindRelevantTunnrs(ICollection<CatChange> catChanges)
	{
		ICollection<int> result = new List<int>();
		foreach (var catChange in catChanges)
		{
			if (Enum.IsDefined(typeof(CertificationEnum), catChange.EmneId))
			{
				if (!result.Contains(catChange.Tunnr))
				{
					result.Add(catChange.Tunnr);
				}
			}
		}
		return result;
	}
}
