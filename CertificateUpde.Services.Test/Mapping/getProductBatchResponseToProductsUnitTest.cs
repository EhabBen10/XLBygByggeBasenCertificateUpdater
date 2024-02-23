using CertificateUpdater.Services.Mapping;
using CertificateUpdater.Services.Responses.GetProductBatch;
using Xunit;

namespace CertificateUpdater.Services.Test.Mapping;
public sealed class getProductBatchResponseToProductsUnitTest
{
	private GetProductBatchResponse _getProductBatchResponse = new();
	private readonly string _testSupplierNr = "testSupplierNr";
	private readonly string _testCompanyName = "testCompanyName";
	private readonly int _testDBNr = 12;
	private readonly string _testProductText1 = "testProductText";
	private readonly int _testVareGruppeId = 1234;
	private readonly ICollection<DGNBDocumentData> _testDGNBDocumentData = new List<DGNBDocumentData>();
	private readonly ICollection<KatalogData> _testKatalogData = new List<KatalogData>();
	private readonly ICollection<EPDData> _testEpdData = new List<EPDData>();
	private readonly ICollection<ProductHazardSentenceData> _testProductHazardSentenceData = new List<ProductHazardSentenceData>();
	private readonly ICollection<ProductHazardSymbolData> _testProductHazardSymbolData = new List<ProductHazardSymbolData>();
	private readonly ICollection<ProductSafetySentenceData> _testProductSafetySentencesData = new List<ProductSafetySentenceData>();

