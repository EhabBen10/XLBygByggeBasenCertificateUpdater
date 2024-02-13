using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;

namespace CertificateUpdater.Services.Interfaces;
public interface IHazardChangeCSVCreator
{
	public Result<string> CreateHazardCSVFile(string basePath, List<HazardInfo> hazardInfos);
}
