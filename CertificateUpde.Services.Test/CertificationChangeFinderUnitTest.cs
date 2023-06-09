using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Services.Finders;
using CertificateUpdater.Services.Interfaces;
using Xunit;

namespace CertificateUpde.Services.Test;

public class CertificationChangeFinderUnitTest
{
	private ICertificationChangeFinder _uut = new CertificationChangeFinder();
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
	public void FindCertificationChanges_KataLogEmpty_ExceptionThrown()
	{
		//Arrange
		ICollection<Product> products = new List<Product>();

		//Act
		var result = Record.Exception(() => _uut.FindCertificationChanges(products));

		//Assert
		Assert.IsType<NullReferenceException>(result);
	}

	[Fact]
	public void FindCertificationChanges_ProductWithChange_CertificationChangeContainsNewChange()
	{

	}
}