	[Fact]
	public void ToProducts_GetProductBatchResponseNull_ArgumentNullExceptionThrown()
	{
		// Arrange
		_getProductBatchResponse = null!;

		// Act
		var result = Record.Exception(_getProductBatchResponse.ToProducts);

		//Assert
		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void ToProducts_ResponseIsNull_ArgumentNullExceptionThrown()
	{
		// Arrange
		_getProductBatchResponse.Result.ResultData = new List<ResultData>() { null! };

		// Act
		var result = Record.Exception(_getProductBatchResponse.ToProducts);

		//Assert
		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void ToProducts_ResultNull_EmptyProductListReturned()
	{
		// Arrange
		_getProductBatchResponse.Result = null!;

		// Act
		var result = _getProductBatchResponse.ToProducts();

		//Assert
		Assert.Empty(result);
	}
	[Fact]
	public void ToProducts_ResultDataNull_EmptyProductListReturned()
	{
		// Arrange
		_getProductBatchResponse.Result.ResultData = null!;

		// Act
		var result = _getProductBatchResponse.ToProducts();

		//Assert
		Assert.Empty(result);
	}

	[Fact]
	public void ToProducts_ResultDataSupplierNrNull_ArgumentNullExceptionThrown()
	{
		// Arrange
		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = null!,
		});

		// Act
		var result = Record.Exception(_getProductBatchResponse.ToProducts);

		//Assert
		Assert.IsType<ArgumentNullException>(result);
	}
	[Fact]
	public void ToProducts_ResultDataSupplierNrEmpty_ArgumentNullExceptionThrown()
	{
		// Arrange
		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = "",
		});

		// Act
		var result = Record.Exception(_getProductBatchResponse.ToProducts);

		//Assert
		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void ToProducts_ResultDataValid_ExpectedCollectionProductReturned()
	{
		// Arrange
		DGNBDocumentData dGNBDocumentData = new()
		{
			IndicatorNumber = 12,
			IndicatorStep = 3
		};
		_testDGNBDocumentData.Add(dGNBDocumentData);

		KatalogData katalogData = new()
		{
			EmneId = 1,
			isValid = true,
			Tunnr = 12,
		};
		_testKatalogData.Add(katalogData);

		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,
			CompanyName = _testCompanyName,
			DBNr = _testDBNr,
			ProductText1 = _testProductText1,
			DGNBDocuments = _testDGNBDocumentData,
			KatalogData = _testKatalogData,
			ProductGroupId = _testVareGruppeId
		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		//Assert
		Assert.NotEmpty(result.First().DGNBDocuments);
		Assert.NotEmpty(result.First().KatalogData);
		Assert.Equal(_testDBNr, result.First().DBNr);
		Assert.Equal(_testProductText1, result.First().ProductText);
		Assert.Equal(_testSupplierNr, result.First().SupplierNr);
		Assert.Equal(_testCompanyName, result.First().CompanyName);
		Assert.Equal(_testVareGruppeId, result.First().ProductGroupId);
	}

	[Fact]
	public void ToProducts_SameIndicatorNumberUsedTwice_OnlyOneDGNBDocumentDataAdded()
	{
		// Arrange
		DGNBDocumentData dGNBDocumentData1 = new()
		{
			IndicatorNumber = 12,
			IndicatorStep = 3
		};
		DGNBDocumentData dGNBDocumentData2 = new()
		{
			IndicatorNumber = 12,
			IndicatorStep = 4
		};
		_testDGNBDocumentData.Add(dGNBDocumentData1);
		_testDGNBDocumentData.Add(dGNBDocumentData2);



		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,
			DGNBDocuments = _testDGNBDocumentData,
		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		//Assert
		Assert.Single(result.First().DGNBDocuments);
	}

	[Fact]
	public void ToProducts_TwoDifferentIndicatorNumbersUsed_TwoDGNBDocumentDataAdded()
	{
		// Arrange
		DGNBDocumentData dGNBDocumentData1 = new()
		{
			IndicatorNumber = 12,
			IndicatorStep = 3
		};
		DGNBDocumentData dGNBDocumentData2 = new()
		{
			IndicatorNumber = 13,
			IndicatorStep = 4
		};
		_testDGNBDocumentData.Add(dGNBDocumentData1);
		_testDGNBDocumentData.Add(dGNBDocumentData2);



		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			DGNBDocuments = _testDGNBDocumentData,
			SupplierNr = _testSupplierNr,

		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		//Assert
		Assert.Equal(2, result.First().DGNBDocuments.Count);
	}

	[Fact]
	public void ToProducts_EPDDataWithNoPdfAppxNameOrPDFNameOrPhaseUnitOrIndicator_EPDDataAdded()
	{
		// Arrange
		string pdfId = "testPdfId";
		decimal conversionFactor = 123;
		string creationDate = "/Date(1687989600000+0200)/";
		string eN15084ACertification = "123";
		int ePDType = 8;
		string functionalUnit = "testFunctionalUnit";
		int functionalUnitAmount = 4;
		bool iSO14025Certified = true;
		bool iSO14040Certified = true;
		bool iSO14044Certified = true;
		string pdfAppxDate = "/Date(1687989600000+0200)/";
		string pdfDate = "/Date(1687989600000+0200)/";
		string validFrom = "/Date(1687989600000+0200)/";
		string validTo = "/Date(1687989600000+0200)/";
		int serviceLifeAmount = 845;
		int testA1 = 78;
		int testA2 = 3567;
		int testA3 = 12363;
		int testA1A3 = 5;
		int testA4 = 1982;
		int testA5 = 36;
		int testB1 = 96;
		int testB2 = 964;
		int testB3 = 711;
		int testB1B7 = 432;
		int testB1C1 = 121;
		int testB2B7 = 90;
		int testB3B7 = 13;
		int testB4 = 1241;
		int testB5 = 908;
		int testB6 = 72;
		int testB7 = 798;
		int testC1 = 231;
		int testC2 = 55576;
		int testC3 = 9814;
		int testC3_1 = 9654587;
		int testC3_2 = 65;
		int testC4 = 5768;
		int testC4_1 = 345;
		int testC4_2 = 687;
		int testD = 4682;
		int testD_1 = 72;
		int testD_2 = 87621;
		int testEPDHeaderId = 716;
		int testId = 714;
		EPDData epDData1 = new()
		{
			ConversionFactor = conversionFactor,
			CreationDate = creationDate,
			EN15804ACertification = Convert.ToInt32(eN15084ACertification),
			EPDType = ePDType,
			FunctionalUnit = functionalUnit,
			FunctionalUnitAmount = functionalUnitAmount,
			ISO14025Certified = iSO14025Certified,
			ISO14040Certified = iSO14040Certified,
			ISO14044Certified = iSO14044Certified,
			PdfAppxDate = pdfAppxDate,
			PdfDate = pdfDate,
			PdfID = pdfId,
			ServiceLifeAmount = serviceLifeAmount,
			ValidFrom = validFrom,
			ValidTo = validTo,
			EPDIndicatorLinesData = new List<EPDIndicatorLinesData>() { new()
			{
				A1 = testA1,
				A2 = testA2,
				A3 = testA3,
				A4 = testA4,
				A5 = testA5,
				A1A3 = testA1A3,
				B1 = testB1,
				B2 = testB2,
				B3 = testB3,
				B4 = testB4,
				B5 = testB5,
				B1B7 = testB1B7,
				B2B7 = testB2B7,
				B1C1 = testB1C1,
				B3B7 = testB3B7,
				B6 = testB6,
				B7 = testB7,
				C1 = testC1,
				C2 = testC2,
				C3 = testC3,
				C4 = testC4,
				C3_1 = testC3_1,
				C3_2 = testC3_2,
				C4_1 = testC4_1,
				C4_2 = testC4_2,
				D = testD,
				D_1 = testD_1,
				D_2 = testD_2,
				EPDHeaderId = testEPDHeaderId,
				Id = testId,
			} }
		};
		_testEpdData.Add(epDData1);

		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,
			EPDDatas = _testEpdData,
		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		// Assert
		Assert.Single(result.First().EPDs!);
		Assert.Null(result.First().EPDs!.First().PdfAppxName);
		Assert.Empty(result.First().EPDs!.First().PdfName);
	}

	[Fact]
	public void ToProducts_EPDDataWithAllFields_EPDDataAdded()
	{
		// Arrange
		string pdfId = "testPdfId";
		decimal conversionFactor = 123;
		string creationDate = "/Date(1687989600000+0200)/";
		string eN15084ACertification = "123";
		int ePDType = 8;
		string functionalUnit = "testFunctionalUnit";
		int functionalUnitAmount = 4;
		bool iSO14025Certified = true;
		bool iSO14040Certified = true;
		bool iSO14044Certified = true;
		string pdfAppxDate = "/Date(1687989600000+0200)/";
		string pdfAppxName = "testpdfAppxName";
		string pdfDate = "/Date(1687989600000+0200)/";
		string pdfName = "testpdfName";
		string validFrom = "/Date(1687989600000+0200)/";
		string validTo = "/Date(1687989600000+0200)/";
		int serviceLifeAmount = 845;
		int testA1 = 78;
		int testA2 = 3567;
		int testA3 = 12363;
		int testA1A3 = 5;
		int testA4 = 1982;
		int testA5 = 36;
		int testB1 = 96;
		int testB2 = 964;
		int testB3 = 711;
		int testB1B7 = 432;
		int testB1C1 = 121;
		int testB2B7 = 90;
		int testB3B7 = 13;
		int testB4 = 1241;
		int testB5 = 908;
		int testB6 = 72;
		int testB7 = 798;
		int testC1 = 231;
		int testC2 = 55576;
		int testC3 = 9814;
		int testC3_1 = 9654587;
		int testC3_2 = 65;
		int testC4 = 5768;
		int testC4_1 = 345;
		int testC4_2 = 687;
		int testD = 4682;
		int testD_1 = 72;
		int testD_2 = 87621;
		int testEPDHeaderId = 716;
		int testId = 714;
		string testIndicator = "testIndicator";
		string testPhaseUnit = "testPhaseUnit";
		EPDData epDData1 = new()
		{
			ConversionFactor = conversionFactor,
			CreationDate = creationDate,
			EN15804ACertification = Convert.ToInt32(eN15084ACertification),
			EPDType = ePDType,
			FunctionalUnit = functionalUnit,
			FunctionalUnitAmount = functionalUnitAmount,
			ISO14025Certified = iSO14025Certified,
			ISO14040Certified = iSO14040Certified,
			ISO14044Certified = iSO14044Certified,
			PdfAppxDate = pdfAppxDate,
			PdfAppxName = pdfAppxName,
			PdfDate = pdfDate,
			PdfName = pdfName,
			PdfID = pdfId,
			ServiceLifeAmount = serviceLifeAmount,
			ValidFrom = validFrom,
			ValidTo = validTo,
			EPDIndicatorLinesData = new List<EPDIndicatorLinesData>() { new()
			{
				A1 = testA1,
				A2 = testA2,
				A3 = testA3,
				A4 = testA4,
				A5 = testA5,
				A1A3 = testA1A3,
				B1 = testB1,
				B2 = testB2,
				B3 = testB3,
				B4 = testB4,
				B5 = testB5,
				B1B7 = testB1B7,
				B2B7 = testB2B7,
				B1C1 = testB1C1,
				B3B7 = testB3B7,
				B6 = testB6,
				B7 = testB7,
				C1 = testC1,
				C2 = testC2,
				C3 = testC3,
				C4 = testC4,
				C3_1 = testC3_1,
				C3_2 = testC3_2,
				C4_1 = testC4_1,
				C4_2 = testC4_2,
				D = testD,
				D_1 = testD_1,
				D_2 = testD_2,
				EPDHeaderId = testEPDHeaderId,
				Id = testId,
				Indicator = testIndicator,
				PhaseUnit = testPhaseUnit,
			} }
		};
		_testEpdData.Add(epDData1);

		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,

			EPDDatas = _testEpdData,
		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		// Assert
		Assert.Single(result.First().EPDs!);
		Assert.Equal(epDData1.EPDType, result.First().EPDs!.First().EPDType);
	}
	[Fact]
	public void ToProducts_EPDIndicatorLinesWithProblematicCharacters_ProblematicCharactersRemovedAdded()
	{
		// Arrange

		string testIndicator = "Indicator\twith\nproblematic\rcharacters";
		string testPhaseUnit = "PhaseUnit\twith\nproblematic\rcharacters";
		EPDData epDData1 = new()
		{
			CreationDate = "/Date(1687989600000+0200)/",
			PdfAppxDate = "/Date(1687989600000+0200)/",
			ValidFrom = "/Date(1687989600000+0200)/",
			ValidTo = "/Date(1687989600000+0200)/",
			PdfDate = "/Date(1687989600000+0200)/",

			EPDIndicatorLinesData = new List<EPDIndicatorLinesData>() { new()
			{
				Indicator = testIndicator,
				PhaseUnit = testPhaseUnit,

	}
}
		};
		_testEpdData.Add(epDData1);

		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,

			EPDDatas = _testEpdData,
		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		// Assert
		Assert.Equal("Indicatorwithproblematiccharacters", result.First().EPDs!.First().EPDIndicatorLines.Indicator);
		Assert.Equal("PhaseUnitwithproblematiccharacters", result.First().EPDs!.First().EPDIndicatorLines.PhaseUnit);
	}
	[Fact]
	public void ToProducts_EPDIndicatorLinesWithNullIndiciatorAndPhase_TheyAreStillNull()
	{
		// Arrange
		EPDData epDData1 = new()
		{
			CreationDate = "/Date(1687989600000+0200)/",
			PdfAppxDate = "/Date(1687989600000+0200)/",
			ValidFrom = "/Date(1687989600000+0200)/",
			ValidTo = "/Date(1687989600000+0200)/",
			PdfDate = "/Date(1687989600000+0200)/",

			EPDIndicatorLinesData = new List<EPDIndicatorLinesData>() { new() }
		};
		_testEpdData.Add(epDData1);

		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,
			EPDDatas = _testEpdData,
		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		// Assert
		Assert.Null(result.First().EPDs.First().EPDIndicatorLines?.PhaseUnit);
		Assert.Null(result.First().EPDs.First().EPDIndicatorLines?.Indicator);


	}

	[Fact]
	public void ToProducts_HazardDataNull_HazardMarkEqual()
	{
		// Arrange
		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,
			HasHazardousGoods = true,
			HazardMark = "testHazardMark",
		});
		// Act
		var result = _getProductBatchResponse.ToProducts();

		// Assert
		Assert.Equal(_getProductBatchResponse.Result.ResultData.First().HazardMark, result.FirstOrDefault()!.HazardInfo!.HazardMark);
		Assert.Empty(result.FirstOrDefault()!.HazardInfo!.ProductHazardSentences);
		Assert.Empty(result.FirstOrDefault()!.HazardInfo!.ProductHazardSymbols);
		Assert.Empty(result.FirstOrDefault()!.HazardInfo!.ProductSafetySentences);
	}

	[Fact]
	public void ToProducts_HazardDataWithProductHazardSentences_HazardSentencesDataAdded()
	{
		// Arrange
		string _testAjourDate = "testAjourDate";
		int _testAjourId = 1;
		string _testAjourUser = "testAjourUser";
		string _testSentence = "testSentence";
		string _testSentenceCode = "testSentenceCode";

		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,
			HazardMark = "testHazardMark",
			ProductHazardSentencesData = new List<ProductHazardSentenceData>()
			{
				new()
				{
					AjourDate=_testAjourDate,
					AjourId=_testAjourId,
					AjourUser=_testAjourUser,
					Sentence=_testSentence,
					SentenceCode=_testSentenceCode
				}
			}
		});
		// Act
		var result = _getProductBatchResponse.ToProducts();

		// Assert
		Assert.Equal(_getProductBatchResponse.Result.ResultData.First().HazardMark, result.FirstOrDefault()!.HazardInfo!.HazardMark);
		Assert.NotEmpty(result.FirstOrDefault()!.HazardInfo!.ProductHazardSentences);
		Assert.Equal(_testSentence, result.FirstOrDefault()!.HazardInfo.ProductHazardSentences.FirstOrDefault().Sentence);
		Assert.Equal(_testSentenceCode, result.FirstOrDefault()!.HazardInfo.ProductHazardSentences.FirstOrDefault().SentenceCode);
		Assert.Equal(_testAjourDate, result.FirstOrDefault()!.HazardInfo.ProductHazardSentences.FirstOrDefault().AjourDate);
		Assert.Equal(_testAjourId, result.FirstOrDefault()!.HazardInfo.ProductHazardSentences.FirstOrDefault().AjourId);
		Assert.Equal(_testAjourUser, result.FirstOrDefault()!.HazardInfo.ProductHazardSentences.FirstOrDefault().AjourUser);

		Assert.Empty(result.FirstOrDefault()!.HazardInfo!.ProductHazardSymbols);
		Assert.Empty(result.FirstOrDefault()!.HazardInfo!.ProductSafetySentences);
	}
	[Fact]
	public void ToProducts_HazardDataWithProductSafetySentences_SafetySentencesDataAdded()
	{
		// Arrange
		string _testAjourDate = "testAjourDate";
		int _testAjourId = 1;
		string _testAjourUser = "testAjourUser";
		string _testSentence = "testSentence";
		string _testSentenceCode = "testSentenceCode";

		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,
			HazardMark = "testHazardMark",
			ProductSafetySentencesData = new List<ProductSafetySentenceData>()
			{
				new()
				{
					AjourDate=_testAjourDate,
					AjourId=_testAjourId,
					AjourUser=_testAjourUser,
					Sentence=_testSentence,
					SentenceCode=_testSentenceCode
				}
			}
		});
		// Act
		var result = _getProductBatchResponse.ToProducts();

		// Assert
		Assert.Equal(_getProductBatchResponse.Result.ResultData.First().HazardMark, result.FirstOrDefault()!.HazardInfo!.HazardMark);
		Assert.NotEmpty(result.FirstOrDefault()!.HazardInfo!.ProductSafetySentences);
		Assert.Equal(_testSentence, result.FirstOrDefault()!.HazardInfo.ProductSafetySentences.FirstOrDefault().Sentence);
		Assert.Equal(_testSentenceCode, result.FirstOrDefault()!.HazardInfo.ProductSafetySentences.FirstOrDefault().SentenceCode);
		Assert.Equal(_testAjourDate, result.FirstOrDefault()!.HazardInfo.ProductSafetySentences.FirstOrDefault().AjourDate);
		Assert.Equal(_testAjourId, result.FirstOrDefault()!.HazardInfo.ProductSafetySentences.FirstOrDefault().AjourId);
		Assert.Equal(_testAjourUser, result.FirstOrDefault()!.HazardInfo.ProductSafetySentences.FirstOrDefault().AjourUser);
		Assert.Empty(result.FirstOrDefault()!.HazardInfo!.ProductHazardSymbols);
		Assert.Empty(result.FirstOrDefault()!.HazardInfo!.ProductHazardSentences);
	}
	[Fact]
	public void ToProducts_HazardDataWithProductHazardSymbol_HazardSymbolDataAdded()
	{
		// Arrange
		string _testSymDesc = "testSymDesc";
		string _testSymImgUrl = "testSymImgUrl";
		string _testSymName = "testSymName";

		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,
			HazardMark = "testHazardMark",
			ProductHazardSymbolsData = new List<ProductHazardSymbolData>()
			{
				new()
				{
					SymDesc=_testSymDesc,
					SymImgUrl=_testSymImgUrl,
					SymName=_testSymName
				}
			}
		});
		// Act
		var result = _getProductBatchResponse.ToProducts();

		// Assert
		Assert.Equal(_getProductBatchResponse.Result.ResultData.First().HazardMark, result.FirstOrDefault()!.HazardInfo!.HazardMark);
		Assert.NotEmpty(result.FirstOrDefault()!.HazardInfo!.ProductHazardSymbols);
		Assert.Equal(_testSymName, result.FirstOrDefault()!.HazardInfo.ProductHazardSymbols.FirstOrDefault().SymName);
		Assert.Equal(_testSymDesc, result.FirstOrDefault()!.HazardInfo.ProductHazardSymbols.FirstOrDefault().SymDesc);
		Assert.Equal(_testSymImgUrl, result.FirstOrDefault()!.HazardInfo.ProductHazardSymbols.FirstOrDefault().SymImgUrl);
		Assert.Empty(result.FirstOrDefault()!.HazardInfo!.ProductSafetySentences);
		Assert.Empty(result.FirstOrDefault()!.HazardInfo!.ProductHazardSentences);
	}
	[Fact]
	public void ToProducts_ReplaceProblematicCharactersInProductText()
	{
		// Arrange
		string productTextWithProblematicChars = "Product\twith\nproblematic\rcharacters";
		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,
			ProductText1 = productTextWithProblematicChars,
		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		// Assert
		Assert.Equal("Productwithproblematiccharacters", result.FirstOrDefault()!.ProductText);
	}

	[Fact]
	public void ToProducts_ReplaceProblematicCharactersInSupplierNr()
	{
		// Arrange
		string supplierNrWithProblematicChars = "Supplier\rwith\tproblematic\ncharacters";
		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = supplierNrWithProblematicChars,
		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		// Assert
		Assert.Equal("Supplierwithproblematiccharacters", result.FirstOrDefault()!.SupplierNr);
	}

	[Fact]
	public void ToProducts_ReplaceProblematicCharactersInCompanyName()
	{
		// Arrange
		string companyNameWithProblematicChars = "Company\rName\twith\nproblematic\rcharacters";
		_getProductBatchResponse.Result.ResultData.Add(new()
		{
			SupplierNr = _testSupplierNr,
			CompanyName = companyNameWithProblematicChars,
		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		// Assert
		Assert.Equal("CompanyNamewithproblematiccharacters", result.FirstOrDefault()!.CompanyName);
	}

}

