using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Enum;
using CertificateUpdater.Services.Finders;
using CertificateUpdater.Services.Interfaces;
using Xunit;

namespace CertificateUpde.Services.Test;

public class CertificationChangeFinderUnitTest
{
	private ICertificationChangeFinder _uut = new CertificationChangeFinder();
	private string _testCompanyName = "testCompanyName";
	private string _testProductText = "testProductText";
	private string _testSupplierNr = "testSupplierNr";

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
				DBNr = 1,
				ProductText= _testProductText,
				SupplierNr=_testSupplierNr,
			}
		};

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.NotEmpty(result);
		Assert.Equal(_testCompanyName, result.First().CompanyName);
		Assert.Equal(_testProductText, result.First().ProductText);
		Assert.Equal(_testSupplierNr, result.First().SupplierNr);
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
	[Fact]
	public void FindCertificationChanges_ProductWithSvanemærk_CertificationChangeSvanemærke()
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
						EmneId= (int)CertificationEnum.Svanemærke,
						isValid=true,
						Tunnr=23
					}
				}
			}
		};

		//Act
		var result = _uut.FindCertificationChanges(products);

		//Assert
		Assert.Equal("1:2", result.First().DGNBQualityStep.First());
	}
}
