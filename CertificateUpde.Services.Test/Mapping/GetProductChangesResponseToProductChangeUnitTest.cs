using CertificateUpdater.Services.Mapping;
using CertificateUpdater.Services.Responses.GetProductChanges;
using Xunit;

namespace CertificateUpdater.Services.Test.Mapping;

public sealed class GetProductChangesResponseToProductChangeUnitTest
{
	private GetProductChangesResponse getProductChangesResponse = new();
	private readonly string _testCreated1 = Convert.ToString(new DateTime(2012, 1, 12));
	private readonly string _testCreated2 = Convert.ToString(new DateTime(2011, 1, 12));
	private readonly int _testEmneId = 2;
	private readonly int _testTunnr = 3;

	[Fact]
	public void ToProductChanges_GetProductChangesResponseNull_ArgumentNullExceptionThrown()
	{
		// Arrange
		getProductChangesResponse = null!;

		// Act
		var result = Record.Exception(getProductChangesResponse.ToProductChanges);

		//Assert
		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void ToProductChanges_GetProductChangesDataNull_ArgumentNullExceptionThrown()
	{
		// Arrange
		getProductChangesResponse.Result.Result.Add(0);

		// Act
		var result = Record.Exception(getProductChangesResponse.ToProductChanges);

		//Assert
		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void ToProductChanges_GetProductChangesEmptyList_ArgumentNullExceptionThrown()
	{
		// Arrange
		getProductChangesResponse.Result.Result.Add(new());

		// Act
		var result = Record.Exception(getProductChangesResponse.ToProductChanges);

		//Assert
		Assert.IsType<ArgumentNullException>(result);
	}
	[Fact]
	public void ToProductChanges_GetProductChangesContainsProductDBNr_ExceptionThrown()
	{
		// Arrange
		getProductChangesResponse.Result.Result.Add(1);

		// Act
		var result = getProductChangesResponse.ToProductChanges();

		//Assert
		Assert.Equal(getProductChangesResponse.Result.Result, result);
	}
}
