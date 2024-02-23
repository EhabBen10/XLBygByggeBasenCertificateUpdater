using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Services.Finders;
using CertificateUpdater.Services.Interfaces;
using Xunit;

namespace CertificateUpdater.Services.Test.Finders;
public sealed class HazardChangeFinderUnitTest
{
	private IHazardChangeFinder _uut = new HazardChangeFinder();
	private ICollection<Product> _testProducts = new List<Product>();
	[Fact]
	public void FindHazardChanges_WithoutProduct_Resultempty()
	{
		// Act
		var result = _uut.FindHazardChanges(_testProducts);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void FindHazardChanges_ProductWithoutHazardMark_ResultEmpty()
	{
		// Arrange
		_testProducts.Add(new()
		{
			SupplierNr = "123"
		});
		// Act
		var result = _uut.FindHazardChanges(_testProducts);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void FindHazardChanges_ProductWithHazardMark_ResultNotEmptyAndWithHazardInfo()
	{
		// Arrange
		HazardInfo testHazardInfo = new()
		{
			HazardMark = "testMark",
		};
		_testProducts.Add(new()
		{
			SupplierNr = "123",
			HazardInfo = new()
			{
				HazardMark = "testMark",
			}
		});
		// Act
		var result = _uut.FindHazardChanges(_testProducts);

		// Assert
		Assert.NotEmpty(result);
		Assert.Equal(testHazardInfo.HazardMark, result.First().HazardMark);
	}
}
