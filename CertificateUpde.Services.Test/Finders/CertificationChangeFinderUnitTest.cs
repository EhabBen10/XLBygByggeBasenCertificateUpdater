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
				dGNBDocuments= new List<DGNBDocument>()
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

	public void FindCertificationChanges_ProductWithValid_CertificationChangedToValid(int certificationEnum, bool hasSvane,
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
}
