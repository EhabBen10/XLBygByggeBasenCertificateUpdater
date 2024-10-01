using CertificateUpdater.Domain.Shared;
using CertificateUpdater.Services.AWSSimulators;
using CertificateUpdater.Services.Finders;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Providers;
using CertificateUpdater.Services.RestSharp;
using CertificateUpdater.Services.Services;
using CertificateUpdater.Services.Services.CSVFileCreators;
using CertificateUpdater.Services.Settings;
using Microsoft.Extensions.Options;

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
#if DEBUG
//string basePath = "C:\\Users\\AME\\OneDrive - XL-BYG a.m.b.a\\Vigtige filer\\ByggebasenTest\\";
string basePath = "\\\\lokal.ditas.dk\\data\\IT\\EG-FIT fællesdrev\\Dokumentation\\Intern\\Varevedligehold\\Bæredygtige varer\\Byg-e udtræk\\";
IDateTimeProvider dateTimeProvider = new DateTimeProvider();
ILogProvider logProvider = new LocalFileLogProvider(basePath + "AWSSimulator\\RunLog.txt", dateTimeProvider);
ICredentialProvider credentialProvider = new LocalFileCredentialProvider(basePath + "AWSSimulator\\TunUserNr.txt", basePath + "AWSSimulator\\UserName.txt",
	basePath + "AWSSimulator\\Password.txt", basePath + "AWSSimulator\\Aspect4Username.txt", basePath + "AWSSimulator\\Aspect4Password.txt");
LoggingToPowerAutomate loggingToPowerAutomate = new LoggingToPowerAutomate(basePath + "AWSSimulator\\AutomateLog.txt");

BaseSettings settings = new ByggeBasenSettings()
{
	BaseUrl = "http://services.byggebasen.dk/V3/BBService.svc/",
	Password = credentialProvider.GetPassword(),
	UserName = credentialProvider.GetUserName(),
	TunUserNr = credentialProvider.GetTunUserNr(),
};
#else
string basePath = "\\\\lokal.ditas.dk\\data\\IT\\EG-FIT fællesdrev\\Dokumentation\\Intern\\Varevedligehold\\Bæredygtige varer\\Byg-e udtræk\\";
IDateTimeProvider dateTimeProvider = new DateTimeProvider();
ILogProvider logProvider = new LocalFileLogProvider(basePath + "AWSSimulator\\RunLog.txt", dateTimeProvider);
ICredentialProvider credentialProvider = new LocalFileCredentialProvider(basePath + "AWSSimulator\\TunUserNr.txt", basePath + "AWSSimulator\\UserName.txt",
	basePath + "AWSSimulator\\Password.txt", basePath + "AWSSimulator\\Aspect4Username.txt", basePath + "AWSSimulator\\Aspect4Password.txt");
LoggingToPowerAutomate loggingToPowerAutomate = new LoggingToPowerAutomate(basePath + "AWSSimulator\\AutomateLog.txt");

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
IEPDChangeFinder ePDChangeFinder = new EPDChangeFinder();
IHazardChangeFinder hazardChangeFinder = new HazardChangeFinder();
IGetProductChangesService getKatalogChangesService = new GetProductChangesService(client, logProvider, credentialProvider);
IGetProductBatchService getProductBatchService = new GetProductBatchService(client, credentialProvider);
ICertificationChangeCSVCreator cSVFileCreator = new CertificationChangeCSVCreator();
IEpdChangeCSVCreator epdChangeCSVCreator = new EpdChangeCSVCreator();
IHazardChangeCSVCreator hazardChangeCSVCreator = new HazardChangeCSVCreator();
ICsvToXlsxConverter csvToXlsxConverter = new CsvToXlsxConverter();
//IPostChangesService postChangesService = new PostChangesService(client, credentialProvider);
Console.WriteLine("(1/5) Getting all changes");
loggingToPowerAutomate.Log("Getting all changes");
var productchangesResult = await getKatalogChangesService.GetProductChanges(default);
if (productchangesResult.IsFailure)
{
	Console.WriteLine(productchangesResult.Error.Message);
	loggingToPowerAutomate.Log(productchangesResult.Error.Message);
	Thread.Sleep(10000);
	return;
}

Console.WriteLine("(2/5) Getting all changed products");
loggingToPowerAutomate.Log("(2/5) Getting all changed products");
var productResult = await getProductBatchService.GetProductBatch(productchangesResult.Value, default);
if (productResult.IsFailure)
{
	Console.WriteLine(productResult.Error.Message);
	loggingToPowerAutomate.Log(productResult.Error.Message);
	Thread.Sleep(10000);
	return;
}
Console.WriteLine("(3/5) Finding changes to certifications, EPDs and Hazard sentences.");
loggingToPowerAutomate.Log("(3/5) Finding changes to certifications, EPDs and Hazard sentences.");
var certificationResult = certificationChangeFinder.FindCertificationChanges(productResult.Value);
var epdResult = ePDChangeFinder.FindEPDChanges(productResult.Value);
var hazardResult = hazardChangeFinder.FindHazardChanges(productResult.Value);
Console.WriteLine("(4/5) Creating Csv Files");
loggingToPowerAutomate.Log("(4/5) Creating Csv Files");
var csvResult1 = cSVFileCreator.CreateCertificationChangeCSVFile
	(basePath, certificationResult.ToList());
var csvResult2 = epdChangeCSVCreator.CreateEPDCSVFile(basePath, epdResult.ToList());
var csvResult3 = hazardChangeCSVCreator.CreateHazardCSVFile(basePath, hazardResult.ToList());
ICollection<Result<string>> csvResult = new List<Result<string>>
{
	csvResult1,
	csvResult2,
	csvResult3
};
if (csvResult.Any(x => x.IsFailure))
{
	Console.WriteLine(csvResult.FirstOrDefault(x => x.IsFailure == true)!.Error.Message);
	loggingToPowerAutomate.Log(csvResult.FirstOrDefault(x => x.IsFailure == true)!.Error.Message);
	Thread.Sleep(20000);
	return;
}
ICollection<string> csvResult4 = new List<string>();
foreach (var item in csvResult)
{
	csvResult4.Add(item.Value);
}
csvToXlsxConverter.ConvertToXlsx(basePath, csvResult4);
Console.WriteLine("(5/5) Updating RunLog");
loggingToPowerAutomate.Log("(5/5) Updating RunLog");
logProvider.UpdateLog();
Console.WriteLine("End");
loggingToPowerAutomate.Log("End");
Thread.Sleep(10000);
//string basePath = "C:\\Users\\AME\\OneDrive - XL-BYG a.m.b.a\\Vigtige filer\\ByggebasenTest\\";
//ICsvToXlsxConverter csvToXlsxConverter = new CsvToXlsxConverter();
//ICollection<string> csvResult4 = new List<string>()
//{
//	@"C:\\Users\\AME\\OneDrive - XL-BYG a.m.b.a\\Vigtige filer\\ByggebasenTest\\ResultCSV\\CertificationUpdates11-04-2024.csv",
//	@"C:\\Users\\AME\\OneDrive - XL-BYG a.m.b.a\\Vigtige filer\\ByggebasenTest\\ResultCSV\\EPDUpdates11-04-2024.csv",
//	@"C:\\Users\\AME\\OneDrive - XL-BYG a.m.b.a\\Vigtige filer\\ByggebasenTest\\ResultCSV\\HazardUpdates11-04-2024.csv"
//};
//csvToXlsxConverter.ConvertToXlsx(basePath, csvResult4);
