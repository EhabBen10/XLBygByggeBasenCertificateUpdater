using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.AWSSimulators;
public sealed class LocalFileCredentialProvider : ICredentialProvider
{
	public string TunnrFileName { get; set; }
	public string UserNameFileName { get; set; }
	public string PasswordFileName { get; set; }
	public string Aspect4UsernameFileName { get; set; }
	public string Aspect4PasswordFileName { get; set; }
	public LocalFileCredentialProvider(string tunnrFileName, string userNameFileName, string passwordFileName, string aspect4UsernameFileName, string aspect4PasswordFileName)
	{
		TunnrFileName = tunnrFileName;
		UserNameFileName = userNameFileName;
		PasswordFileName = passwordFileName;
		Aspect4UsernameFileName = aspect4UsernameFileName;
		Aspect4PasswordFileName = aspect4PasswordFileName;
	}

	public int GetTunUserNr()
	{
		using StreamReader sr = new(TunnrFileName);
		return Convert.ToInt32(sr.ReadToEnd());
	}

	public string GetUserName()
	{
		using StreamReader sr = new(UserNameFileName);
		return sr.ReadToEnd();
	}

	public string GetPassword()
	{
		using StreamReader sr = new(PasswordFileName);
		return sr.ReadToEnd();
	}
	public string GetAspect4Username()
	{
		using StreamReader sr = new(Aspect4UsernameFileName);
		return sr.ReadToEnd();
	}

	public string GetAspect4Password()
	{
		using StreamReader sr = new(Aspect4PasswordFileName);
		return sr.ReadToEnd();
	}
}
