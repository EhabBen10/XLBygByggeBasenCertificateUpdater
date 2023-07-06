using CertificateUpdater.Domain.Entities;
using CertificateUpdater.Domain.Shared;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Mapping;
using CertificateUpdater.Services.Responses.GetProductBatch;
using CertificateUpdater.Services.Services;
using CertificateUpdater.Services.Settings;
using Moq;
using RestSharp;
using Xunit;

namespace CertificateUpdater.Services.Test.Services;
public sealed class GetProduktBatchServiceUnitTest
{
	private readonly Mock<IClient<BaseSettings>> _clientMock = new();
	private readonly Mock<ICredentialProvider> _credentialProviderMock = new();
	private IGetProductBatchService _uut;
	private readonly CancellationToken _token = new();
	private readonly ICollection<int> _tunnrs = new List<int>();
	private readonly Mock<IResponse<GetProductBatchResponse>> _responseMock = new();

	private readonly string _testUserName = "userName";
	private readonly string _testPassword = "testPassword";
	private readonly int _testGetTunUserNr = 123;
	private readonly string _testSupplierNr = "testSupplierNr";
	private readonly string _testCompanyName = "testCompanyName";
	private readonly int _testDBNr = 12;
	private readonly string _testProductText1 = "testProductText";
	private readonly ICollection<DGNBDocument> _testDGNBDocument = new List<DGNBDocument>();
	private readonly ICollection<Katalog> _testKatalog = new List<Katalog>();

	public GetProduktBatchServiceUnitTest()
	{
		_uut = new GetProductBatchService(_clientMock.Object, _credentialProviderMock.Object);
	}

	[Fact]
	public void Construcor_ClientNull_ArgumentNullExceptionThrown()
	{
		// Arrange 

		// Act
		var result = Record.Exception(() => _uut = new GetProductBatchService(null!, _credentialProviderMock.Object));

		// Assert 
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains("restClient", result.Message);
	}

	[Fact]
	public void Construcor_CredentialProviderNull_ArgumentNullExceptionThrown()
	{
		// Arrange 

		// Act
		var result = Record.Exception(() => _uut = new GetProductBatchService(_clientMock.Object, null!));

		// Assert 
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains("credentialProvider", result.Message);
	}

