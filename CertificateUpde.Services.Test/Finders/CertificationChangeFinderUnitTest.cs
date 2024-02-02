using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Enum;
using CertificateUpdater.Services.Finders;
using CertificateUpdater.Services.Interfaces;
using Xunit;

namespace CertificateUpdater.Services.Test.Finders;

public sealed class CertificationChangeFinderUnitTest
{
	private readonly ICertificationChangeFinder _uut = new CertificationChangeFinder();
	private readonly string _testCompanyName = "testCompanyName";
	private readonly string _testProductText = "testProductText";
	private readonly string _testSupplierNr = "testSupplierNr";
	private readonly int _testDBNr = 4;
	private readonly int _testVareGruppeId = 1234;

	[Fact]
	public void FindCertificationChanges_ProductsNull_ExceptionsIsThrown()
	{
		//Arrange
		//Act
		var result = Record.Exception(() => _uut.FindCertificationChanges(null!));

		//Assert
		Assert.IsType<NullReferenceException>(result);
	}

	[Fact]
	public void FindCertificationChanges_NoProductInList_NoChangesAdded()
	{
		//Arrange
		ICollection<Product> products = new List<Product>();

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.Empty(result);
	}

	[Fact]
	public void FindCertificationChanges_ProductsNotEmpty_CertificationChangeContainsNewChange()
	{
		//Arrange
		ICollection<Product> products = new List<Product>()
		{
			new Product()
			{
				CompanyName = _testCompanyName,
				DBNr = _testDBNr,
				ProductText= _testProductText,
				SupplierNr=_testSupplierNr,
				ProductGroupId=_testVareGruppeId
			}
		};

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.NotEmpty(result);
		Assert.Equal(_testCompanyName, result.First().CompanyName);
		Assert.Equal(_testProductText, result.First().ProductText);
		Assert.Equal(_testSupplierNr, result.First().SupplierNr);
		Assert.Equal(_testDBNr, result.First().DBNr);
		Assert.Equal(_testVareGruppeId, result.First().ProductGroupId);
	}

	[Fact]
	public void FindCertificationChanges_ProductWithDGNBDocument_CertificationChangeContainsDgnbChanges()
	{
		//Arrange
		ICollection<Product> products = new List<Product>()
		{
			new Product()
			{
				DGNBDocuments= new List<DGNBDocument>()
				{
					new()
					{
						IndicatorNumber= 1,
						IndicatorStep = 2
					}
				}
			}
		};

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.Equal("1:2", result.First().DGNBQualityStep.First());
	}

	[Theory]
	[InlineData(0, false, false, false, false, false, false, false, false, false, false, false, false)]
	public void FindCertificationChanges_ProductWithNoCertificationChange_CertificationNotchanged(int certificationEnum, bool hasSvane,
		bool hasBreeam, bool hasLeed, bool hasC2c, bool hasBlaue, bool hasFsc, bool hasPefc, bool hasIndeklima, bool hasSvaneByg, bool hasAstma, bool hasEpd, bool hasAllergyUk)
	{
		//Arrange
		ICollection<Product> products = new List<Product>()
		{
			new Product()
			{
				KatalogData= new List<Katalog>()
				{
					new()
					{
						EmneId = certificationEnum,
						isValid = false,
						Tunnr = 23
					}
				}
			}
		};

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.Equal(hasSvane, result.First().hasSvanemærke);
		Assert.Equal(hasBreeam, result.First().hasBREEAM);
		Assert.Equal(hasLeed, result.First().hasLEED);
		Assert.Equal(hasC2c, result.First().hasC2C);
		Assert.Equal(hasBlaue, result.First().hasDBE);
		Assert.Equal(hasFsc, result.First().hasFSC);
		Assert.Equal(hasPefc, result.First().hasPEFC);
		Assert.Equal(hasIndeklima, result.First().hasIndeKlima);
		Assert.Equal(hasSvaneByg, result.First().hasSvanemærkeByggeri);
		Assert.Equal(hasAstma, result.First().hasAstmaOgAllergi);
		Assert.Equal(hasEpd, result.First().hasEPD);
		Assert.Equal(hasAllergyUk, result.First().hasALUK);
	}

