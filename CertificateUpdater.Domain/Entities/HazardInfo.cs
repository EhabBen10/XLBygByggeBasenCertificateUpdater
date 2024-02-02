﻿namespace CertificateUpdater.Domain.Entities;
public sealed record HazardInfo
{
	public string? HazardClass { get; set; }
	public string? HazardMark { get; set; }
	public bool? HasHazardousGoods { get; set; }
	public List<ProductHazardSentence> ProductHazardSentences { get; set; } = new List<ProductHazardSentence>();
	public List<ProductSafetySentence> ProductSafetySentences { get; set; } = new List<ProductSafetySentence>();
	public List<ProductHazardSymbol> ProductHazardSymbols { get; set; } = new List<ProductHazardSymbol>();
	public string? UNCode { get; set; }
	public string? ShippingDesignation { get; set; }
}