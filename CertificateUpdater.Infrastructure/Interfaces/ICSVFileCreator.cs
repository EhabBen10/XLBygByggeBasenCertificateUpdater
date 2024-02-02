using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;

namespace CertificateUpdater.Services.Interfaces;
public interface ICSVFileCreator
{
	public Result<ICollection<string>> CreateCSVFiles(string basePath, List<CertificationChange> changes);


}
