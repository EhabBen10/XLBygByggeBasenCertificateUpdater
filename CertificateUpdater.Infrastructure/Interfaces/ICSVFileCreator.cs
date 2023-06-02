using CertificateUpdater.Domain.Entities;

namespace CertificateUpdater.Services.Interfaces;
public interface ICSVFileCreator
{
	public void CreateCSVFiles(List<CertificationChange> changes);
}
