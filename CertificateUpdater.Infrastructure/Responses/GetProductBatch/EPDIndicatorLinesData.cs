using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetProductBatch;
internal sealed record EPDIndicatorLinesData
{
	[JsonPropertyName("A1")]
	public decimal? A1 { get; set; }

	[JsonPropertyName("A1A3")]
	public decimal? A1A3 { get; set; }

	[JsonPropertyName("A2")]
	public decimal? A2 { get; set; }

	[JsonPropertyName("A3")]
	public decimal? A3 { get; set; }

	[JsonPropertyName("A4")]
	public decimal? A4 { get; set; }

	[JsonPropertyName("A5")]
	public decimal? A5 { get; set; }

	[JsonPropertyName("B1")]
	public decimal? B1 { get; set; }

	[JsonPropertyName("B1B7")]
	public decimal? B1B7 { get; set; }

	[JsonPropertyName("B1C1")]
	public decimal? B1C1 { get; set; }

	[JsonPropertyName("B2")]
	public decimal? B2 { get; set; }

	[JsonPropertyName("B2B7")]
	public decimal? B2B7 { get; set; }

	[JsonPropertyName("B3")]
	public decimal? B3 { get; set; }

	[JsonPropertyName("B3B7")]
	public decimal? B3B7 { get; set; }

	[JsonPropertyName("B4")]
	public decimal? B4 { get; set; }

	[JsonPropertyName("B5")]
	public decimal? B5 { get; set; }

	[JsonPropertyName("B6")]
	public decimal? B6 { get; set; }

	[JsonPropertyName("B7")]
	public decimal? B7 { get; set; }

	[JsonPropertyName("C1")]
	public decimal? C1 { get; set; }

	[JsonPropertyName("C2")]
	public decimal? C2 { get; set; }

	[JsonPropertyName("C3")]
	public decimal? C3 { get; set; }

	[JsonPropertyName("C3_1")]
	public decimal? C3_1 { get; set; }

	[JsonPropertyName("C3_2")]
	public decimal? C3_2 { get; set; }

	[JsonPropertyName("C4")]
	public decimal? C4 { get; set; }

	[JsonPropertyName("C4_1")]
	public decimal? C4_1 { get; set; }

	[JsonPropertyName("C4_2")]
	public decimal? C4_2 { get; set; }

	[JsonPropertyName("D")]
	public decimal? D { get; set; }

	[JsonPropertyName("D_1")]
	public decimal? D_1 { get; set; }

	[JsonPropertyName("D_2")]
	public decimal? D_2 { get; set; }

	[JsonPropertyName("EPD_Header_ID")]
	public int? EPDHeaderId { get; set; }

	[JsonPropertyName("ID")]
	public int? Id { get; set; }

	[JsonPropertyName("Indicator")]
	public string? Indicator { get; set; } = string.Empty;

	[JsonPropertyName("PhaseUnit")]
	public string? PhaseUnit { get; set; } = string.Empty;
}
