using CertificateUpdater.Services.AWSSimulators;
using CertificateUpdater.Services.Finders;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Providers;
using CertificateUpdater.Services.RestSharp;
using CertificateUpdater.Services.Services;
using CertificateUpdater.Services.Settings;
using Microsoft.Extensions.Options;

string basePath = "O:\\IT\\EG-FIT fællesdrev\\Dokumentation\\Intern\\Varevedligehold\\Bæredygtige varer\\Byg-e udtræk\\AWSSimulator\\";
IDateTimeProvider dateTimeProvider = new DateTimeProvider();
ILogProvider logProvider = new LocalFileLogProvider(basePath + "RunLog.txt", dateTimeProvider);
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
ICsvToXlsxConverter csvToXlsxConverter = new CsvToXlsxConverter();
//IPostChangesService postChangesService = new PostChangesService(client, credentialProvider);
Console.WriteLine("(1/7) Getting all changes to product katalogs");
var catChangesResult = await getKatalogChangesService.GetKatalogChanges(default);
if (catChangesResult.IsFailure)
{
	Console.WriteLine(catChangesResult.Error.Message);
	Thread.Sleep(10000);
	return;
}

Console.WriteLine("(2/7) Finding relevant changes to product katalogs");
var tunnr = relevantTunnrFinder.FindRelevantTunnrs(catChangesResult.Value);
Console.WriteLine("(3/7) Getting all changed products");
var productResult = await getProductBatchService.GetProductBatch(tunnr, default);
if (productResult.IsFailure)
{
	Console.WriteLine(productResult.Error.Message);
	Thread.Sleep(10000);
	return;
}
Console.WriteLine("(4/7) Finding changes to certification");
var certificationResult = certificationChangeFinder.FindCertificationChanges(productResult.Value);
Console.WriteLine("(5/7) Creating Csv File");
var csvResult = cSVFileCreator.CreateCSVFiles(certificationResult.ToList());
if (csvResult.IsFailure)
{
	Console.WriteLine(csvResult.Error.Message);
	Thread.Sleep(20000);
	return;
}
csvToXlsxConverter.ConvertToXlsx(csvResult.Value);
Console.WriteLine("(7/7) Updating RunLog");
logProvider.UpdateLog();
Console.WriteLine("End");
Thread.Sleep(10000);


