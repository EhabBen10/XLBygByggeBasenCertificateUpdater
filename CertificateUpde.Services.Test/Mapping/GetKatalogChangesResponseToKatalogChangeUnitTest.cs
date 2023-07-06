using CertificateUpdater.Services.Mapping;
using CertificateUpdater.Services.Responses.GetKatalogChanges;
using Xunit;

namespace CertificateUpdater.Services.Test.Mapping;

public sealed class GetKatalogChangesResponseToKatalogChangeUnitTest
{
	private GetKatalogChangesResponse getKatalogChangesResponse = new();
	private readonly string _testCreated1 = Convert.ToString(new DateTime(2012, 1, 12));
	private readonly string _testCreated2 = Convert.ToString(new DateTime(2011, 1, 12));
	private readonly int _testEmneId = 2;
	private readonly int _testTunnr = 3;

	[Fact]
	public void ToKatalogChanges_GetKatalogChangesResponseNull_ArgumentNullExceptionThrown()
	{
		// Arrange
		getKatalogChangesResponse = null!;

		// Act
		var result = Record.Exception(getKatalogChangesResponse.ToKatalogChanges);

		//Assert
		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void ToKatalogChanges_CatChangesDataNull_ArgumentNullExceptionThrown()
	{
		// Arrange
		getKatalogChangesResponse.Result.CatChangesData.Add(null!);

		// Act
		var result = Record.Exception(getKatalogChangesResponse.ToKatalogChanges);

		//Assert
		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void ToKatalogChanges_CatChangesContainsNonDateTimeForCreated_ExceptionThrown()
	{
		// Arrange
		getKatalogChangesResponse.Result.CatChangesData.Add(new()
		{
			Created = "Not a DateTime"
		});

		// Act
		var result = Record.Exception(getKatalogChangesResponse.ToKatalogChanges);

		//Assert
		Assert.IsType<FormatException>(result);
	}

	[Fact]
	public void ToKatalogChanges_CatChangesDataNotNull_MappedToKatalogChangesInCorrectOrder()
	{
		// Arrange
		CatChangeData catChange1 = new()
		{
			Created = _testCreated1,
			EmneId = _testEmneId,
			Tunnr = _testTunnr,
		};
		CatChangeData catChange2 = new()
		{
			Created = _testCreated2,
			EmneId = _testEmneId + 1,
			Tunnr = _testTunnr + 1,
		};
		CatChangeData catChange3 = new()
		{
			Created = _testCreated2,
			EmneId = _testEmneId + 2,
			Tunnr = 4,
		};
		getKatalogChangesResponse.Result.CatChangesData.Add(catChange1);
		getKatalogChangesResponse.Result.CatChangesData.Add(catChange2);
		getKatalogChangesResponse.Result.CatChangesData.Add(catChange3);

		// Act
		var result = getKatalogChangesResponse.ToKatalogChanges();

		//Assert
		Assert.Equal(Convert.ToDateTime(catChange1.Created), result.First().CreatedAt);
		Assert.Equal(catChange1.EmneId, result.First().EmneId);
		Assert.Equal(catChange1.Tunnr, result.First().Tunnr);

		Assert.Equal(Convert.ToDateTime(catChange2.Created), result.ToArray()[1].CreatedAt);
		Assert.Equal(catChange2.EmneId, result.ToArray()[1].EmneId);
		Assert.Equal(catChange2.Tunnr, result.ToArray()[1].Tunnr);

		Assert.Equal(Convert.ToDateTime(catChange3.Created), result.Last().CreatedAt);
		Assert.Equal(catChange3.EmneId, result.Last().EmneId);
		Assert.Equal(catChange3.Tunnr, result.Last().Tunnr);
	}
}