	[Theory]
	[InlineData((int)CertificationEnum.Svanemærke, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.BREEAM, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.LEED, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.Cradle2Cradle, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.DerBlaueEngel, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.FSC, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.PEFC, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.IndeKlima, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.SvanemærkeByggeri, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.AstmaOgAllergi, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.EPD, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.AllergyUK, false, false, false, false, false, false, false, false, false, false, false, false)]

	public void FindCertificationChanges_ProductWithInvalidCertification_CertificationChangedToFalse(int certificationEnum, bool hasSvane,
		bool hasBreeam, bool hasLeed, bool hasC2c, bool hasBlaue, bool hasFsc, bool hasPefc, bool hasIndeklima, bool hasSvaneByg, bool hasAstma, bool hasEpd, bool hasAllergyUk)
	{
		//Arrange
		ICollection<Product> products = new List<Product>()
		{
			new Product()
			{
				KatalogData= new List<Katalog>()
				{
					new()
					{
						EmneId = certificationEnum,
						isValid = false,
						Tunnr = 23
					}
				}
			}
		};

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.Equal(hasSvane, result.First().hasSvanemærke);
		Assert.Equal(hasBreeam, result.First().hasBREEAM);
		Assert.Equal(hasLeed, result.First().hasLEED);
		Assert.Equal(hasC2c, result.First().hasC2C);
		Assert.Equal(hasBlaue, result.First().hasDBE);
		Assert.Equal(hasFsc, result.First().hasFSC);
		Assert.Equal(hasPefc, result.First().hasPEFC);
		Assert.Equal(hasIndeklima, result.First().hasIndeKlima);
		Assert.Equal(hasSvaneByg, result.First().hasSvanemærkeByggeri);
		Assert.Equal(hasAstma, result.First().hasAstmaOgAllergi);
		Assert.Equal(hasEpd, result.First().hasEPD);
		Assert.Equal(hasAllergyUk, result.First().hasALUK);
	}

	[Theory]
	[InlineData((int)CertificationEnum.Svanemærke, true, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.BREEAM, false, true, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.LEED, false, false, true, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.Cradle2Cradle, false, false, false, true, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.DerBlaueEngel, false, false, false, false, true, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.FSC, false, false, false, false, false, true, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.PEFC, false, false, false, false, false, false, true, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.IndeKlima, false, false, false, false, false, false, false, true, false, false, false, false)]
	[InlineData((int)CertificationEnum.SvanemærkeByggeri, false, false, false, false, false, false, false, false, true, false, false, false)]
	[InlineData((int)CertificationEnum.AstmaOgAllergi, false, false, false, false, false, false, false, false, false, true, false, false)]
	[InlineData((int)CertificationEnum.EPD, false, false, false, false, false, false, false, false, false, false, true, false)]
	[InlineData((int)CertificationEnum.AllergyUK, false, false, false, false, false, false, false, false, false, false, false, true)]

