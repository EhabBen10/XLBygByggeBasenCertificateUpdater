using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;

namespace CertificateUpdater.Services.Interfaces;
public interface ICSVFileCreator
{
	public Result CreateCSVFiles(List<CertificationChange> changes);
}
