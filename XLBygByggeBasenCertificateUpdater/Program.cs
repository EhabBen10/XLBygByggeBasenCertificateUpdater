using CertificateUpdater.Services.AWSSimulators;
using CertificateUpdater.Services.Finders;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Providers;
using CertificateUpdater.Services.RestSharp;
using CertificateUpdater.Services.Services;
using CertificateUpdater.Services.Settings;
using Microsoft.Extensions.Options;


var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
#if DEBUG
string basePath = "C:\\Users\\AME\\OneDrive - XL-BYG a.m.b.a\\Vigtige filer\\ByggebasenTest\\";
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
#else
string basePath = "O:\\IT\\EG-FIT fællesdrev\\Dokumentation\\Intern\\Varevedligehold\\Bæredygtige varer\\Byg-e udtræk\\";
IDateTimeProvider dateTimeProvider = new DateTimeProvider();
ILogProvider logProvider = new LocalFileLogProvider(basePath + "AWSSimulator\\RunLog.txt", dateTimeProvider);
ICredentialProvider credentialProvider = new LocalFileCredentialProvider(basePath + "AWSSimulator\\TunUserNr.txt", basePath + "AWSSimulator\\UserName.txt",
basePath + "AWSSimulator\\Password.txt", basePath + "AWSSimulator\\Aspect4Username.txt", basePath + "Aspect4Password.txt");
BaseSettings settings = new ByggeBasenSettings()
{
	BaseUrl = "http://services.byggebasen.dk/V3/BBService.svc/",
	Password = credentialProvider.GetPassword(),
	UserName = credentialProvider.GetUserName(),
	TunUserNr = credentialProvider.GetTunUserNr(),
};
#endif

IOptions<BaseSettings> byggeBasenSettings = Options.Create(settings);
IClient<BaseSettings> client = new RestSharpClient<BaseSettings>(byggeBasenSettings);
ICertificationChangeFinder certificationChangeFinder = new CertificationChangeFinder();
IGetProductChangesService getKatalogChangesService = new GetProductChangesService(client, logProvider, credentialProvider);
IGetProductBatchService getProductBatchService = new GetProductBatchService(client, credentialProvider);
ICSVFileCreator cSVFileCreator = new CSVFileCreator();
ICsvToXlsxConverter csvToXlsxConverter = new CsvToXlsxConverter();
//IPostChangesService postChangesService = new PostChangesService(client, credentialProvider);
Console.WriteLine("(1/5) Getting all changes");
var productchangesResult = await getKatalogChangesService.GetProductChanges(default);
if (productchangesResult.IsFailure)
{
	Console.WriteLine(productchangesResult.Error.Message);
	Thread.Sleep(10000);
	return;
}

Console.WriteLine("(2/5) Getting all changed products");
var productResult = await getProductBatchService.GetProductBatch(productchangesResult.Value, default);
if (productResult.IsFailure)
{
	Console.WriteLine(productResult.Error.Message);
	Thread.Sleep(10000);
	return;
}
Console.WriteLine("(3/5) Finding changes to certification");
var certificationResult = certificationChangeFinder.FindCertificationChanges(productResult.Value);
Console.WriteLine("(4/5) Creating Csv File");
var csvResult = cSVFileCreator.CreateCSVFiles(basePath, certificationResult.ToList());
if (csvResult.IsFailure)
{
	Console.WriteLine(csvResult.Error.Message);
	Thread.Sleep(20000);
	return;
}
csvToXlsxConverter.ConvertToXlsx(csvResult.Value);
Console.WriteLine("(5/5) Updating RunLog");
logProvider.UpdateLog();
Console.WriteLine("End");
Thread.Sleep(10000);
