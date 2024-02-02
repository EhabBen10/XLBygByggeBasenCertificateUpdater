//using CertificateUpdater.Services.Mapping;
//using CertificateUpdater.Services.Responses.GetProductChanges;
//using Xunit;

//namespace CertificateUpdater.Services.Test.Mapping;

//public sealed class GetKatalogChangesResponseToKatalogChangeUnitTest
//{
//	private GetProductChangesResponse getProductChangesResponse = new();
//	private readonly string _testCreated1 = Convert.ToString(new DateTime(2012, 1, 12));
//	private readonly string _testCreated2 = Convert.ToString(new DateTime(2011, 1, 12));
//	private readonly int _testEmneId = 2;
//	private readonly int _testTunnr = 3;

//	[Fact]
//	public void ToKatalogChanges_GetKatalogChangesResponseNull_ArgumentNullExceptionThrown()
//	{
//		// Arrange
//		getProductChangesResponse = null!;

//		// Act
//		var result = Record.Exception(getProductChangesResponse.ToKatalogChanges);

//		//Assert
//		Assert.IsType<ArgumentNullException>(result);
//	}

//	[Fact]
//	public void ToKatalogChanges_CatChangesDataNull_ArgumentNullExceptionThrown()
//	{
//		// Arrange
//		getProductChangesResponse.Result.CatChangesData.Add(null!);

//		// Act
//		var result = Record.Exception(getProductChangesResponse.ToKatalogChanges);

//		//Assert
//		Assert.IsType<ArgumentNullException>(result);
//	}

//	[Fact]
//	public void ToKatalogChanges_CatChangesContainsNonDateTimeForCreated_ExceptionThrown()
//	{
//		// Arrange
//		getProductChangesResponse.Result.Result.Add(new());

//		// Act
//		var result = Record.Exception(getProductChangesResponse.ToKatalogChanges);

//		//Assert
//		Assert.IsType<FormatException>(result);
//	}

//	//[Fact]
//	//public void ToKatalogChanges_CatChangesDataNotNull_MappedToKatalogChangesInCorrectOrder()
//	//{



//	//	// Act
//	//	var result = getKatalogChangesResponse.ToKatalogChanges();

//	//	//Assert
//	//	Assert.Equal(Convert.ToDateTime(catChange1.Created), result.First().CreatedAt);
//	//	Assert.Equal(catChange1.EmneId, result.First().EmneId);
//	//	Assert.Equal(catChange1.Tunnr, result.First().Tunnr);

//	//	Assert.Equal(Convert.ToDateTime(catChange2.Created), result.ToArray()[1].CreatedAt);
//	//	Assert.Equal(catChange2.EmneId, result.ToArray()[1].EmneId);
//	//	Assert.Equal(catChange2.Tunnr, result.ToArray()[1].Tunnr);

//	//	Assert.Equal(Convert.ToDateTime(catChange3.Created), result.Last().CreatedAt);
//	//	Assert.Equal(catChange3.EmneId, result.Last().EmneId);
//	//	Assert.Equal(catChange3.Tunnr, result.Last().Tunnr);
//	//}
//}
