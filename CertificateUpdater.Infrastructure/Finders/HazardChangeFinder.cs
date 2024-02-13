using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.Finders;
public class HazardChangeFinder : IHazardChangeFinder
{
	public ICollection<HazardInfo> FindHazardChanges(ICollection<Product> products)
	{
		ICollection<HazardInfo> hazardInfos = new List<HazardInfo>();
		foreach (var product in products)
		{
			if (product.HazardInfo != null)
			{
				hazardInfos.Add(product.HazardInfo);
			}
		}
		return hazardInfos;
	}
}
