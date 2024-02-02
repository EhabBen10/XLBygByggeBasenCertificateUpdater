using CertificateUpdater.Domain.Shared;
using CertificateUpdater.Services.Interfaces;
using CertificateUpdater.Services.Responses.GetProductChanges;
using CertificateUpdater.Services.Services;
using CertificateUpdater.Services.Settings;
using Moq;
using RestSharp;
using Xunit;

namespace CertificateUpdater.Services.Test.Services;
public sealed class GetKatalogChangesServiceUnitTest
{
	private readonly Mock<IClient<BaseSettings>> _clientMock = new();
	private readonly Mock<ILogProvider> _logProviderMock = new();
	private readonly Mock<ICredentialProvider> _credentialProviderMock = new();
	private readonly Mock<IResponse<GetProductChangesResponse>> _responseMock = new();
	private IGetProductChangesService _uut;
	private readonly string _testLog = "testLog";
	private readonly string _testUserName = "userName";
	private readonly string _testPassword = "testPassword";
	private readonly int _testGetTunUserNr = 123;
	public GetKatalogChangesServiceUnitTest()
	{
		_uut = new GetProductChangesService(_clientMock.Object, _logProviderMock.Object, _credentialProviderMock.Object);
	}

	[Fact]
	public void Construcor_ClientNull_ArgumentNullExceptionThrown()
	{
		// Arrange 

		// Act
		var result = Record.Exception(() => _uut = new GetProductChangesService(null!, _logProviderMock.Object, _credentialProviderMock.Object));

		// Assert 
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains("restClient", result.Message);
	}

	[Fact]
	public void Construcor_LogProviderNull_ArgumentNullExceptionThrown()
	{
		// Arrange 

		// Act
		var result = Record.Exception(() => _uut = new GetProductChangesService(_clientMock.Object, null!, _credentialProviderMock.Object));

		// Assert 
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains("logProvider", result.Message);
	}

	[Fact]
	public void Construcor_CredentialProviderNull_ArgumentNullExceptionThrown()
	{
		// Arrange 

		// Act
		var result = Record.Exception(() => _uut = new GetProductChangesService(_clientMock.Object, _logProviderMock.Object, null!));

		// Assert 
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains("credentialProvider", result.Message);
	}

	[Fact]
	public async void GetKatalogChanges_ResponseGetResultReturnsFailure_ResultIsFailure()
	{
		// Arrange 

		CancellationToken cancellationToken = new();
		_logProviderMock
			.Setup(x => x.GetLastLog()).Returns(_testLog);

		_credentialProviderMock
			.Setup(x => x.GetPassword()).Returns(_testPassword);

		_credentialProviderMock
			.Setup(x => x.GetUserName()).Returns(_testUserName);

		_credentialProviderMock
			.Setup(x => x.GetTunUserNr()).Returns(_testGetTunUserNr);

		_clientMock
			.Setup(x => x.PostAsync<GetProductChangesResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(_responseMock.Object);

		_responseMock
			.Setup(x => x.GetResult(
				It.IsAny<Func<GetProductChangesResponse, ICollection<int>>>()))
			.Returns(Result.Failure<ICollection<int>>(Error.NullValue));

		// Act
		var result = await _uut.GetProductChanges(cancellationToken);

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
	public async void GetKatalogChanges_CredentialProviderNull_ArgumentNullExceptionThrown()
	{
		// Arrange 

		CancellationToken cancellationToken = new();
		_logProviderMock
			.Setup(x => x.GetLastLog()).Returns(_testLog);

		_credentialProviderMock
			.Setup(x => x.GetPassword()).Returns(_testPassword);

		_credentialProviderMock
			.Setup(x => x.GetUserName()).Returns(_testUserName);

		_credentialProviderMock
			.Setup(x => x.GetTunUserNr()).Returns(_testGetTunUserNr);

		_clientMock
			.Setup(x => x.PostAsync<GetProductChangesResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(_responseMock.Object);
		ICollection<int> expectedResult = new List<int>()
		{
			24,
			12
		};
		_responseMock
			.Setup(x => x.GetResult(
				It.IsAny<Func<GetProductChangesResponse, ICollection<int>>>()))
			.Returns(Result.Success(expectedResult));

		// Act
		var result = await _uut.GetProductChanges(cancellationToken);

		// Assert 
		Assert.False(result.IsFailure);
		Assert.True(result.IsSuccess);
		Assert.Equal(expectedResult, result.Value);

		_credentialProviderMock
			.Verify(x => x.GetUserName(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetPassword(), Times.Once());
		_credentialProviderMock
			.Verify(x => x.GetTunUserNr(), Times.Once());
	}
}
