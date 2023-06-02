using CertificateUpdater.Services.AWSSimulators;
using CertificateUpdater.Services.Finders;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.RestSharp;
using CertificateUpdater.Services.Services;
using CertificateUpdater.Services.Settings;
using Microsoft.Extensions.Options;

string basePath = "C:\\Users\\AME\\OneDrive - XL-BYG a.m.b.a\\Documents\\Byggebasen\\AWSSimulator\\";
ILogProvider logProvider = new LocalFileLogProvider(basePath + "RunLog.txt");
ICredentialProvider credentialProvider = new LocalFileCredentialProvider(basePath + "TunUserNr.txt", basePath + "UserName.txt",
	basePath + "Password.txt", basePath + "Aspect4Username.txt", basePath + "Aspect4Password.txt");
BaseSettings settings = new ByggeBasenSettings()
{
	BaseUrl = "http://services.byggebasen.dk/V3/BBService.svc/",
	Password = credentialProvider.GetPassword(),
	UserName = credentialProvider.GetUserName(),
	TunUserNr = credentialProvider.GetTunUserNr(),
};
IOptions<BaseSettings> byggeBasenSettings = Options.Create(settings);
IClient<BaseSettings> client = new RestSharpClient<BaseSettings>(byggeBasenSettings);
IRelevantTunnrFinder relevantTunnrFinder = new RelevantTunnrFinder();
ICertificationChangeFinder certificationChangeFinder = new CertificationChangeFinder();


IGetKatalogChangesService getKatalogChangesService = new GetKatalogChangesService(client, logProvider, credentialProvider);
IGetProductBatchService getProductBatchService = new GetProductBatchService(client, credentialProvider);
ICSVFileCreator cSVFileCreator = new CSVFileCreator();
//IPostChangesService postChangesService = new PostChangesService(client, credentialProvider);

var catChangesResult = await getKatalogChangesService.GetKatalogChanges(default);
var tunnr = relevantTunnrFinder.FindRelevantTunnrs(catChangesResult);
var productResult = await getProductBatchService.GetProductBatch(tunnr, default);
var certificationResult = certificationChangeFinder.FindCertificationChanges(productResult);
cSVFileCreator.CreateCSVFiles(certificationResult.ToList());
Console.WriteLine("end");
