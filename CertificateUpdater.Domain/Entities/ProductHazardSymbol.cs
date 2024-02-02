namespace CertificateUpdater.Domain.Entities;
public sealed record ProductHazardSymbol
{
	public string? SymDesc { get; set; }
	public string? SymName { get; set; }
	public string? SymImgUrl { get; set; }
}
