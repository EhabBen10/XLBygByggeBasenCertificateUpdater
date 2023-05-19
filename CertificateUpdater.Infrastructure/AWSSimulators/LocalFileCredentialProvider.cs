using CertificateUpdater.Services.Interfaces;

namespace CertificateUpdater.Services.AWSSimulators;
public sealed class LocalFileCredentialProvider : ICredentialProvider
{
	public string TunnrFileName { get; set; }
	public string UserNameFileName { get; set; }
	public string PasswordFileName { get; set; }
	public LocalFileCredentialProvider(string tunnrFileName, string userNameFileName, string passwordFileName)
	{
		TunnrFileName = tunnrFileName;
		UserNameFileName = userNameFileName;
		PasswordFileName = passwordFileName;
	}

	public int GetTunUserNr()
	{
		using StreamReader sr = new StreamReader(TunnrFileName);
		return Convert.ToInt32(sr.ReadToEnd());
	}

	public string GetUserName()
	{
		using StreamReader sr = new StreamReader(UserNameFileName);
		return sr.ReadToEnd();
	}

	public string GetPassword()
	{
		using StreamReader sr = new StreamReader(PasswordFileName);
		return sr.ReadToEnd();
	}
}
