using System.Text.Json.Serialization;

namespace CertificateUpdater.Services.Responses.GetProductBatch;
internal sealed record DGNBDocumentData
{
	[JsonPropertyName("Ind_No")]
	public int IndicatorNumber { get; set; }

	[JsonPropertyName("Ind_Step")]
	public int IndicatorStep { get; set; }
}
