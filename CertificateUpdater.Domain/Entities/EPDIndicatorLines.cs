namespace CertificateUpdater.Domain.Entities;
public sealed record EPDIndicatorLines
{
	public decimal? A1 { get; set; }
	public decimal? A1A3 { get; set; }
	public decimal? A2 { get; set; }
	public decimal? A3 { get; set; }
	public decimal? A4 { get; set; }
	public decimal? A5 { get; set; }
	public decimal? B1 { get; set; }
	public decimal? B2 { get; set; }
	public decimal? B3 { get; set; }
	public decimal? B4 { get; set; }
	public decimal? B5 { get; set; }
	public decimal? B6 { get; set; }
	public decimal? B7 { get; set; }
	public decimal? B2B7 { get; set; }
	public decimal? B3B7 { get; set; }
	public decimal? B1B7 { get; set; }
	public decimal? B1C1 { get; set; }
	public decimal? C1 { get; set; }
	public decimal? C2 { get; set; }
	public decimal? C3 { get; set; }
	public decimal? C4 { get; set; }
	public decimal? C2_1 { get; set; }
	public decimal? C2_2 { get; set; }
	public decimal? C3_1 { get; set; }
	public decimal? C3_2 { get; set; }
	public decimal? C4_1 { get; set; }
	public decimal? C4_2 { get; set; }
	public decimal? D { get; set; }
	public decimal? D_1 { get; set; }
	public decimal? D_2 { get; set; }
	public int? EPDHeaderId { get; set; }
	public int? Id { get; set; }
	public string? Indicator { get; set; }
	public string? PhaseUnit { get; set; }
}
