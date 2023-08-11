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
		Assert.NotEmpty(result.First().dGNBDocuments);
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
			DGNBDocuments = _testDGNBDocumentData,
		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		//Assert
		Assert.Single(result.First().dGNBDocuments);
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
		});

		// Act
		var result = _getProductBatchResponse.ToProducts();

		//Assert
		Assert.Equal(2, result.First().dGNBDocuments.Count);
	}
}
