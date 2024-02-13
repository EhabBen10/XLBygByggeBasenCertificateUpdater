using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.Finders;
public class EPDChangeFinder : IEPDChangeFinder
{
	public ICollection<EPD> FindEPDChanges(ICollection<Product> products)
	{
		ICollection<EPD> epDChanges = new List<EPD>();
		foreach (var product in products)
		{
			EPD? latestEPD = product.EPDs?.OrderByDescending(e => e.CreationDate).FirstOrDefault();

			if (latestEPD != null)
			{
				epDChanges.Add(latestEPD);
			}
		}
		return epDChanges;
	}
}
