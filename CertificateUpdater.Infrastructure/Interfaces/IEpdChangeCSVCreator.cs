using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;

namespace CertificateUpdater.Services.Interfaces;
public interface IEpdChangeCSVCreator
{
	public Result<string> CreateEPDCSVFile(string basePath, List<EPD> epds);

}