	[Fact]
	public async void GetProductBatch_TunnrsIsEmpty_NullValueErrorIsReturned()
	{
		// Arrange
		_credentialProviderMock
			.Setup(x => x.GetPassword()).Returns(_testPassword);

		_credentialProviderMock
			.Setup(x => x.GetUserName()).Returns(_testUserName);

		_credentialProviderMock
			.Setup(x => x.GetTunUserNr()).Returns(_testGetTunUserNr);

		// Act
		var result = await _uut.GetProductBatch(_tunnrs, _token);

		// Assert 
		Assert.True(result.IsFailure);
		Assert.False(result.IsSuccess);
		Assert.Equal(Error.NullValue, result.Error);

		_credentialProviderMock
			.Verify(x => x.GetUserName(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetPassword(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetTunUserNr(), Times.Once());
	}

	[Fact]
	public async void GetProductBatch_TunnrsIsBelow1000ClientReturnsFailure_NullValueErrorIsReturned()
	{
		// Arrange
		_credentialProviderMock
			.Setup(x => x.GetPassword()).Returns(_testPassword);

		_credentialProviderMock
			.Setup(x => x.GetUserName()).Returns(_testUserName);

		_credentialProviderMock
			.Setup(x => x.GetTunUserNr()).Returns(_testGetTunUserNr);

		_clientMock
			.Setup(x => x.PostAsync<GetProductBatchResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(_responseMock.Object);

		_responseMock
			.Setup(x => x.GetResult(
				It.IsAny<Func<GetProductBatchResponse, ICollection<Product>>>()))
			.Returns(Result.Failure<ICollection<Product>>(Error.NullValue));

		_tunnrs.Add(12);

		// Act
		var result = await _uut.GetProductBatch(_tunnrs, _token);

		// Assert 
		Assert.True(result.IsFailure);
		Assert.False(result.IsSuccess);
		Assert.Equal(Error.NullValue, result.Error);

		_clientMock
			.Verify(x => x
				.PostAsync<GetProductBatchResponse>(
					It.IsAny<RestRequest>(),
					_token),
				Times.Once);

		_responseMock
			.Verify(x => x
				.GetResult(getProduktBatchResponseToProducts.ToProducts), Times.Once);

		_credentialProviderMock
			.Verify(x => x.GetUserName(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetPassword(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetTunUserNr(), Times.Once());
	}

	[Fact]
	public async void GetProductBatch_TunnrsIsAbove1000ClientReturnsFailure_Error500IsReturned()
	{
		// Arrange
		_credentialProviderMock
			.Setup(x => x.GetPassword()).Returns(_testPassword);

		_credentialProviderMock
			.Setup(x => x.GetUserName()).Returns(_testUserName);

		_credentialProviderMock
			.Setup(x => x.GetTunUserNr()).Returns(_testGetTunUserNr);

		_clientMock
			.Setup(x => x.PostAsync<GetProductBatchResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(_responseMock.Object);

		_responseMock
			.Setup(x => x.GetResult(
				It.IsAny<Func<GetProductBatchResponse, ICollection<Product>>>()))
			.Returns(Result.Failure<ICollection<Product>>(Error.NullValue));

		for (int i = 0; i < 2000; i++)
		{
			_tunnrs.Add(i);
		}

		// Act
		var result = await _uut.GetProductBatch(_tunnrs, _token);

		// Assert 
		Assert.True(result.IsFailure);
		Assert.False(result.IsSuccess);
		Assert.Equal(new Error("500", "An unexpected error occured"), result.Error);

		_clientMock
			.Verify(x => x
				.PostAsync<GetProductBatchResponse>(
					It.IsAny<RestRequest>(),
					_token),
				Times.Once);

		_responseMock
			.Verify(x => x
				.GetResult(getProduktBatchResponseToProducts.ToProducts), Times.Once);

		_credentialProviderMock
			.Verify(x => x.GetUserName(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetPassword(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetTunUserNr(), Times.Once());
	}

	[Fact]
	public async void GetProductBatch_TunnrsIsAbove1000ClientSuccess_SuccessWithExpectedResultReturned()
	{
		// Arrange
		_credentialProviderMock
			.Setup(x => x.GetPassword()).Returns(_testPassword);

		_credentialProviderMock
			.Setup(x => x.GetUserName()).Returns(_testUserName);

		_credentialProviderMock
			.Setup(x => x.GetTunUserNr()).Returns(_testGetTunUserNr);

		_clientMock
			.Setup(x => x.PostAsync<GetProductBatchResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(_responseMock.Object);

		ICollection<Product> responseResult = new List<Product>()
		{
			new()
			{
				CompanyName = _testCompanyName,
				DBNr = _testDBNr,
				dGNBDocuments=_testDGNBDocument,
				KatalogData= _testKatalog,
				ProductText=_testProductText1,
				SupplierNr=_testSupplierNr,
			}
		};
		ICollection<Product> expectedResult = new List<Product>()
		{
			new()
			{
				CompanyName = _testCompanyName,
				DBNr = _testDBNr,
				dGNBDocuments=_testDGNBDocument,
				KatalogData= _testKatalog,
				ProductText=_testProductText1,
				SupplierNr=_testSupplierNr,
			},
			new()
			{
				CompanyName = _testCompanyName,
				DBNr = _testDBNr,
				dGNBDocuments=_testDGNBDocument,
				KatalogData= _testKatalog,
				ProductText=_testProductText1,
				SupplierNr=_testSupplierNr,
			}
		};

		_responseMock
			.Setup(x => x.GetResult(
				It.IsAny<Func<GetProductBatchResponse, ICollection<Product>>>()))
			.Returns(Result.Success(responseResult));

		for (int i = 0; i < 2000; i++)
		{
			_tunnrs.Add(i);
		}

		// Act
		var result = await _uut.GetProductBatch(_tunnrs, _token);

		// Assert 
		Assert.False(result.IsFailure);
		Assert.True(result.IsSuccess);
		Assert.Equal(expectedResult, result.Value);

		_clientMock
			.Verify(x => x
				.PostAsync<GetProductBatchResponse>(
					It.IsAny<RestRequest>(),
					_token),
				Times.Exactly(2));

		_responseMock
			.Verify(x => x
				.GetResult(getProduktBatchResponseToProducts.ToProducts), Times.Exactly(2));

		_credentialProviderMock
			.Verify(x => x.GetUserName(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetPassword(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetTunUserNr(), Times.Once());
	}

	[Fact]
	public async void GetProductBatch_TunnrsIsBelow1000ClientSuccess_SuccessWithExpectedResultReturned()
	{
		// Arrange
		_credentialProviderMock
			.Setup(x => x.GetPassword()).Returns(_testPassword);

		_credentialProviderMock
			.Setup(x => x.GetUserName()).Returns(_testUserName);

		_credentialProviderMock
			.Setup(x => x.GetTunUserNr()).Returns(_testGetTunUserNr);

		_clientMock
			.Setup(x => x.PostAsync<GetProductBatchResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(_responseMock.Object);

		ICollection<Product> expectedResult = new List<Product>()
		{
			new()
			{
				CompanyName = _testCompanyName,
				DBNr = _testDBNr,
				dGNBDocuments=_testDGNBDocument,
				KatalogData= _testKatalog,
				ProductText=_testProductText1,
				SupplierNr=_testSupplierNr,
			}
		};
		_responseMock
			.Setup(x => x.GetResult(
				It.IsAny<Func<GetProductBatchResponse, ICollection<Product>>>()))
			.Returns(Result.Success(expectedResult));

		_tunnrs.Add(12);

		// Act
		var result = await _uut.GetProductBatch(_tunnrs, _token);

		// Assert 
		Assert.False(result.IsFailure);
		Assert.True(result.IsSuccess);
		Assert.Equal(expectedResult, result.Value);
		_clientMock
			.Verify(x => x
				.PostAsync<GetProductBatchResponse>(
					It.IsAny<RestRequest>(),
					_token),
				Times.Once);

		_responseMock
			.Verify(x => x
				.GetResult(getProduktBatchResponseToProducts.ToProducts), Times.Once);

		_credentialProviderMock
			.Verify(x => x.GetUserName(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetPassword(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetTunUserNr(), Times.Once());
	}
}
