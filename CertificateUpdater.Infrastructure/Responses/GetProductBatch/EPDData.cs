using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetProductBatch;
internal sealed record EPDData
{
	[JsonPropertyName("ConversionFactor")]
	public decimal? ConversionFactor { get; set; }

	[JsonPropertyName("CreationDate")]
	public string CreationDate { get; set; } = string.Empty;

	[JsonPropertyName("EN15804ACertification")]
	public int? EN15804ACertification { get; set; }

	[JsonPropertyName("EPDType")]
	public int? EPDType { get; set; }

	[JsonPropertyName("EPD_IndicatorLines")]
	public List<EPDIndicatorLinesData>? EPDIndicatorLinesData { get; set; }

	[JsonPropertyName("FunctionalUnit")]
	public string FunctionalUnit { get; set; } = string.Empty;

	[JsonPropertyName("FunctionalUnitAmount")]
	public int? FunctionalUnitAmount { get; set; }

	[JsonConverter(typeof(CustomBooleanConverter))]
	[JsonPropertyName("ISO14025Certified")]
	public bool? ISO14025Certified { get; set; } = false;

	[JsonConverter(typeof(CustomBooleanConverter))]
	[JsonPropertyName("ISO14040Certified")]
	public bool? ISO14040Certified { get; set; } = false;

	[JsonConverter(typeof(CustomBooleanConverter))]
	[JsonPropertyName("ISO14044Certified")]
	public bool? ISO14044Certified { get; set; } = false;

	[JsonPropertyName("PdfAppxDate")]
	public string? PdfAppxDate { get; set; }

	[JsonPropertyName("PdfAppxName")]
	public string? PdfAppxName { get; set; }

	[JsonPropertyName("PdfDate")]
	public string? PdfDate { get; set; }

	[JsonPropertyName("PdfID")]
	public string PdfID { get; set; } = string.Empty;

	[JsonPropertyName("PdfName")]
	public string PdfName { get; set; } = string.Empty;

	[JsonPropertyName("ServiceLifeAmount")]
	public int? ServiceLifeAmount { get; set; }

	[JsonPropertyName("ValidFrom")]
	public string? ValidFrom { get; set; }

	[JsonPropertyName("ValidTo")]
	public string? ValidTo { get; set; }
}
