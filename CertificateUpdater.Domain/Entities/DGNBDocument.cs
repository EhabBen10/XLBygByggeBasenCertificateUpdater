namespace CertificateUpdater.Domain.Entities;
public sealed record DGNBDocument
{
	public int IndicatorNumber { get; set; }
	public int IndicatorStep { get; set; }
}
