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
	public void FindCertificationChanges_ProductsNotEmptyButNoKatalogData_NoChangesAdded()
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
				ProductGroupId=_testVareGruppeId,
			}
		};

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.NotEmpty(result);
	}

	[Fact]
	public void FindCertificationChanges_ProductsNotEmptyAndKatalogDataNotValid_ChangesAdded()
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
				ProductGroupId=_testVareGruppeId,
				KatalogData= new List<Katalog>()
				{
					new()
					{
						EmneId=10000,
					}
				}
			}
		};

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.NotEmpty(result);
	}

	[Fact]
	public void FindCertificationChanges_ProductsNotEmptyAndKatalogDataIsValid_ChangeAdded()
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
				ProductGroupId=_testVareGruppeId,
				KatalogData= new List<Katalog>()
				{
					new()
					{
						EmneId=Convert.ToInt32(CertificationEnum.PEFC),
						isValid=true
					}
				}
			}
		};

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.NotEmpty(result);
		Assert.Equal(_testCompanyName, result.FirstOrDefault().CompanyName);
		Assert.Equal(_testDBNr, result.FirstOrDefault().DBNr);
		Assert.Equal(_testSupplierNr, result.FirstOrDefault().SupplierNr);
		Assert.Equal(_testProductText, result.FirstOrDefault().ProductText);
		Assert.Equal(_testVareGruppeId, result.FirstOrDefault().ProductGroupId);
		Assert.True(result.FirstOrDefault().hasPEFC);
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
	[InlineData((int)CertificationEnum.Svanemærke, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.BREEAM, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.LEED, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.Cradle2Cradle, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.DerBlaueEngel, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.FSC, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.PEFC, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.IndeKlima, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.SvanemærkeByggeri, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.AstmaOgAllergi, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.EPD, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.AllergyUK, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.M1, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.GlobalCompact, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.EUTR, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.GEV_EMICODE, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.NEMKO, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.SITAC, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.EUBlomsten, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.Energimærkning, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	public void FindCertificationChanges_ProductWithInvalidCertification_CertificationChangedToFalse(int certificationEnum, bool hasSvane,
		bool hasBreeam, bool hasLeed, bool hasC2c, bool hasBlaue, bool hasFsc, bool hasPefc, bool hasIndeklima, bool hasSvaneByg, bool hasAstma, bool hasEpd, bool hasAllergyUk,
		bool hasM1, bool hasGlobalCompact, bool hasEUTR, bool hasGEV_EMICODE, bool hasNEMKO, bool hasSITAC, bool hasEublomsten, bool hasEnergimærkning)
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
		Assert.Equal(hasEUTR, result.First().hasEUTR);
		Assert.Equal(hasGEV_EMICODE, result.First().hasGEV_EMICODE);
		Assert.Equal(hasGlobalCompact, result.First().hasGlobalCompact);
		Assert.Equal(hasM1, result.First().hasM1);
		Assert.Equal(hasNEMKO, result.First().hasNEMKO);
		Assert.Equal(hasSITAC, result.First().hasSITAC);
		Assert.Equal(hasEublomsten, result.First().hasEUBlomsten);
		Assert.Equal(hasEnergimærkning, result.First().hasEnergiMærkning);
	}

	[Theory]
	[InlineData((int)CertificationEnum.Svanemærke, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.BREEAM, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.LEED, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.Cradle2Cradle, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.DerBlaueEngel, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.FSC, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.PEFC, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.IndeKlima, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.SvanemærkeByggeri, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.AstmaOgAllergi, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.EPD, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.AllergyUK, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.M1, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.GlobalCompact, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.EUTR, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false, false)]
	[InlineData((int)CertificationEnum.GEV_EMICODE, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false, false)]
	[InlineData((int)CertificationEnum.NEMKO, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false, false)]
	[InlineData((int)CertificationEnum.SITAC, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false)]
	[InlineData((int)CertificationEnum.EUBlomsten, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false)]
	[InlineData((int)CertificationEnum.Energimærkning, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true)]
	public void FindCertificationChanges_ProductWithValidCertification_CertificationChangedToValid(int certificationEnum, bool hasSvane,
		bool hasBreeam, bool hasLeed, bool hasC2c, bool hasBlaue, bool hasFsc, bool hasPefc, bool hasIndeklima, bool hasSvaneByg, bool hasAstma, bool hasEpd, bool hasAllergyUk,
		bool hasM1, bool hasGlobalCompact, bool hasEUTR, bool hasGEV_EMICODE, bool hasNEMKO, bool hasSITAC, bool hasEublomsten, bool hasEnergimærkning)
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
		Assert.Equal(hasEUTR, result.First().hasEUTR);
		Assert.Equal(hasGEV_EMICODE, result.First().hasGEV_EMICODE);
		Assert.Equal(hasGlobalCompact, result.First().hasGlobalCompact);
		Assert.Equal(hasM1, result.First().hasM1);
		Assert.Equal(hasNEMKO, result.First().hasNEMKO);
		Assert.Equal(hasSITAC, result.First().hasSITAC);
		Assert.Equal(hasEublomsten, result.First().hasEUBlomsten);
		Assert.Equal(hasEnergimærkning, result.First().hasEnergiMærkning);
	}
}