	public void FindCertificationChanges_ProductWithValidCertification_CertificationChangedToValid(int certificationEnum, bool hasSvane,
		bool hasBreeam, bool hasLeed, bool hasC2c, bool hasBlaue, bool hasFsc, bool hasPefc, bool hasIndeklima, bool hasSvaneByg, bool hasAstma, bool hasEpd, bool hasAllergyUk)
	{
		//Arrange
		ICollection<Product> products = new List<Product>()
		{
			new Product()
			{
				KatalogData= new List<Katalog>()
				{
					new()
					{
						EmneId = certificationEnum,
						isValid = true,
						Tunnr = 23
					}
				}
			}
		};

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.Equal(hasSvane, result.First().hasSvanemærke);
		Assert.Equal(hasBreeam, result.First().hasBREEAM);
		Assert.Equal(hasLeed, result.First().hasLEED);
		Assert.Equal(hasC2c, result.First().hasC2C);
		Assert.Equal(hasBlaue, result.First().hasDBE);
		Assert.Equal(hasFsc, result.First().hasFSC);
		Assert.Equal(hasPefc, result.First().hasPEFC);
		Assert.Equal(hasIndeklima, result.First().hasIndeKlima);
		Assert.Equal(hasSvaneByg, result.First().hasSvanemærkeByggeri);
		Assert.Equal(hasAstma, result.First().hasAstmaOgAllergi);
		Assert.Equal(hasEpd, result.First().hasEPD);
		Assert.Equal(hasAllergyUk, result.First().hasALUK);
	}

	[Fact]
	public void FindCertificationChanges_WithEpd_ResultWithEPDsReturned()
	{
		//Arrange
		string pdfId = "testPdfId";
		decimal conversionFactor = 123;
		DateTime creationDate = DateTime.Now.AddMinutes(-2);
		string eN15084ACertification = "855";
		int ePDType = 8;
		string functionalUnit = "testFunctionalUnit";
		int functionalUnitAmount = 4;
		bool iSO14025Certified = true;
		bool iSO14040Certified = true;
		bool iSO14044Certified = true;
		DateTime pdfAppxDate = DateTime.Now.AddMinutes(-8);
		string pdfAppxName = "testpdfAppxName";
		DateTime pdfDate = DateTime.Now.AddMinutes(-11);
		string pdfName = "testpdfName";
		DateTime validFrom = DateTime.Now.AddMinutes(-17);
		DateTime validTo = DateTime.Now.AddMinutes(-17);
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
		int testC2_1 = 9876;
		int testC2_2 = 187;
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
		ICollection<Product> products = new List<Product>()
		{
			new Product()
			{
				EPDs= new List<EPD>()
				{
					new()
					{
						PdfId=pdfId,
						ConversionFactor=conversionFactor,
						CreationDate = creationDate,
						EN15084ACertification = eN15084ACertification,
						EPDType = ePDType,
						FunctionalUnit = functionalUnit,
						 FunctionalUnitAmount = functionalUnitAmount,
						 ISO14025Certified= iSO14025Certified,
						 ISO14040Certified = iSO14040Certified,
						 ISO14044Certified = iSO14044Certified,
						 PdfAppxDate = pdfAppxDate,
						 PdfAppxName = pdfAppxName,
						 PdfDate = pdfDate,
						 PdfName = pdfName,
						 ValidFrom = validFrom,
						 ServiceLifeAmount = serviceLifeAmount,
						 ValidTo = validTo,
						EPDIndicatorLines = new()
						{
							A1 = testA1,
							A2 = testA2,
							A3 = testA3,
							A1A3 = testA1A3,
							A4=testA4,
							A5=testA5,
							B1=testB1,
							B1B7=testB1B7,
							B1C1=testB1C1,
							B2=testB2,
							B2B7=testB2B7,
							B3=testB3,
							B3B7=testB3B7,
							B4=testB4,
							B5=testB5,
							B6=testB6,
							B7=testB7,
							C1=testC1,
							C2=testC2,
							C2_1=testC2_1,
							C2_2=testC2_2,
							C3=testC3,
							C3_1=testC3_1,
							C3_2=testC3_2,
							C4=testC4,
							C4_1=testC4_1,
							C4_2=testC4_2,
							D=testD,
							D_1=testD_1,
							D_2=testD_2,
							EPDHeaderId=testEPDHeaderId,
							Id=testId,
							Indicator=testIndicator,
							PhaseUnit=testPhaseUnit,
						}
					}
				}
			}
		};

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.Equal(pdfId, result.First().EPDs.First().PdfId);
		Assert.Equal(testA1, result.First().EPDs.First().EPDIndicatorLines!.A1);
	}
}
