namespace CertificateUpdater.Domain.Entities;
public sealed record ProductSafetySentence
{
	public string? AjourDate { get; set; }
	public int? AjourId { get; set; }
	public string? AjourUser { get; set; }
	public string? Sentence { get; set; }
	public string? SentenceCode { get; set; }
}
