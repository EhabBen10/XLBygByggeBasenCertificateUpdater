using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;

namespace CertificateUpdater.Services.Interfaces;
public interface ICertificationChangeCSVCreator
{
	public Result<string> CreateCertificationChangeCSVFile(string basePath, List<CertificationChange> changes);
}
