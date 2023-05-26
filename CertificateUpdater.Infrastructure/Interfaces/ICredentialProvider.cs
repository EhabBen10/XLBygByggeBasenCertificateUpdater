namespace CertificateUpdater.Services.Interfaces;
public interface ICredentialProvider
{
	public int GetTunUserNr();
	public string GetUserName();
	public string GetPassword();
	public string GetAspect4Username();
	public string GetAspect4Password();
}
