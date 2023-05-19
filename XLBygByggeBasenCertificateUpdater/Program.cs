using CertificateUpdater.Services.AWSSimulators;
using CertificateUpdater.Services.Finders;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.RestSharp;
using CertificateUpdater.Services.Services;
using CertificateUpdater.Services.Settings;
using Microsoft.Extensions.Options;

string basePath = "C:\\Users\\AME\\OneDrive - XL-BYG a.m.b.a\\Documents\\Byggebasen\\AWSSimulator\\";
ILogProvider logProvider = new LocalFileLogProvider(basePath + "RunLog.txt");
ICredentialProvider credentialProvider = new LocalFileCredentialProvider(basePath + "TunUserNr.txt", basePath + "UserName.txt", basePath + "Password.txt");
BaseSettings settings = new ByggeBasenSettings()
{
	BaseUrl = "http://servicetest.byggebasen.com/EPDTEST/BBService.svc/",
	Password = credentialProvider.GetPassword(),
	UserName = credentialProvider.GetUserName(),
	TunUserNr = credentialProvider.GetTunUserNr(),
};
IOptions<BaseSettings> byggeBasenSettings = Options.Create(settings);
IClient<BaseSettings> client = new RestSharpClient<BaseSettings>(byggeBasenSettings);
IRelevantTunnrFinder relevantTunnrFinder = new RelevantTunnrFinder();

GetKatalogChangesService getKatalogChangesService = new GetKatalogChangesService(client, logProvider, credentialProvider);
var result = await getKatalogChangesService.GetKatalogChanges(default);
var tunnr = relevantTunnrFinder.FindRelevantTunnrs(result);
Console.WriteLine("End");
